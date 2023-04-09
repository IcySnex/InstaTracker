using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using Serilog;

namespace InstaTracker.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly DatabaseConnection database;
    readonly SnackBar snackBar;

    public Config Config { get; }
    public AccountManager AccountManager { get; }

    public AccountViewModel(
        ILogger logger,
        Config config,
        DatabaseConnection database,
        SnackBar snackBar,
        AccountManager accountManager)
    {
        this.logger = logger;
        this.Config = config;
        this.database = database;
        this.snackBar = snackBar;
        this.AccountManager = accountManager;
    }


    [ObservableProperty]
    string username = default!;

    [ObservableProperty]
    string password = default!;


    [RelayCommand]
    async Task LoginAsync()
    {
        snackBar.Show("Logging in...", null);
        await AccountManager.LoginAsync(Username, Password);
    }


    [RelayCommand]
    async Task LogoutAsync()
    {
        snackBar.Show("Logging out...", null);
        await AccountManager.LogoutAsync();
    }
}