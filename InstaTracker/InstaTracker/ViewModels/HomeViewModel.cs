using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Services;

namespace InstaTracker.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly Navigation navigation;

    public HomeViewModel(
        Navigation navigation)
    {
        this.navigation = navigation;
    }


    [RelayCommand]
    void NavigateToSearch() =>
        navigation.NavigateToTab(1);

    [RelayCommand]
    void NavigateToLogin() =>
        navigation.NavigateToTab(1);
}