using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
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


    public async Task<int> CountAsync<T>()
    {
        logger.Log("Getting count for row from database");
        return await Connection.ExecuteScalarAsync<int>($"SELECT count(id) FROM {typeof(T).Name}");
    }

    public async Task<int> CountWhereAsync<T>(
        string property,
        string predicate)
    {
        logger.Log("Getting count for row from database");
        return await Connection.ExecuteScalarAsync<int>($"SELECT count(id) FROM {typeof(T).Name} WHERE {property} = ?", predicate);
    }


    public async Task<List<T>> GetAsync<T>(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default) where T : new()
    {
        await EnsureInitializedAsync();

        logger.Log($"Getting all {typeof(T).Name}s from database");
        return await Connection.GetAllWithChildrenAsync(predicate, false, cancellationToken);
    }


    public async Task<int> AddAsync(
        object item,
        bool replace = true)
    {
        await EnsureInitializedAsync();

        logger.Log("Adding item to database");

        if (replace)
            await Connection.InsertOrReplaceWithChildrenAsync(item);
        else
            await Connection.InsertWithChildrenAsync(item);

        return await GetLastInsertedId();
    }


    public async Task RemoveAsync<T>(
        Expression<Func<T, bool>>? predicate = null,
        CancellationToken cancellationToken = default) where T : new()
    {
        await EnsureInitializedAsync();

        logger.Log($"Deleting {typeof(T).Name} from database");

        List<T> toDelete = await GetAsync(predicate, cancellationToken);
        await Connection.DeleteAllAsync(toDelete);
    }
}