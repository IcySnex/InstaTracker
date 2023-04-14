﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Models;
using InstaTracker.Services;
using InstaTracker.Views;
using Serilog;
using System;
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

    public SearchedAccount Account { get; set; } = default!;

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


    [RelayCommand]
    async Task GoBackAsync() =>
        await navigation.GoBackAsync();


    [RelayCommand]
    async Task RemoveAsync()
    {
        if (!await message.ShowQuestionAsync("Are you sure?", "Deleting this account will clear all follower, following and fans statistics. You can't undo this action."))
            return;

        await searchViewModel.RemoveAccountFromHistoryAsync(Account.Username);
        await GoBackAsync();
    }


    [RelayCommand]
    async Task OpenProfilePictureAsync()
    {
        try
        {
            snackBar.Show("Opening profile picture...", null, awaitPreviousSnackBar: true);
            string profilePicture = await infoManager.GetHdProfilePictureAsync(Account.Username);

            await navigation.NavigateAsync(new ProfilePictureView(profilePicture, async (s, e) => await GoBackAsync()));
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to open profile picture!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to open profile picture!", ex.Message));
        }
    }

}