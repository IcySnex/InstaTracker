using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Helpers;
using InstaTracker.Models;
using InstaTracker.Services;
using Serilog;
using System;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class SearchViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly SearchedAccountDatabaseConnection database;
    readonly Message message;
    readonly SnackBar snackBar;

    public SearchViewModel(
        ILogger logger,
        SearchedAccountDatabaseConnection database,
        Message message,
        SnackBar snackBar)
    {
        this.logger = logger;
        this.database = database;
        this.message = message;
        this.snackBar = snackBar;
    }

    public async Task InitializeAsync()
    {
        await LoadSearchHistory();

        logger.Log("Initialized SearchViewModel");
    }


    async Task ExecuteNotifiedAsync(
        Task task,
        string startingMessage,
        string failedTitle,
        string failedMessage)
    {
        try
        {
            snackBar.Show(startingMessage, null, awaitPreviousSnackBar: true);
            await task;
        }
        catch (Exception ex)
        {
            snackBar.Show(failedTitle, "More", 10000, onButtonClicked: async () => await message.ShowAsync(failedTitle, $"{failedMessage}({ex.Message})"));
        }
    }


    [ObservableProperty]
    SearchedAccount[] searchHistory = Array.Empty<SearchedAccount>();

    async Task LoadSearchHistory()
    {
        try
        {
            snackBar.Show("Loading search history...", null, awaitPreviousSnackBar: true);
            SearchHistory = await database.GetAllAsync();
        }
        catch (Exception ex)
        {
            snackBar.Show("Failed loading search history!", "More", 10000, onButtonClicked: async () => await message.ShowAsync("Failed loading search history!", $"({ex.Message})"));
        }
    }


    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
    string searchUsername = default!;

    [ObservableProperty]
    bool hasSearchResults = false;

    bool CanSearchCommandExecute =>
        !string.IsNullOrWhiteSpace(SearchUsername);

    [RelayCommand(CanExecute = nameof(CanSearchCommandExecute))]
    async Task SearchAsync()
    {
        await database.AddAsync(new(SearchUsername, "Full Name", null));

        await LoadSearchHistory();
    }


    [RelayCommand(AllowConcurrentExecutions = true)]
    async Task RemoveSearchedAccountAsync(
        int id)
    {
        await ExecuteNotifiedAsync(
            database.RemoveAsync(id),
            "Removing account from history...",
            "Failed removing account from history",
            "");

        await LoadSearchHistory();
    }
}