using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstagramApiSharp.Classes.Models;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly SearchedAccountDatabaseConnection database;
    readonly Message message;
    readonly SnackBar snackBar;
    readonly SearchManager searchmanager;

    public SearchViewModel(
        ILogger logger,
        SearchedAccountDatabaseConnection database,
        Message message,
        SnackBar snackBar,
        SearchManager searchmanager)
    {
        this.logger = logger;
        this.database = database;
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
                InstaUser user = await searchmanager.GetAccount(account.Username);
                SearchHistory.Add(new SearchedAccount(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext, account.Id));
            }

        }
        catch (Exception ex)
        {
            snackBar.Show("Failed loading search history!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed loading search history!", ex.Message));
        }
    }

    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task RemoveSearchedAccountFromHistoryAsync(
        int id)
    {
        try
        {
            snackBar.Show("Removing account from history...", null, awaitPreviousSnackBar: true);
            await database.RemoveAsync(id);
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
    async Task OpenAccountAsync(
        InstaUser user)
    {
        try
        {
            snackBar.Show("Opening account...", null, awaitPreviousSnackBar: true);
            await database.AddAsync(new(user.UserName, user.FullName, user.ProfilePicture, user.IsPrivate, user.FriendshipStatus.Following, user.SearchSocialContext));
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed to open account!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed to open account!", ex.Message));
        }

        await LoadSearchHistory();
    }
}