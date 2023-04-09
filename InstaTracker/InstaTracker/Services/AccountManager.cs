using CommunityToolkit.Mvvm.ComponentModel;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;

namespace InstaTracker.Services;

public partial class AccountManager : ObservableObject
{
    readonly ILogger logger;
    readonly Config config;
    readonly DatabaseConnection database;
    readonly IInstaApi instagram;

    public AccountManager(
        ILogger logger,
        Config config,
        DatabaseConnection database,
        IInstaApi instagram)
    {
        this.logger = logger;
        this.config = config;
        this.database = database;
        this.instagram = instagram;

        logger.Log("Registered AccountManager");
    }


    [ObservableProperty]
    bool isLoggedIn;


    public async Task<InstaCurrentUser> GetCurrentUserAsync()
    {
        logger.Log("Getting currently logged in user");
        IResult<InstaCurrentUser> result = await instagram.GetCurrentUserAsync();

        if (!result.Succeeded)
            throw result.Info.Exception;

        return result.Value;
    }


    public async Task<bool> LoginAsync(
        string username,
        string password,
        string? stateJson = null)
    {
        // Check if already logged in
        if (instagram.IsUserAuthenticated)
        {
            logger.Log("Failed logging into Instagram API", new("An user is already logged in."));
            return false;
        }

        // Login with state data if exists
        if (stateJson is not null)
        {
            logger.Log("Logging into Instagram API with state data");
            instagram.LoadStateDataFromString(stateJson);

            IsLoggedIn = true;
            return true;
        }

        // Login with username and password
        logger.Log("Logging into Instagram API with username and password");
        instagram.SetUser(UserSessionData
            .ForUsername(username)
            .WithPassword(password));

        IResult<InstaLoginResult> result = await instagram.LoginAsync();
        if (!result.Succeeded)
        {
            logger.Log("Failed logging into Instagram API", result.Info.Exception);
            return false;
        }

        // Save login to database if saveLoginInformation is true
        if (config.SaveLoginInformation)
        {
            logger.Log("Saving login state to database");
            string newStateJson = instagram.GetStateDataAsString();
            int id = await database.AddAccountAsync(new Account(username, password, newStateJson));

            config.SavedLoginId = id;
        }

        IsLoggedIn = true;
        return true;
    }


    public async Task<bool> LogoutAsync()
    {
        // Check if logged in
        if (!instagram.IsUserAuthenticated)
        {
            logger.Log("Failed logging out of Instagram API", new("No user is logged in."));
            return false;
        }

        // Logout
        logger.Log("Logging out of Instagram API");
        IResult<bool> result = await instagram.LogoutAsync();
        if (!result.Succeeded)
        {
            logger.Log("Failed logging out of Instagram API", result.Info.Exception);
            return false;
        }

        IsLoggedIn = false;
        return result.Value;
    }
}