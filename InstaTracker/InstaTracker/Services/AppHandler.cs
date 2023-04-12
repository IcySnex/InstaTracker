﻿using InstaTracker.Views;
using Xamarin.Essentials;
using Serilog;
using InstaTracker.Models;
using Xamarin.Forms;
using InstaTracker.Helpers;
using InstaTracker.ViewModels;
using System.IO;

namespace InstaTracker.Services;

public class AppHandler
{
    readonly ILogger logger;
    readonly Config config;
    readonly JsonConverter converter;
    readonly SearchViewModel searchViewModel;
    readonly AccountViewModel accountViewModel;

    public AppHandler(
        ILogger logger,
        Config config,
        JsonConverter converter,
        SearchViewModel searchViewModel,
        AccountViewModel accountViewModel)
    {
        this.logger = logger;
        this.config = config;
        this.converter = converter;
        this.searchViewModel = searchViewModel;
        this.accountViewModel = accountViewModel;

        Application.Current.MainPage = new MainView();

        Initialize();

        logger.Log("App is ready");
    }


    public async void Initialize()
    {
        await searchViewModel.InitializeAsync();
        await accountViewModel.InitializeAsync();
    }


    public void SaveConfig()
    {
        File.WriteAllText($"{FileSystem.AppDataDirectory}/config.json", converter.ToString(config));
        logger.Log($"Saved config to {FileSystem.AppDataDirectory}/config.json");
    }
}