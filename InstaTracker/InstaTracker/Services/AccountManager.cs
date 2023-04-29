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
    readonly AccountDatabaseConnection database;
    readonly IInstaApi instagram;

    public AccountManager(
        ILogger logger,
        Config config,
        AccountDatabaseConnection database,
        IInstaApi instagram)
    {
        this.logger = logger;
        this.config = config;
        this.database = database;
        this.instagram = instagram;

        logger.Log("Registered AccountManager");
    }


    [ObservableProperty]
    InstaUserShort? loggedAccount = null;

    public InstaUserShort GetLoggedAccount()
    {
        instagram.ThrowIfUnauthhenticated("Failed getting logged account!", logger);

        logger.Log("Getting logged account");
        UserSessionData result = instagram.GetLoggedUser();
        if (result is null)
        {
            logger.Log("Failed getting logged account!", new NullReferenceException());
            throw new NullReferenceException();
        }

        return result.LoggedInUser;
    }


    public async Task LoginAsync(
        string username,
        string password,
        bool saveAccount = false)
    {
        instagram.ThrowIfAuthhenticated("Failed logging in with username!", logger);

        // Login with username
        logger.Log("Logging in with username");
        instagram.SetUser(UserSessionData
            .ForUsername(username)
            .WithPassword(password));

        IResult<InstaLoginResult> result = await instagram.LoginAsync();
        result.ThrowIfFailed("Failed logging in with username!", logger);

        // Set logged user
        LoggedAccount = GetLoggedAccount();

        // Save login to database if saveLoginInformation is true
        if (!saveAccount)
            return;

        logger.Log("Saving login state to database");
        Account? account = await database.GetAsync(LoggedAccount.UserName);
        int accountId = await database.AddAsync(new(
            LoggedAccount.UserName,
            password,
            LoggedAccount.FullName,
            LoggedAccount.ProfilePicture,
            instagram.GetStateDataAsString(),
            account is null ? null: account.Id));

        if (config.AutoLoginId.HasValue)
            config.AutoLoginId = accountId;
    }

    public async Task LoginAsync(
        string stateJson,
        bool saveAccount = false)
    {
        instagram.ThrowIfAuthhenticated("Failed logging in with state!", logger);

        // Login with state
        logger.Log("Logging in with state!");
        instagram.LoadStateDataFromString(stateJson);

        // Update logged user
        LoggedAccount = GetLoggedAccount();

        // Save login to database if saveLoginInformation is true
        if (!saveAccount)
            return;

        logger.Log("Saving login state to database");
        Account? account = await database.GetAsync(LoggedAccount.UserName);
        int accountId = await database.AddAsync(new(
            LoggedAccount.UserName,
            account is null ? null : account.Password,
            LoggedAccount.FullName,
            LoggedAccount.ProfilePicture,
            instagram.GetStateDataAsString(),
            account is null ? null : account.Id));

        if (config.AutoLoginId.HasValue)
            config.AutoLoginId = accountId;
    }


    public async Task LogoutAsync()
    {
        // Check if logged in
        instagram.ThrowIfUnauthhenticated("Failed logging out!", logger);

        // Logout
        logger.Log("Logging out");
        IResult<bool> result = await instagram.LogoutAsync();
        result.ThrowIfFailed("Failed logging out!", logger);

        LoggedAccount = null;
        return;
    }
}