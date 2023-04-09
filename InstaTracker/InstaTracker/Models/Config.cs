using CommunityToolkit.Mvvm.ComponentModel;

namespace InstaTracker.Models;

public partial class Config : ObservableObject
{
    [ObservableProperty]
    bool saveLoginInformation = false;

    public int? SavedLoginId { get; set; } = null;
}