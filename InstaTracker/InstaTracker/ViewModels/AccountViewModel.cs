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

    public Config Config { get; }
    public AccountManager AccountManager { get; }

    public AccountViewModel(
        ILogger logger,
        Config config,
        AccountDatabaseConnection database,
        Message message,
        SnackBar snackBar,
        AccountManager accountManager,
        SettingsViewModel settingsViewModel)
    {
        this.logger = logger;
        this.Config = config;
        this.database = database;
        this.message = message;
        this.snackBar = snackBar;
        this.AccountManager = accountManager;
        this.settingsViewModel = settingsViewModel;
    }

    public async Task InitializeAsync()
    {
        if (Config.AutoLoginId.HasValue && await database.GetAsync(Config.AutoLoginId.Value) is Account savedLogin)
            await snackBar.RunAsync(
                "Logging in...",
                AccountManager.LoginAsync(savedLogin.StateJson, false),
                snackBar.ErrorCallback("Failed logging in!"));

        await snackBar.RunAsync(
            "Loading saved accounts...",
            LoadSavedAccountsAsync(),
            snackBar.ErrorCallback("Failed loading saved accounts!"));

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
        await snackBar.RunAsync(
            "Removing saved account...",
            database.RemoveAsync(id),
            snackBar.ErrorCallback("Failed removing account!"));


        await snackBar.RunAsync(
            "Loading saved accounts...",
            LoadSavedAccountsAsync(),
            snackBar.ErrorCallback("Failed loading saved accounts!"));
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
        await snackBar.RunAsync(
            "Logging in...",
            account is null ? AccountManager.LoginAsync(Username, Password, Config.SaveAccount) : AccountManager.LoginAsync(account.StateJson, false),
            snackBar.ErrorCallback("Failed logging in!", "Make sure you entered your correct username and password and disable 2FA if enabled."));

        if (Config.SaveAccount && account is null)
            await snackBar.RunAsync(
                "Loading saved accounts...",
                LoadSavedAccountsAsync(),
                snackBar.ErrorCallback("Failed loading saved accounts!"));
    }


    [RelayCommand]
    async Task LogoutAsync()
    {
        await snackBar.RunAsync(
            "Logging out...",
            AccountManager.LogoutAsync(),
            snackBar.ErrorCallback("Failed logging out!"));
    }
}