using InstaTracker.Views;
using Xamarin.Essentials;
using Serilog;
using InstaTracker.Models;
using Xamarin.Forms;
using InstaTracker.Helpers;

namespace InstaTracker.Services;

public class AppHandler
{
    readonly ILogger logger;
    readonly Config config;
    readonly JsonConverter converter;

    public AppHandler(
        ILogger logger,
        Config config,
        JsonConverter converter)
    {
        this.logger = logger;
        this.config = config;
        this.converter = converter;

        Application.Current.MainPage = new MainView();

        logger.Log("App is ready");
    }


    public void SaveConfig()
    {
        File.WriteAllText($"{FileSystem.AppDataDirectory}/config.json", converter.ToString(config));
        logger.Log($"Saved config to {FileSystem.AppDataDirectory}/config.json");
    }
}