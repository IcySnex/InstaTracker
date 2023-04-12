using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Helpers;
using InstaTracker.Views;
using Serilog;
using Xamarin.Forms;

namespace InstaTracker.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;

    public HomeViewModel(
        ILogger logger)
    {
        this.logger = logger;
    }


    [RelayCommand]
    void NavigateToSearch()
    {
        MainView mainView = (MainView)Application.Current.MainPage;
        mainView.CurrentPage = mainView.Children[1];

        logger.Log("Navigated to search page");
    }

    [RelayCommand]
    void NavigateToLogin()
    {
        MainView mainView = (MainView)Application.Current.MainPage;
        mainView.CurrentPage = mainView.Children[2];

        logger.Log("Navigated to login page");
    }
}