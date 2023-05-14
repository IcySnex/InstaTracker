using InstaTracker.Models;
using System.Collections.Generic;
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
        database.GetAllAsync<Account>("Account");


    public Task<Account?> GetAsync(
        int id) =>
        database.GetAsync<Account>("Account", "Id", id);

    public Task<Account?> GetAsync(
        string username) =>
        database.GetAsync<Account>("Account", "Username", username);


    public Task<int> AddAsync(
        Account account) =>
        database.AddAsync(account, account.Id.HasValue);


    public Task<int> RemoveAsync(
        int id) =>
        database.DeleteAsync("Account", "Id", id);

    public Task<int> RemoveAsync(
        string username) =>
        database.DeleteAsync("Account", "Username", username);
}