using CommunityToolkit.Mvvm.ComponentModel;
using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System;
using System.Linq;

namespace InstaTracker.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    readonly ILogger logger;

    public Config Config { get; }

    public SettingsViewModel(
        ILogger logger,
        Config config)
    {
        this.logger = logger;
        this.Config = config;
    }


    public void ReloadAccountSettings(
        Account[] savedAccounts)
    {
        SavedAccounts = savedAccounts;
        AutoLogin = Config.AutoLoginId.HasValue;
        AutoLoginAccount = SavedAccounts.FirstOrDefault(account => account.Id == Config.AutoLoginId);

        logger.Log("Reloaded account settings");
    }


    [ObservableProperty]
    Account[] savedAccounts = Array.Empty<Account>();


    [ObservableProperty]
    bool autoLogin;

    partial void OnAutoLoginChanged(bool value)
    {
        if (!value)
            Config.AutoLoginId = null;
    }


    [ObservableProperty]
    Account? autoLoginAccount;

    partial void OnAutoLoginAccountChanged(Account? value)
    {
        if (value is not null)
            Config.AutoLoginId = value.Id;
    }
}