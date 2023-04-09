using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using SQLite;
using Xamarin.Essentials;

namespace InstaTracker.Services;

public class DatabaseConnection
{
    public static readonly string FileName = "account-database.db3";
    public static readonly string FullPath = $"{FileSystem.AppDataDirectory}/{FileName}";

    public static readonly SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;


    SQLiteAsyncConnection database = default!;
    readonly ILogger logger;

    public DatabaseConnection(
        ILogger logger)
    {
        this.logger = logger;

    }


    public async Task EnsureInitializedAsync()
    {
        if (database is not null)
            return;

        logger.Log("Initializing database");
        database = new SQLiteAsyncConnection(FullPath, Flags);
        await database.CreateTableAsync<Account>();
    }


    public async Task<List<Account>> GetAccountsAsync()
    {
        await EnsureInitializedAsync();

        logger.Log("Getting accounts from database");
        return await database.Table<Account>().ToListAsync();
    }


    public async Task<Account?> GetAccountAsync(
        int id)
    {
        await EnsureInitializedAsync();

        logger.Log("Getting accounts from database");
        return await database.Table<Account>().FirstOrDefaultAsync(account => account.Id == id);
    }

    public async Task<Account?> GetAccountAsync(
        string username)
    {
        await EnsureInitializedAsync();

        logger.Log("Getting accounts from database");
        return await database.Table<Account>().FirstOrDefaultAsync(account => account.Username == username);
    }


    public async Task<int> AddAccountAsync(
        Account account)
    {
        await EnsureInitializedAsync();

        logger.Log("Adding account to database");

        if (account.Id.HasValue)
            await database.InsertOrReplaceAsync(account);
        else
            await database.InsertAsync(account);

        logger.Log("Getting last added id");
        return await database.ExecuteScalarAsync<int>($"SELECT last_insert_rowid()");
    }


    public async Task<int> RemoveAccountAsync(
        int id)
    {
        await EnsureInitializedAsync();

        logger.Log("Deleting account from database with id");

        return await database.Table<Account>().DeleteAsync(account => account.Id == id);
    }

    public async Task<int> RemoveAccountAsync(
        string username)
    {
        await EnsureInitializedAsync();

        logger.Log("Deleting account from database with username");

        return await database.Table<Account>().DeleteAsync(account => account.Username == username);
    }
}