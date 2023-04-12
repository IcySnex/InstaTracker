using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class SearchedAccountDatabaseConnection
{
    readonly ILogger logger;
    readonly DatabaseConnection database;

    public SearchedAccountDatabaseConnection(
        ILogger logger,
        DatabaseConnection database)
    {
        this.logger = logger;
        this.database = database;
    }


    public async Task<SearchedAccount[]> GetAllAsync()
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting searched accounts from SearchedAccount database");
        return await database.Connection.Table<SearchedAccount>().ToArrayAsync();
    }


    public async Task<SearchedAccount?> GetAsync(
        int id)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting searched account from SearchedAccount database");
        return await database.Connection.Table<SearchedAccount>().FirstOrDefaultAsync(account => account.Id == id);
    }

    public async Task<SearchedAccount?> GetAsync(
        string username)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting searched accounts from SearchedAccount database");
        return await database.Connection.Table<SearchedAccount>().FirstOrDefaultAsync(account => account.Username == username);
    }


    public async Task<int> AddAsync(
        SearchedAccount searchedAccount)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Adding searched accounts to SearchedAccount database");

        if (searchedAccount.Id.HasValue)
            await database.Connection.InsertOrReplaceAsync(searchedAccount);
        else
            await database.Connection.InsertAsync(searchedAccount);

        return await database.GetLastInsertedId();
    }


    public async Task<int> RemoveAsync(
        int id)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Removing searched accounts from SearchedAccount database with id");

        return await database.Connection.Table<SearchedAccount>().DeleteAsync(account => account.Id == id);
    }

    public async Task<int> RemoveAsync(
        string username)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Removing searched accounts from SearchedAccount database with username");

        return await database.Connection.Table<SearchedAccount>().DeleteAsync(account => account.Username == username);
    }
}