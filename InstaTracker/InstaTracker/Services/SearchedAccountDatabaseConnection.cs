using InstaTracker.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstaTracker.Services;

public class SearchedAccountDatabaseConnection
{
    readonly DatabaseConnection database;

    public SearchedAccountDatabaseConnection(
        DatabaseConnection database)
    {
        this.database = database;
    }


    public Task<List<SearchedAccount>> GetAllAsync() =>
        database.GetAllAsync<SearchedAccount>("SearchedAccount");


    public Task<SearchedAccount?> GetAsync(
        int id) =>
        database.GetAsync<SearchedAccount>("SearchedAccount", "Id", id);

    public Task<SearchedAccount?> GetAsync(
        string username) =>
        database.GetAsync<SearchedAccount>("SearchedAccount", "Username", username);


    public Task<int> AddAsync(
        SearchedAccount searchedAccount) =>
        database.AddAsync(searchedAccount, searchedAccount.Id.HasValue);


    public Task<int> RemoveAsync(
        int id) =>
        database.DeleteAsync("SearchedAccount", "Id", id);

    public Task<int> RemoveAsync(
        string username) =>
        database.DeleteAsync("SearchedAccount", "Username", username);
}