using CommunityToolkit.Mvvm.ComponentModel;
using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System;
using System.Threading.Tasks;

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
    InstaUserShort? loggedUser = null;

    public InstaUserShort GetLoggedUser()
    {
        // Check if logged in
        if (!instagram.IsUserAuthenticated)
        {
            logger.Log("Failed gettings user information", new("No user is currently logged in."));
            throw new("No user is currently logged in.");
        }

        // Get user information
        logger.Log("Getting currently logged in user");
        UserSessionData result = instagram.GetLoggedUser();
        if (result is null)
        {
            logger.Log("Failed get user information", new NullReferenceException());
            throw new NullReferenceException();
        }

        return result.LoggedInUser;
    }


    public async Task LoginAsync(
        string username,
        string password,
        bool saveAccount = false)
    {
        // Check if already logged in
        if (instagram.IsUserAuthenticated)
        {
            logger.Log("Failed logging into Instagram API", new("An user is already logged in."));
            throw new("An user is already logged in.");
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
            throw result.Info.Exception;
        }

        // Set logged user
        LoggedUser = GetLoggedUser();

        // Save login to database if saveLoginInformation is true
        if (!saveAccount)
            return;

        logger.Log("Saving login state to database");
        int accountId = await database.AddAccountAsync(new(
            LoggedUser.UserName,
            LoggedUser.FullName,
            LoggedUser.ProfilePicture,
            instagram.GetStateDataAsString(),
            await database.GetAccountAsync(username) is Account account ? account.Id : null));

        if (config.AutoLoginId.HasValue)
            config.AutoLoginId = accountId;
    }

    public async Task LoginAsync(
        string stateJson,
        bool saveAccount = false)
    {
        // Check if already logged in
        if (instagram.IsUserAuthenticated)
        {
            logger.Log("Failed logging into Instagram API", new("An user is already logged in."));
            throw new("An user is already logged in.");
        }

        // Login with state
        logger.Log("Logging into Instagram API with state data");
        instagram.LoadStateDataFromString(stateJson);

        // Update logged user
        LoggedUser = GetLoggedUser();

        // Save login to database if saveLoginInformation is true
        if (!saveAccount)
            return;

        logger.Log("Saving login state to database");
        int accountId = await database.AddAccountAsync(new(
            LoggedUser.UserName,
            LoggedUser.FullName,
            LoggedUser.ProfilePicture,
            instagram.GetStateDataAsString(),
            await database.GetAccountAsync(LoggedUser.UserName) is Account account ? account.Id : null));

        if (config.AutoLoginId.HasValue)
            config.AutoLoginId = accountId;
    }


    public async Task LogoutAsync()
    {
        // Check if logged in
        if (!instagram.IsUserAuthenticated)
        {
            logger.Log("Failed logging out of Instagram API", new("No user is currently logged in."));
            throw new("No user is currently logged in.");
        }

        // Logout
        logger.Log("Logging out of Instagram API");
        IResult<bool> result = await instagram.LogoutAsync();
        if (!result.Succeeded)
        {
            logger.Log("Failed logging out of Instagram API", result.Info.Exception);
            throw result.Info.Exception;
        }

        LoggedUser = null;
        return;
    }
}