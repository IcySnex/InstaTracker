using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using InstaTracker.Views;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class InfoViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Navigation navigation;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly InfoManager infoManager;
    readonly SearchViewModel searchViewModel;

    public InfoViewModel(
        ILogger logger,
        Navigation navigation,
        Message message,
        SnackBar snackBar,
        InfoManager infoManager,
        SearchViewModel searchViewModel)
    {
        this.logger = logger;
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
        FollowersCount = AccountInfo.FollowerCount;
        FollowingCount = AccountInfo.FollowingCount;
        FansCount = FollowersCount - FollowingCount < 0 ? 0 : FollowersCount - FollowingCount;

        // Load followers/following/fans list
        if (AccountInfo.IsPrivate && !FriendshipStatus.Following)
        {
            CanLoad = false;
            ShowFailedLoading();
            return;
        }

        Followers = await infoManager.GetFollowersAsync(AccountInfo.Pk);
        Following = await infoManager.GetFollowingAsync(AccountInfo.Pk);
        Fans = Followers.Except(Following);
    }

    async void ShowFailedLoading()
    {
        await Task.Delay(1000);

        await snackBar.DisplayAsync(
            "Failed loading followers/following & fanse!",
            "More",
            true,
            async () => await message.ShowAsync("Failed loading account info", "Since this account is private you, must be a follower to load account information like followers, following and fans."));
    }

    public InstaUserInfo AccountInfo { get; private set; } = default!;

    public InstaStoryFriendshipStatus FriendshipStatus { get; private set; } = default!;

    [ObservableProperty]
    bool canLoad = true;


    [ObservableProperty]
    long? followersCount = null;

    [ObservableProperty]
    long? followingCount = null;

    [ObservableProperty]
    long? fansCount = null;


    [ObservableProperty]
    List<InstaUserShort>? followers = null;

    [ObservableProperty]
    List<InstaUserShort>? following = null;

    [ObservableProperty]
    IEnumerable<InstaUserShort>? fans = null;


    [RelayCommand]
    async Task GoBackAsync() =>
        await navigation.GoBackAsync();


    [RelayCommand]
    async Task RemoveAsync()
    {
        if (!await message.ShowQuestionAsync("Are you sure?", "Deleting this account will clear all follower, following and fans statistics. You can't undo this action."))
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