using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using SQLite;
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
    }


    public async Task<int> GetLastInsertedId()
    {
        logger.Log("Getting last inserted id from database");
        return await Connection.ExecuteScalarAsync<int>($"SELECT last_insert_rowid()");
    }
}