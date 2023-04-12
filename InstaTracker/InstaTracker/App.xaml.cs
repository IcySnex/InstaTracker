﻿using InstaTracker.Helpers;
using InstaTracker.Services;
using InstaTracker.Models;
using Serilog;
using Xamarin.Essentials;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using InstaTracker.ViewModels;
using InstagramApiSharp.API.Builder;
using System;
using System.IO;

namespace InstaTracker;

public partial class App : Application
{
    AppHandler AppHandler { get; set; } = default!;

    public static IServiceProvider Provider { get; private set; } = default!;
    public static InMemorySink Sink { get; private set; } = new();


    public App()
    {
        ILogger logger = new LoggerConfiguration()
            .WriteTo.Debug()
            .WriteTo.Sink(Sink)
            .WriteTo.File($"{FileSystem.CacheDirectory}/logs/log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 10)
            .CreateLogger();

        logger.Log("Creating app configuration");
        Config configuration = File.Exists($"{FileSystem.AppDataDirectory}/config.json") ? JsonSerializer.Deserialize<Config>(File.ReadAllText($"{FileSystem.AppDataDirectory}/config.json")) ?? new() : new();

        logger.Log("Initializing ServiceProvider");
        Provider = new ServiceCollection()
            .AddSingleton(logger)
            .AddSingleton(configuration)
            .AddSingleton<DatabaseConnection>()
            .AddSingleton<AccountDatabaseConnection>()
            .AddSingleton<SearchedAccountDatabaseConnection>()
            .AddSingleton<AppHandler>()
            .AddSingleton<JsonConverter>()
            .AddSingleton<Message>()
            .AddSingleton<SnackBar>()
            .AddSingleton(InstaApiBuilder.CreateBuilder().Build())
            .AddSingleton<AccountManager>()
            .AddSingleton<HomeViewModel>()
            .AddSingleton<SearchViewModel>()
            .AddSingleton<AccountViewModel>()
            .AddSingleton<SettingsViewModel>()
            .BuildServiceProvider();

        InitializeComponent();
    }


    protected override void OnStart() =>
        AppHandler = Provider.GetRequiredService<AppHandler>();

    protected override void OnSleep() =>
        AppHandler.SaveConfig();
}