using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Services;
using Serilog;

namespace InstaTracker.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Navigation navigation;

    public HomeViewModel(
        ILogger logger,
        Navigation navigation)
    {
        this.logger = logger;
        this.navigation = navigation;
    }


    [RelayCommand]
    void NavigateToSearch() =>
        navigation.NavigateToTab(1);

    [RelayCommand]
    void NavigateToLogin() =>
        navigation.NavigateToTab(1);
}