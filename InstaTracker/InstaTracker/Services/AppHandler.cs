using InstaTracker.Views;
using Xamarin.Essentials;
using Serilog;
using InstaTracker.Models;
using Xamarin.Forms;
using InstaTracker.Helpers;
using InstaTracker.ViewModels;
using System.IO;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes.Android.DeviceInfo;
using System;
using SQLiteNetExtensions.Extensions.TextBlob;

namespace InstaTracker.Services;

public class AppHandler
{
    readonly ILogger logger;
    readonly Config config;
    readonly JsonConverter converter;
    readonly IInstaApi instagram;
    readonly SearchViewModel searchViewModel;
    readonly AccountViewModel accountViewModel;

    public AppHandler(
        ILogger logger,
        Config config,
        JsonConverter converter,
        IInstaApi instagram,
        SearchViewModel searchViewModel,
        AccountViewModel accountViewModel)
    {
        this.logger = logger;
        this.config = config;
        this.converter = converter;
        this.instagram = instagram;
        this.searchViewModel = searchViewModel;
        this.accountViewModel = accountViewModel;

        Application.Current.MainPage = new MainView();

        Initialize();

        logger.Log("App is ready");
    }


    public async void Initialize()
    {
        AndroidDevice device = new()
        {
            AndroidBoardName = "Exynos 2200",
            AndroidBootloader = "S901BXXU4CWCG",
            DeviceBrand = "samsung",
            DeviceModel = "SM-S901B",
            DeviceModelBoot = "exynos",
            DeviceModelIdentifier = "Galaxy S22",
            FirmwareBrand = "rainbow",
            FirmwareFingerprint = "samsung/rainbow/rainbow:13/26203421/S901BXXU4CWCG:user/release-keys",
            FirmwareTags = "release-keys",
            FirmwareType = "user",
            HardwareManufacturer = "samsung",
            HardwareModel = "SM-S901B",
            DeviceGuid = config.DeviceGuid,
            PhoneGuid = config.PhoneGuid,
            Resolution = "1080x2340",
            Dpi = "425dpi"
        };
        instagram.SetDevice(device);

        TextBlobOperations.SetTextSerializer();

        //await accountViewModel.InitializeAsync();
        //await searchViewModel.InitializeAsync();
    }


    public void SaveConfig()
    {
        File.WriteAllText($"{FileSystem.AppDataDirectory}/config.json", converter.ToString(config));
        logger.Log($"Saved config to {FileSystem.AppDataDirectory}/config.json");
    }
}