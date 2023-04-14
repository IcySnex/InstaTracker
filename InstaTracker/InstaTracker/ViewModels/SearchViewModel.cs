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
        await LoadSearchHistory(true);

        logger.Log("Initialized SearchViewModel");
    }


    public SObservableCollection<SearchedAccount> SearchHistory { get; } = new();

    async Task LoadSearchHistory(
        bool reloadAll = false)
    {
        try
        {
            snackBar.Show("Loading search history...", null, awaitPreviousSnackBar: true);

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
        catch (Exception ex)
        {
            snackBar.Show("Failed loading search history!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed loading search history!", ex.Message));
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
        try
        {
            await database.RemoveAsync(username);
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed removing account from history", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed removing account from history", ex.Message));
        }

        await LoadSearchHistory();
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
        try
        {
            snackBar.Show("Searching for username...", null, awaitPreviousSnackBar: true);
            SearchResults = await searchmanager.SearchAccountsAsync(SearchUsername);
            ShowSearchResults = true;
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to search for username!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to search for username!", ex.Message));
        }

    }

    [RelayCommand]
    void ClearSearch()
    {
        try
        {
            snackBar.Show("Clearing search results...", null, awaitPreviousSnackBar: true);
            SearchResults = null;
            ShowSearchResults = false;
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to clear search results!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to clear search results!", ex.Message));
        }
    }


    [RelayCommand]
    async Task AddAccountAsync(
        InstaUser user)
    {
        try
        {
            snackBar.Show("Adding account...", null, awaitPreviousSnackBar: true);
            await database.AddAsync(new(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext));
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to add account!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to add account!", ex.Message));
        }

        await LoadSearchHistory();
    }


    [RelayCommand]
    async Task OpenAccountAsync(
        SearchedAccount account)
    {
        try
        {
            snackBar.Show("Opening account...", null, awaitPreviousSnackBar: true);

            InfoViewModel viewModel = App.Provider.GetRequiredService<InfoViewModel>();
            viewModel.Account = account;

            await navigation.NavigateAsync(new InfoPage(viewModel));
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to open account!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to open account!", ex.Message));
        }
    }
}