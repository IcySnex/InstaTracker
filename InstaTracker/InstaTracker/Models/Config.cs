using CommunityToolkit.Mvvm.ComponentModel;

namespace InstaTracker.Models;

public partial class Config : ObservableObject
{
    [ObservableProperty]
    public bool saveAccount = false;


    [ObservableProperty]
    public int? autoLoginId = null;
}