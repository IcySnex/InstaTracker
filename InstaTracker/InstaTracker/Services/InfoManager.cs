using InstagramApiSharp.API;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes;
using InstaTracker.Helpers;
using Serilog;
using System.Threading.Tasks;
using InstagramApiSharp;
using InstagramApiSharp.API.Processors;
using System.Collections.Generic;
using System.Linq;

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
        instagram.ThrowIfUnauthenticated("Failed getting account info!", logger);

        logger.Log("Getting account info");
        IResult<InstaUserInfo> result = await instagram.UserProcessor.GetUserInfoByUsernameAsync(username);
        result.ThrowIfFailed("Failed getting account info!", logger);

        return result.Value;
    }

    public async Task<InstaStoryFriendshipStatus> GetFirendshipStatusAsync(
        long id)
    {
        instagram.ThrowIfUnauthenticated("Failed getting friendship status!", logger);

        logger.Log("Getting friendship status");
        IResult<InstaStoryFriendshipStatus> result = await instagram.UserProcessor.GetFriendshipStatusAsync(id);
        result.ThrowIfFailed("Failed getting friendship status!", logger);

        return result.Value;
    }


    public async Task<List<InstaUserShort>> GetFollowersAsync(
        long id,
        int max = 200,
        string query = "",
        bool mutualsFirst = false)
    {
        instagram.ThrowIfUnauthenticated("Failed getting followers!", logger);

        InstaUserShortList followers = new();

        while (followers.Count < max)
        {
            logger.Log($"Getting followers [{followers.Count}]");
            IResult<InstaUserShortList> result = await instagram.UserProcessor.GetUserFollowersByIdAsync(id, PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(followers.Count.ToString()), query, mutualsFirst);
            result.ThrowIfFailed("Failed getting followers!", logger);

            if (result.Value.Count == 0 || result.Info.ResponseType != ResponseType.OK)
                break;

            followers.AddRange(result.Value);
        }

        return new HashSet<InstaUserShort>(followers).ToList();
    }


    public async Task<List<InstaUserShort>> GetFollowingAsync(
        long id,
        int max = 200,
        string query = "")
    {
        instagram.ThrowIfUnauthenticated("Failed getting following!", logger);

        InstaUserShortList following = new();

        while (following.Count < max)
        {
            logger.Log($"Getting following [{following.Count}]");
            IResult<InstaUserShortList> result = await instagram.UserProcessor.GetUserFollowingByIdAsync(id, PaginationParameters.MaxPagesToLoad(1).StartFromMaxId(following.Count.ToString()), query);
            result.ThrowIfFailed("Failed getting following!", logger);

            if (result.Value.Count == 0 || result.Info.ResponseType != ResponseType.OK)
                break;

            following.AddRange(result.Value);
        }

        return new HashSet<InstaUserShort>(following).ToList();
    }
}