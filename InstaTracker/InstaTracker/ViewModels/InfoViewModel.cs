using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using InstaTracker.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class InfoViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Config config;
    readonly InfoDatabaseConnection database;
    readonly Navigation navigation;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly InfoManager infoManager;
    readonly SearchViewModel searchViewModel;

    public InfoViewModel(
        ILogger logger,
        Config config,
        InfoDatabaseConnection database,
        Navigation navigation,
        Message message,
        SnackBar snackBar,
        InfoManager infoManager,
        SearchViewModel searchViewModel)
    {
        this.logger = logger;
        this.config = config;
        this.database = database;
        this.navigation = navigation;
        this.message = message;
        this.snackBar = snackBar;
        this.infoManager = infoManager;
        this.searchViewModel = searchViewModel;
    }

    public async Task InitializeAsync(
        string username)
    {
        // Load account info
        AccountInfo = await infoManager.GetAccountInfoAsync(username);
        FriendshipStatus = await infoManager.GetFirendshipStatusAsync(AccountInfo.Pk);

        profilePicture = AccountInfo.HdProfilePicUrlInfo.Uri;

        Info = new()
        {
            Username = AccountInfo.Username,
            FetchedAt = DateTime.UtcNow
        };

        Info.FollowersCount = AccountInfo.FollowerCount;
        Info.FollowingCount = AccountInfo.FollowingCount;

        // Load followers/following/fans list
        if (AccountInfo.IsPrivate && !FriendshipStatus.Following)
        {
            CanLoad = false;
            return;
        }

        Info.Followers = await infoManager.GetFollowersAsync(AccountInfo.Pk, config.FetchFollowerLimit);
        Info.Following = await infoManager.GetFollowingAsync(AccountInfo.Pk, config.FetchFollowingLimit);
        Info.Fans = Info.Followers.Except(Info.Following).ToList();
        Info.FansCount = Info.Fans.Count();

        await database.AddAsync(Info);
    }

    /// <summary>
    /// //////
    /// </summary>
    public InstaUserInfo AccountInfo { get; set; } = default!;
    /// <summary>
    /// ///////////
    /// </summary>
    public InstaStoryFriendshipStatus FriendshipStatus { get; set; } = default!;

    [ObservableProperty]
    bool canLoad = true;

    [ObservableProperty]
    Info info = default!;


    [RelayCommand]
    async Task GoBackAsync() =>
        await navigation.GoBackAsync();


    [RelayCommand]
    async Task RemoveAsync()
    {
        if (!await message.ShowQuestionAsync("Are you sure?", "Deleting this entry will clear all follower, following and fans statistics from this date and time. You can't undo this action."))
            return;

        await searchViewModel.RemoveAccountFromHistoryAsync(AccountInfo.Username);
        await GoBackAsync();
    }


    string profilePicture = default!;

    [RelayCommand]
    async Task OpenProfilePictureAsync()
    {
        await snackBar.RunAsync(
            "Opening profile picture...",
            navigation.NavigateAsync(new ProfilePictureView(profilePicture, GoBackCommand)),
            snackBar.ErrorCallback());
    }
}