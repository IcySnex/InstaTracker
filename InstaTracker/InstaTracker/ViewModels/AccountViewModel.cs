using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using Serilog;
using Xamarin.Forms;

namespace InstaTracker.ViewModels;

public partial class AccountViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly SnackBar snackBar;

    public Config Config { get; }
    
    public AccountViewModel(
        ILogger logger,
        Config config,
        SnackBar snackBar)
    {
        this.logger = logger;
        this.Config = config;
        this.snackBar = snackBar;
    }


    [ObservableProperty]
    bool isLoggedIn;

    [RelayCommand]
    async Task LoginAsync()
    {
        snackBar.Show("I hate Xamarin <3", null);
        IsLoggedIn = true;

        logger.Log("Logged into Instagram account");
    }

    [RelayCommand]
    async Task LogoutAsync()
    {
        snackBar.Show("I hate Xamarin <3", "me too");
        IsLoggedIn = false;

        logger.Log("Logged out of Instagram account");
    }
}