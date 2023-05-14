using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace InstaTracker.Services;

public class DatabaseConnection
{
    public static readonly string FileName = "database.db3";
    public static readonly string FullPath = $"{FileSystem.AppDataDirectory}/{FileName}";

    public static readonly SQLiteOpenFlags Flags =
        SQLiteOpenFlags.ReadWrite |
        SQLiteOpenFlags.Create |
        SQLiteOpenFlags.SharedCache;


    readonly ILogger logger;

    public SQLiteAsyncConnection Connection { get; private set; } = default!;

    public DatabaseConnection(
        ILogger logger)
    {
        this.logger = logger;
    }


    public async Task EnsureInitializedAsync()
    {
        if (Connection is not null)
            return;

        logger.Log("Initializing database");
        Connection = new(FullPath, Flags);

        await Connection.CreateTableAsync<Account>();
        await Connection.CreateTableAsync<SearchedAccount>();
        await Connection.CreateTableAsync<Info>();
    }


    public async Task<int> GetLastInsertedId()
    {
        logger.Log("Getting last inserted id from database");
        return await Connection.ExecuteScalarAsync<int>($"SELECT last_insert_rowid()");
    }


    public async Task<List<T>> GetAllAsync<T>(
        string table) where T : new()
    {
        await EnsureInitializedAsync();

        logger.Log($"Getting all {table}s from database");
        return await Connection.QueryAsync<T>($"SELECT * FROM {table}");
    }

    public async Task<T?> GetAsync<T>(
        string table,
        string property,
        object predicate) where T : new()
    {
        await EnsureInitializedAsync();

        logger.Log($"Getting {table} from database");
        return (await Connection.QueryAsync<T>($"SELECT * FROM {table} WHERE {property} = ?", predicate)).FirstOrDefault();
    }

    public async Task<int> AddAsync(
        object item,
        bool replace = true)
    {
        await EnsureInitializedAsync();

        logger.Log("Adding item to database");

        if (replace)
            await Connection.InsertOrReplaceAsync(item);
        else
            await Connection.InsertAsync(item);

        return await GetLastInsertedId();
    }

    public async Task<int> DeleteAsync(
        string table,
        string property,
        object predicate)
    {
        await EnsureInitializedAsync();

        logger.Log($"Deleting {table} from database");
        return await Connection.ExecuteAsync($"DELETE FROM {table} WHERE {property} = ?", predicate);
    }
}