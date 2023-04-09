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
    readonly DatabaseConnection database;
    readonly JsonConverter converter;
    readonly AccountManager accountManager;

    public AppHandler(
        ILogger logger,
        Config config,
        DatabaseConnection database,
        JsonConverter converter,
        AccountManager accountManager)
    {
        this.logger = logger;
        this.config = config;
        this.database = database;
        this.converter = converter;
        this.accountManager = accountManager;

        Application.Current.MainPage = new MainView();

        Initialize();

        logger.Log("App is ready");
    }


    public async void Initialize()
    {
        if (config.SaveLoginInformation && config.SavedLoginId.HasValue && await database.GetAccountAsync(config.SavedLoginId.Value) is Account savedLogin)
        {
            await accountManager.LoginAsync(savedLogin.Username, savedLogin.Password, savedLogin.StateJson);
        }
    }


    public void SaveConfig()
    {
        File.WriteAllText($"{FileSystem.AppDataDirectory}/config.json", converter.ToString(config));
        logger.Log($"Saved config to {FileSystem.AppDataDirectory}/config.json");
    }
}