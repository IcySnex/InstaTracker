using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class AccountDatabaseConnection
{
    readonly ILogger logger;
    readonly DatabaseConnection database;

    public AccountDatabaseConnection(
        ILogger logger,
        DatabaseConnection database)
    {
        this.logger = logger;
        this.database = database;
    }


    public async Task<Account[]> GetAllAsync()
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting accounts from Account database");
        return await database.Connection.Table<Account>().ToArrayAsync();
    }


    public async Task<Account?> GetAsync(
        int id)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting account from Account database");
        return await database.Connection.Table<Account>().FirstOrDefaultAsync(account => account.Id == id);
    }

    public async Task<Account?> GetAsync(
        string username)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Getting account from Account database");
        return await database.Connection.Table<Account>().FirstOrDefaultAsync(account => account.Username == username);
    }


    public async Task<int> AddAsync(
        Account account)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Adding account to Account database");

        if (account.Id.HasValue)
            await database.Connection.InsertOrReplaceAsync(account);
        else
            await database.Connection.InsertAsync(account);

        return await database.GetLastInsertedId();
    }


    public async Task<int> RemoveAsync(
        int id)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Removing account from Account database with id");

        return await database.Connection.Table<Account>().DeleteAsync(account => account.Id == id);
    }

    public async Task<int> RemoveAsync(
        string username)
    {
        await database.EnsureInitializedAsync();

        logger.Log("Removing account from Account database with username");

        return await database.Connection.Table<Account>().DeleteAsync(account => account.Username == username);
    }
}