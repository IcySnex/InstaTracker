using InstaTracker.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class AccountDatabaseConnection
{
    readonly DatabaseConnection database;

    public AccountDatabaseConnection(
        DatabaseConnection database)
    {
        this.database = database;
    }


    public Task<List<Account>> GetAllAsync() =>
        database.GetAsync<Account>();


    public async Task<Account?> GetAsync(
        int id) =>
        (await database.GetAsync<Account>(account => account.Id == id)).FirstOrDefault();

    public async Task<Account?> GetAsync(
        string username) =>
        (await database.GetAsync<Account>(account => account.Username == username)).FirstOrDefault();


    public  Task<int> AddAsync(
        Account account) =>
        database.AddAsync(account, account.Id.HasValue);


    public Task RemoveAsync(
        int id) =>
        database.RemoveAsync<Account>(account => account.Id == id);

    public Task RemoveAsync(
        string username) =>
        database.RemoveAsync<Account>(account => account.Username == username);
}