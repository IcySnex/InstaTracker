using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Enums;
using InstaTracker.Helpers;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class SearchManager
{
    readonly ILogger logger;
    readonly SearchedAccountDatabaseConnection database;
    readonly IInstaApi instagram;

    public SearchManager(
        ILogger logger,
        SearchedAccountDatabaseConnection database,
        IInstaApi instagram)
    {
        this.logger = logger;
        this.database = database;
        this.instagram = instagram;

        logger.Log("Registered SearchManager");
    }


    public async Task<List<InstaUser>> SearchAccountsAsync(
        string username)
    {
        // Check if logged in
        if (!instagram.IsUserAuthenticated)
        {
            logger.Log("Failed searching for account", new("No account is currently logged in."));
            throw new("No account is currently logged in.");
        }

        // Get username suggestions
        IResult<InstaDiscoverSearchResult> result = await instagram.DiscoverProcessor.SearchPeopleAsync(username, 10);
        if (!result.Succeeded)
        {
            logger.Log("Failed searching for accounts", result.Info.Exception);
            throw result.Info.Exception;
        }

        return result.Value.Users;
    }
}