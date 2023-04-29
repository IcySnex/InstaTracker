using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using Serilog;
using System;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly AccountDatabaseConnection database;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly SettingsViewModel settingsViewModel;
    readonly SearchViewModel searchViewModel;

    public Config Config { get; }
    public AccountManager AccountManager { get; }

    public AccountViewModel(
        ILogger logger,
        Config config,
        AccountDatabaseConnection database,
        Message message,
        SnackBar snackBar,
        AccountManager accountManager,
        SettingsViewModel settingsViewModel,
        SearchViewModel searchViewModel)
    {
        this.logger = logger;
        this.Config = config;
        this.database = database;
        this.message = message;
        this.snackBar = snackBar;
        this.AccountManager = accountManager;
        this.settingsViewModel = settingsViewModel;
        this.searchViewModel = searchViewModel;
    }

    public async Task InitializeAsync()
    {
        if (Config.AutoLoginId.HasValue && await database.GetAsync(Config.AutoLoginId.Value) is Account savedLogin)
            await snackBar.RunAsync(
                "Logging in...",
                Config.LoginWithState ?
                    AccountManager.LoginAsync(savedLogin.StateJson, false) :
                    AccountManager.LoginAsync(savedLogin.Username, savedLogin.Password!, false),
                snackBar.ErrorCallback());

        await snackBar.RunAsync(
            "Loading saved accounts...",
            LoadSavedAccountsAsync(),
            snackBar.ErrorCallback());

        logger.Log("Initialized AccountViewModel");
    }


    [ObservableProperty]
    Account[] savedAccounts = Array.Empty<Account>();

    async Task LoadSavedAccountsAsync()
    {
        SavedAccounts = await database.GetAllAsync();
        settingsViewModel.ReloadAccountSettings(SavedAccounts);
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task RemoveSavedAccountAsync(
        int id)
    {
        if (!await snackBar.RunAsync(
            "Removing saved account...",
            database.RemoveAsync(id),
            snackBar.ErrorCallback()))
            return;

        await snackBar.RunAsync(
            "Loading saved accounts...",
            LoadSavedAccountsAsync(),
            snackBar.ErrorCallback());
    }


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    string username = default!;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
    string password = default!;

    bool CanLoginCommandExecute(Account? account) =>
        account is null ? !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password) : true;

    [RelayCommand(CanExecute = nameof(CanLoginCommandExecute))]
    async Task LoginAsync(
        Account? account = null)
    {
        if (!await snackBar.RunAsync(
                "Logging in...",
                account is null ?
                    AccountManager.LoginAsync(Username, Password, Config.SaveAccount) :
                    Config.LoginWithState ?
                        AccountManager.LoginAsync(account.StateJson, false) :
                        AccountManager.LoginAsync(account.Username, account.Password!, false),
                snackBar.ErrorCallback("Make sure you entered your correct username and password and disable 2FA if enabled.")))
            return;

        if (Config.SaveAccount && account is null)
            await snackBar.RunAsync(
                "Loading saved accounts...",
                LoadSavedAccountsAsync(),
                snackBar.ErrorCallback());

        searchViewModel.IsRefreshing = true;
        await snackBar.RunAsync(
            "Loading search history...",
            searchViewModel.LoadSearchHistory(true),
            snackBar.ErrorCallback(),
            refreshed =>
            {
                if (refreshed)
                    return;

                snackBar.ErrorCallback("Could not reload account information from Instagram, instead search history from local database was restored.").Invoke(new("Failed refreshing search history!", new("No account is currently logged in.")));
            });
        searchViewModel.IsRefreshing = false;
    }


    [RelayCommand]
    async Task LogoutAsync()
    {
        await snackBar.RunAsync(
            "Logging out...",
            AccountManager.LogoutAsync(),
            snackBar.ErrorCallback());
    }
}