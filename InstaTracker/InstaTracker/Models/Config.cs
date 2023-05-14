using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace InstaTracker.Models;

public partial class Config : ObservableObject
{
    public Guid DeviceGuid { get; } = Guid.NewGuid();

    public Guid PhoneGuid { get; } = Guid.NewGuid();


    [ObservableProperty]
    public bool saveAccount = false;

    [ObservableProperty]
    public int? autoLoginId = null;

    [ObservableProperty]
    bool loginWithState = true;


    [ObservableProperty]
    int fetchFollowerLimit = 200;

    [ObservableProperty]
    int fetchFollowingLimit = 200;
}