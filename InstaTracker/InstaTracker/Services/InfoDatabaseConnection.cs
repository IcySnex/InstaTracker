using InstaTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class InfoDatabaseConnection
{
    readonly DatabaseConnection database;

    public InfoDatabaseConnection(
        DatabaseConnection database)
    {
        this.database = database;
    }


    public Task<List<Info>> GetAllAsync() =>
        database.GetAllAsync<Info>("Info");


    public Task<Info?> GetAsync(
        int id) =>
        database.GetAsync<Info>("Info", "Id", id);

    public Task<Info?> GetAsync(
        string username) =>
        database.GetAsync<Info>("Info", "Username", username);


    public Task<int> AddAsync(
        Info info) =>
        database.AddAsync(info, info.Id.HasValue);


    public Task<int> RemoveAsync(
        int id) =>
        database.DeleteAsync("Info", "Id", id);

    public Task<int> RemoveAsync(
        string username) =>
        database.DeleteAsync("Info", "Username", username);
}