using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using InstaTracker.Types;
using InstaTracker.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace InstaTracker.ViewModels;

public partial class InfoViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Config config;
    readonly DatabaseConnection database;
    readonly Navigation navigation;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly InfoManager infoManager;
    readonly SearchViewModel searchViewModel;

    readonly InstaUserShortComparer userComparer = new();

    public InfoViewModel(
        ILogger logger,
        Config config,
        DatabaseConnection database,
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
        logger.Log($"Initializing InfoViewModel [{username}]");

        if (await database.CountWhereAsync<Info>("Username", username) < 1)
        {
            await CreateCurrentInfoAsync(username);
        }

        await LoadInfosAsync(username);
        SetSelectedInfo(Infos.ElementAt(0));
    }



    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedListCollection))]
    [NotifyPropertyChangedFor(nameof(SelectedListEmptyMessage))]
    [NotifyPropertyChangedFor(nameof(CompareableInfos))]
    [NotifyPropertyChangedFor(nameof(ActualFollowersCount))]
    [NotifyPropertyChangedFor(nameof(ActualFollowingCount))]
    [NotifyPropertyChangedFor(nameof(ActualFansCount))]
    [NotifyPropertyChangedFor(nameof(FollowerCountCompared))]
    [NotifyPropertyChangedFor(nameof(FollowingCountCompared))]
    Info selectedInfo = default!;

    [RelayCommand]
    void SetSelectedInfo(
        Info info)
    {
        logger.Log($"Setting selected info [{info.Username}]");

        SelectedInfo = info;

        if (ComparedInfo?.FetchedAt == info.FetchedAt)
            SetComparedInfo(null);
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CompareableInfos))]
    IOrderedEnumerable<Info> infos = default!;

    public async Task LoadInfosAsync(
        string username)
    {
        logger.Log($"Loading infos [{username}]");

        await snackBar.RunAsync(
            $"Loading account statistic states...",
            database.GetAsync<Info>(info => info.Username == username),
            snackBar.ErrorCallback(),
            (List<Info> infos) =>
                Infos = infos.OrderByDescending(info => info.FetchedAt));
    }


    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedListCollection))]
    [NotifyPropertyChangedFor(nameof(ActualFollowersCount))]
    [NotifyPropertyChangedFor(nameof(ActualFollowingCount))]
    [NotifyPropertyChangedFor(nameof(ActualFansCount))]
    [NotifyPropertyChangedFor(nameof(FollowerCountCompared))]
    [NotifyPropertyChangedFor(nameof(FollowingCountCompared))]
    Info? comparedInfo = null;

    [RelayCommand]
    void SetComparedInfo(
        Info? info)
    {
        logger.Log($"Setting compared info [{SelectedInfo.Username}]");

        ComparedInfo = ComparedInfo is null || ComparedInfo.FetchedAt != info?.FetchedAt ? info ?? null : null ;
    }

    public IEnumerable<Info> CompareableInfos =>
        Infos.Where(info => info.FetchedAt != SelectedInfo.FetchedAt);

    [RelayCommand]
    void OnShowComparedInfoChanged(
        bool value)
    {
        if (value)
            SetComparedInfo(null);
    }


    public int? ActualFollowersCount =>
        ComparedInfo is null ? SelectedInfo.Followers?.Count : SelectedInfo.Followers?.Count - ComparedInfo?.Followers?.Count ?? null;

    public int? ActualFollowingCount =>
        ComparedInfo is null ? SelectedInfo.Following?.Count : SelectedInfo.Following?.Count - ComparedInfo?.Following?.Count ?? null;

    public int? ActualFansCount =>
        ComparedInfo is null ? SelectedInfo.Fans?.Count : SelectedInfo.Fans?.Count - ComparedInfo?.Fans?.Count ?? null;


    public long? FollowerCountCompared =>
        SelectedInfo.FollowersCount - ComparedInfo?.FollowersCount ?? null;

    public long? FollowingCountCompared =>
         SelectedInfo.FollowingCount - ComparedInfo?.FollowingCount ?? null;

    public IEnumerable<InstaUserShort>? FollowersCompared
    {
        get
        {
            if (ComparedInfo is null)
                return null;

            IEnumerable<InstaUserShort>? addedFollowers = SelectedInfo.Followers.Except(ComparedInfo.Followers, userComparer);
            addedFollowers.ForEach(follower => follower.IsVerified = true);

            IEnumerable<InstaUserShort>? removedFollowers = ComparedInfo.Followers.Except(SelectedInfo.Followers, userComparer);
            removedFollowers.ForEach(follower => follower.IsVerified = false);

            return addedFollowers.Concat(removedFollowers);
        }
    }

    public IEnumerable<InstaUserShort>? FollowingCompared
    {
        get
        {
            if (ComparedInfo is null)
                return null;

            IEnumerable<InstaUserShort>? addedFollowing = SelectedInfo.Following.Except(ComparedInfo.Following, userComparer);
            addedFollowing.ForEach(following => following.IsVerified = true);

            IEnumerable<InstaUserShort>? removedFollowing = ComparedInfo.Following.Except(SelectedInfo.Following, userComparer);
            removedFollowing.ForEach(following => following.IsVerified = false);

            return addedFollowing.Concat(removedFollowing);
        }
    }

    public IEnumerable<InstaUserShort>? FansCompared
    {
        get
        {
            if (ComparedInfo is null)
                return null;

            IEnumerable<InstaUserShort>? addedFans = SelectedInfo.Fans.Except(ComparedInfo.Fans, userComparer);
            addedFans.ForEach(fan => fan.IsVerified = true);

            IEnumerable<InstaUserShort>? removedFans = ComparedInfo.Fans.Except(SelectedInfo.Fans, userComparer);
            removedFans.ForEach(fan => fan.IsVerified = false);

            return addedFans.Concat(removedFans);
        }
    }


    [RelayCommand]
    async Task GoBackAsync() =>
        await navigation.GoBackAsync();


    async Task CreateCurrentInfoAsync(
        string username)
    {
        logger.Log($"Creating current info [{username}]");

        InstaUserInfo accountInfo = await infoManager.GetAccountInfoAsync(username);
        InstaStoryFriendshipStatus friendshipStatus = await infoManager.GetFirendshipStatusAsync(accountInfo.Pk);

        Info newInfo = new(accountInfo.Username,
            accountInfo.Pk,
            accountInfo.HdProfilePicUrlInfo.Uri,
            friendshipStatus.IsPrivate,
            friendshipStatus.Following,
            accountInfo.ProfileContext,
            DateTime.UtcNow,
            !friendshipStatus.IsPrivate || friendshipStatus.Following);

        newInfo.FollowersCount = accountInfo.FollowerCount;
        newInfo.FollowingCount = accountInfo.FollowingCount;

        if (newInfo.IsLoadable)
        {
            newInfo.Followers = await infoManager.GetFollowersAsync(newInfo.Pk, config.FetchFollowerLimit, config.FetchStatisitcsDelay);
            newInfo.Following = await infoManager.GetFollowingAsync(newInfo.Pk, config.FetchFollowingLimit, config.FetchStatisitcsDelay);
            newInfo.Fans = newInfo.Followers.Except(newInfo.Following, userComparer).ToList();

        }

        await database.AddAsync(newInfo);
    }


    public IEnumerable<InstaUserShort> SelectedListCollection
    {
        get
        {
            if (ComparedInfo is null)
                return SelectedList switch
                {
                    SelectedList.Followers => SelectedInfo.Followers,
                    SelectedList.Following => SelectedInfo.Following,
                    SelectedList.Fans => SelectedInfo.Fans,
                    _ => null
                } ?? Enumerable.Empty<InstaUserShort>();

            return SelectedList switch
            {
                SelectedList.Followers => FollowersCompared,
                SelectedList.Following => FollowingCompared,
                SelectedList.Fans => FansCompared,
                _ => null
            } ?? Enumerable.Empty<InstaUserShort>();
        }
    }

    public string SelectedListEmptyMessage
    {
        get
        {
            if (ComparedInfo is null)
                return SelectedList switch
                {
                    SelectedList.Followers => "It looks like this account has no followers.",
                    SelectedList.Following => "It looks like this account is not following anyone.",
                    SelectedList.Fans => "It looks like this account has no fans.",
                    _ => "Please select a list to display!"
                };

            return SelectedList switch
            {
                SelectedList.Followers => "It looks like this account hasnt lost or gained any followers.",
                SelectedList.Following => "It looks like this account didnt follow or unfollow anyone.",
                SelectedList.Fans => "It looks like this account hasnt lost or gained any fans.",
                _ => "Please select a list to display!"
            };
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SelectedListCollection))]
    [NotifyPropertyChangedFor(nameof(SelectedListEmptyMessage))]
    SelectedList selectedList = SelectedList.Followers;

    [RelayCommand]
    void ShowPressed(
        string text) =>
        SelectedList = (SelectedList)Enum.Parse(typeof(SelectedList), text);


    [RelayCommand]
    async Task AddNewAsync()
    {
        logger.Log($"Adding new info [{SelectedInfo.Username}]");

        if (!await message.ShowQuestionAsync("Are you sure?", "Creating a new statistics creats a lot of API request, you shouldnt do this too often."))
            return;

        await snackBar.RunAsync(
            $"Adding new account statistics...",
            CreateCurrentInfoAsync(SelectedInfo.Username),
            snackBar.ErrorCallback());

        await LoadInfosAsync(SelectedInfo.Username);
        SetSelectedInfo(Infos.ElementAt(0));

    }

    [RelayCommand]
    async Task RemoveAsync()
    {
        logger.Log($"Removing info [{SelectedInfo.Username}]");

        if (!await message.ShowQuestionAsync("Are you sure?", "Deleting this entry will clear follower, following and fans statistics from this date and time. You can't undo this action."))
            return;

        await snackBar.RunAsync(
            $"Deleting account statistics...",
            database.RemoveAsync<Info>(info => info.Username == SelectedInfo.Username && info.FetchedAt == SelectedInfo.FetchedAt),
            snackBar.ErrorCallback());

        await LoadInfosAsync(SelectedInfo.Username);

        if (infos.Count() >= 1)
        {
            SetSelectedInfo(Infos.ElementAt(0));
            return;
        }

        await searchViewModel.RemoveAccountFromHistoryAsync(selectedInfo.Username);
        GoBackCommand.Execute(null);
    }


    [RelayCommand]
    Task OpenProfilePictureAsync() =>
        navigation.NavigateAsync(new ProfilePictureView(SelectedInfo.ProfilePicture, GoBackCommand));


    [RelayCommand]
    Task OpenAccountUrlAsync(
        string username) =>
        Browser.OpenAsync($"https://www.instagram.com/{username}");
}