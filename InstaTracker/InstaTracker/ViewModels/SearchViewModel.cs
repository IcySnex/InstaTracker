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
    readonly AccountManager accountManager;
    readonly SearchManager searchmanager;

    public SearchViewModel(
        ILogger logger,
        SearchedAccountDatabaseConnection database,
        Navigation navigation,
        Message message,
        SnackBar snackBar,
        AccountManager accountManager,
        SearchManager searchmanager)
    {
        this.logger = logger;
        this.database = database;
        this.navigation = navigation;
        this.message = message;
        this.snackBar = snackBar;
        this.accountManager = accountManager;
        this.searchmanager = searchmanager;
    }

    public async Task InitializeAsync()
    {
        IsRefreshing = true;
        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(true),
            snackBar.ErrorCallback(),
            refreshed =>
            {
                if (!refreshed)
                    snackBar.ErrorCallback("Could not load account information from Instagram, instead search history from local database was restored.").Invoke(new("Failed loading search history!", new("No account is currently logged in.")));
            });
        IsRefreshing = false;

        logger.Log("Initialized SearchViewModel");
    }


    public SObservableCollection<SearchedAccount> SearchHistory { get; } = new();

    public async Task<bool> LoadSearchHistory(
        bool reloadAll = false)
    {
        SearchHistory.Clear();
        if (!reloadAll || accountManager.LoggedAccount is null)
        {
            SearchHistory.AddRange(await database.GetAllAsync());
            return false;
        }

        foreach (SearchedAccount account in await database.GetAllAsync())
        {
            InstaUser user = await searchmanager.GetAccountAsync(account.Username);

            SearchedAccount newAccount = new SearchedAccount(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext, account.Id);
            SearchHistory.Add(newAccount);
            await database.AddAsync(newAccount);
        }

        return true;
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

        IsRefreshing = true;
        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(),
            snackBar.ErrorCallback());
        IsRefreshing = false;
    }


    [ObservableProperty]
    bool isRefreshing = false;

    [RelayCommand]
    async Task RefreshSearchHistoryAccountsAsync()
    {
        if (IsRefreshing)
            return;

        IsRefreshing = true;
        await snackBar.RunAsync(
            "Reloading search history...",
            LoadSearchHistory(true),
            snackBar.ErrorCallback(),
            refreshed =>
            {
                if (!refreshed)
                    snackBar.ErrorCallback("Could not reload account information from Instagram, instead search history from local database was restored.").Invoke(new("Failed reloading search history!", new("No account is currently logged in.")));
            });
        IsRefreshing = false;
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
            snackBar.ErrorCallback(),
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


    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task AddAccountAsync(
        InstaUser user)
    {
        if (!await snackBar.RunAsync(
                "Adding account...",
                database.AddAsync(new(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext)),
                snackBar.ErrorCallback()))
            return;

        IsRefreshing = true;
        await snackBar.RunAsync(
            "Loading search history...",
            LoadSearchHistory(),
            snackBar.ErrorCallback());
        IsRefreshing = false;
    }


    [RelayCommand]
    async Task LoadAccountAsync(
        string username)
    {
        InfoViewModel viewModel = App.Provider.GetRequiredService<InfoViewModel>();

        if (!await snackBar.RunAsync(
                "Loading account...",
                viewModel.InitializeAsync(username),
                snackBar.ErrorCallback()))
            return;

        await navigation.NavigateAsync(new InfoView(viewModel));
        if (!viewModel.CanLoad)
            await snackBar.DisplayAsync(
                "Failed loading followers/following & fanse!",
                "More",
                true,
                async () => await message.ShowAsync("Failed loading followers/following & fanse", "Since this account is private you, must be a follower to load account information like followers, following and fans."));
    }
}