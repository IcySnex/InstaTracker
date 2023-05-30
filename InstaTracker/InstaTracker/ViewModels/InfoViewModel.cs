﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Components;
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
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

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
        if (await database.CountWhereAsync<Info>("Username", username) < 1)
        {
            await CreateCurrentInfoAsync(username);
        }

        await LoadInfosAsync(username);
        SetSelectedInfo(Infos.ElementAt(0));
    }


    [ObservableProperty]
    Info selectedInfo = default!;

    [RelayCommand]
    void SetSelectedInfo(
        Info info) =>
        SelectedInfo = info;


    [ObservableProperty]
    IOrderedEnumerable<Info> infos = default!;

    public async Task LoadInfosAsync(
        string username)
    {
        await snackBar.RunAsync(
            $"Loading account statistic states...",
            database.GetAsync<Info>(info => info.Username == username),
            snackBar.ErrorCallback(),
            (List<Info> infos) =>
                Infos = infos.OrderByDescending(info => info.FetchedAt));
    }


    [RelayCommand]
    async Task GoBackAsync() =>
        await navigation.GoBackAsync();


    async Task CreateCurrentInfoAsync(
        string username)
    {
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
            newInfo.Followers = await infoManager.GetFollowersAsync(newInfo.Pk, config.FetchFollowerLimit);
            newInfo.Following = await infoManager.GetFollowingAsync(newInfo.Pk, config.FetchFollowingLimit);
            newInfo.Fans = newInfo.Followers.Except(newInfo.Following).ToList();

        }

        await database.AddAsync(newInfo);
    }


    [ObservableProperty]
    SelectedList selectedList = SelectedList.Followers;

    [RelayCommand]
    void ShowPressed(
        string text) =>
        SelectedList = (SelectedList)Enum.Parse(typeof(SelectedList), text);


    [RelayCommand]
    async Task AddNewAsync()
    {
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