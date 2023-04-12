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
            await ExecuteNotifiedAsync(
                AccountManager.LoginAsync(savedLogin.StateJson, false),
                "Logging in...",
                "Failed to login!",
                "The saved login information is not valid. Please make sure your selected account's username and password is correct and if two-factor authorization is enabled, please disable it.\n");

        await LoadSavedAccountsAsync();

        logger.Log("Initialized AccountViewModel");
    }


    async Task ExecuteNotifiedAsync(
        Task task,
        string startingMessage,
        string failedTitle,
        string failedMessage)
    {
        try
        {
            snackBar.Show(startingMessage, null, 2000, awaitPreviousSnackBar: true);
            await task;
        }
        catch (Exception ex)
        {
            snackBar.Show(failedTitle, "More", 10000, onButtonClicked: async () => await message.ShowAsync(failedTitle, $"{failedMessage}({ex.Message})"));
        }
    }


    [ObservableProperty]
    Account[] savedAccounts = Array.Empty<Account>();

    async Task LoadSavedAccountsAsync()
    {
        try
        {
            snackBar.Show("Loading saved accounts...", null, 2000, awaitPreviousSnackBar: true);
            SavedAccounts = await database.GetAllAsync();
            settingsViewModel.ReloadAccountSettings(SavedAccounts);
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed loading saved accounts!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed loading saved accounts!", $"({ex.Message})"));
        }
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
        await ExecuteNotifiedAsync(
            account is null ? AccountManager.LoginAsync(Username, Password, Config.SaveAccount) : AccountManager.LoginAsync(account.StateJson, false),
            "Logging in...",
            "Failed to login!",
            "Please make sure you entered your correct username and password and if two-factor authorization is enabled, please disable it.\n\n");

        if (Config.SaveAccount && account is null)
            await LoadSavedAccountsAsync();
    }


    [RelayCommand]
    async Task LogoutAsync() =>
        await ExecuteNotifiedAsync(
            AccountManager.LogoutAsync(),
            "Logging out...",
            "Failed to log out!",
            "");


    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task RemoveSavedAccountAsync(
        int id)
    {
        await ExecuteNotifiedAsync(
            database.RemoveAsync(id),
            "Removing saved account...",
            "Failed removing saved account",
            "");

        await LoadSavedAccountsAsync();
    }
}