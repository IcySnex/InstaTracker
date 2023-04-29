using InstagramApiSharp.API;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes;
using InstaTracker.Helpers;
using Serilog;
using System.Threading.Tasks;
using InstagramApiSharp;

namespace InstaTracker.Services;

public class InfoManager
{
    readonly ILogger logger;
    readonly IInstaApi instagram;

    public InfoManager(
        ILogger logger,
        IInstaApi instagram)
    {
        this.logger = logger;
        this.instagram = instagram;

        logger.Log("Registered InfoManager");
    }


    public async Task<InstaUserInfo> GetAccountInfoAsync(
        string username)
    {
        instagram.ThrowIfUnauthhenticated("Failed getting account info!", logger);

        logger.Log("Getting account info");
        IResult<InstaUserInfo> result = await instagram.UserProcessor.GetUserInfoByUsernameAsync(username);
        result.ThrowIfFailed("Failed getting account info!", logger);

        return result.Value;
    }

    public async Task<InstaStoryFriendshipStatus> GetFirendshipStatusAsync(
        long id)
    {
        instagram.ThrowIfUnauthhenticated("Failed getting friendship status!", logger);

        logger.Log("Getting friendship status");
        IResult<InstaStoryFriendshipStatus> result = await instagram.UserProcessor.GetFriendshipStatusAsync(id);
        result.ThrowIfFailed("Failed getting friendship status!", logger);

        return result.Value;
    }


    public async Task<InstaUserShortList> GetFollowersAsync(
        long id,
        string query = "",
        bool mutualsFirst = false)
    {
        instagram.ThrowIfUnauthhenticated("Failed getting followers!", logger);

        logger.Log("Getting followers");
        IResult<InstaUserShortList> result = await instagram.UserProcessor.GetUserFollowersByIdAsync(id, PaginationParameters.Empty, query, mutualsFirst);
        result.ThrowIfFailed("Failed getting followers!", logger);

        return result.Value;
    }


    public async Task<InstaUserShortList> GetFollowingAsync(
        long id,
        string query = "",
        bool mutualsFirst = false)
    {
        instagram.ThrowIfUnauthhenticated("Failed getting following!", logger);

        logger.Log("Getting following");
        IResult<InstaUserShortList> result = await instagram.UserProcessor.GetUserFollowingByIdAsync(id, PaginationParameters.Empty, query);
        result.ThrowIfFailed("Failed getting following!", logger);

        return result.Value;
    }
}