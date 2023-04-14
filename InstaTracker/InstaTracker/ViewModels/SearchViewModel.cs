using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using InstaTracker.Views;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly SearchedAccountDatabaseConnection database;
    readonly Navigation navigation;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly SearchManager searchmanager;

    public SearchViewModel(
        ILogger logger,
        SearchedAccountDatabaseConnection database,
        Navigation navigation,
        Message message,
        SnackBar snackBar,
        SearchManager searchmanager)
    {
        this.logger = logger;
        this.database = database;
        this.navigation = navigation;
        this.message = message;
        this.snackBar = snackBar;
        this.searchmanager = searchmanager;
    }

    public async Task InitializeAsync()
    {
        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(true),
            snackBar.ErrorCallback("Failed loading search history!"));

        logger.Log("Initialized SearchViewModel");
    }


    public SObservableCollection<SearchedAccount> SearchHistory { get; } = new();

    async Task LoadSearchHistory(
        bool reloadAll = false)
    {
        SearchHistory.Clear();
        if (!reloadAll)
        {
            SearchHistory.AddRange(await database.GetAllAsync());
            return;
        }

        foreach (SearchedAccount account in await database.GetAllAsync())
        {
            InstaUser user = await searchmanager.GetAccountAsync(account.Username);
            SearchHistory.Add(new SearchedAccount(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext, account.Id));
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task RemoveAccountFromHistoryWarningAsync(
        string username)
    {
        if (!await message.ShowQuestionAsync("Are you sure?", "Deleting this account will clear all follower, following and fans statistics. You can't undo this action."))
            return;

        await RemoveAccountFromHistoryAsync(username);
    }

    public async Task RemoveAccountFromHistoryAsync(
        string username)
    {
        await database.RemoveAsync(username);

        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(),
            snackBar.ErrorCallback("Failed loading search history!"));
    }


    [ObservableProperty]
    List<InstaUser>? searchResults = null;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    string searchUsername = default!;

    [ObservableProperty]
    bool showSearchResults = false;

    bool CanSearchCommandExecute =>
        !string.IsNullOrWhiteSpace(SearchUsername);

    [RelayCommand(CanExecute = nameof(CanSearchCommandExecute))]
    async Task SearchAsync()
    {
        await snackBar.RunAsync(
            "Searching for users...",
            searchmanager.SearchAccountsAsync(SearchUsername),
            snackBar.ErrorCallback("Failed searching for users!"),
            (List<InstaUser> users) =>
            {
                SearchResults = users;
                ShowSearchResults = true;
            });
    }

    [RelayCommand]
    void ClearSearch()
    {
        SearchResults = null;
        ShowSearchResults = false;
    }


    [RelayCommand]
    async Task AddAccountAsync(
        InstaUser user)
    {
        await snackBar.RunAsync(
            "Adding account...",
            database.AddAsync(new(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext)),
            snackBar.ErrorCallback("Failed adding account!"));

        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(),
            snackBar.ErrorCallback("Failed loading search history!"));
    }


    [RelayCommand]
    async Task OpenAccountAsync(
        SearchedAccount account)
    {
        InfoViewModel viewModel = App.Provider.GetRequiredService<InfoViewModel>();
        viewModel.Account = account;

        await navigation.NavigateAsync(new InfoPage(viewModel));
    }
}