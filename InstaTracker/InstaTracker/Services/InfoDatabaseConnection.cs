using InstaTracker.Models;
using System.Collections.Generic;
using System.Linq;
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
        database.GetAsync<Info>();


    public async Task<Info?> GetAsync(
        int id) =>
        (await database.GetAsync<Info>(info => info.Id == id)).FirstOrDefault();

    public async Task<Info?> GetAsync(
        string username) =>
        (await database.GetAsync<Info>(info => info.Username == username)).FirstOrDefault();


    public Task<int> AddAsync(
        Info info) =>
        database.AddAsync(info, info.Id.HasValue);


    public Task RemoveAsync(
        int id) =>
        database.RemoveAsync<Info>(info => info.Id == id);

    public Task RemoveAsync(
        string username) =>
        database.RemoveAsync<Info>(info => info.Username == username);
}