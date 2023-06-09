﻿using CommunityToolkit.Mvvm.ComponentModel;
using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System.Collections.Generic;
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
        List<Account> savedAccounts)
    {
        logger.Log("Reloading account settings");

        SavedAccounts = savedAccounts;
        AutoLogin = Config.AutoLoginId.HasValue;
        AutoLoginAccount = SavedAccounts.FirstOrDefault(account => account.Id == Config.AutoLoginId);
    }


    [ObservableProperty]
    List<Account> savedAccounts = new();


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