using InstagramApiSharp.API;
using InstagramApiSharp.Classes;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using Serilog;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class SearchManager
{
    readonly ILogger logger;
    readonly IInstaApi instagram;

    public SearchManager(
        ILogger logger,
        IInstaApi instagram)
    {
        this.logger = logger;
        this.instagram = instagram;

        logger.Log("Registered SearchManager");
    }


    public async Task<List<InstaUser>> SearchAccountsAsync(
        string username)
    {
        instagram.ThrowIfUnauthhenticated("Failed searching for accounts!", logger);

        logger.Log("Searching for accounts");
        IResult<InstaDiscoverSearchResult> result = await instagram.DiscoverProcessor.SearchPeopleAsync(username, 10);
        result.ThrowIfFailed("Failed searching for accounts!", logger);

        return result.Value.Users;
    }


    public async Task<InstaUser> GetAccountAsync(
        string username)
    {
        instagram.ThrowIfUnauthhenticated("Failed getting account!", logger);

        logger.Log("Getting account");
        IResult<InstaUser> result = await instagram.UserProcessor.GetUserAsync(username);
        result.ThrowIfFailed("Failed getting account!", logger);

        return result.Value;
    }
}