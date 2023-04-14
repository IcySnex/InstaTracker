using InstagramApiSharp.API;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.Classes;
using InstaTracker.Helpers;
using Serilog;
using System.Threading.Tasks;

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


    public async Task<string> GetHdProfilePictureAsync(
        string username)
    {
        // Check if logged in
        if (!instagram.IsUserAuthenticated)
        {
            logger.Log("Failed getting hd profile picture", new("No account is currently logged in."));
            throw new("No account is currently logged in.");
        }

        // Get account
        IResult<InstaUserInfo> result = await instagram.UserProcessor.GetUserInfoByUsernameAsync(username);
        if (!result.Succeeded)
        {
            logger.Log("Failed getting hd profile picture", result.Info.Exception);
            throw result.Info.Exception;
        }

        return result.Value.HdProfilePicUrlInfo.Uri;
    }
}