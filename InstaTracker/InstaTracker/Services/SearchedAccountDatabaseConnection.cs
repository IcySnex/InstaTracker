using InstaTracker.Models;
using System.Collections.Generic;
using System.Linq;
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
        database.GetAsync<SearchedAccount>();


    public async Task<SearchedAccount?> GetAsync(
        int id) =>
        (await database.GetAsync<SearchedAccount>(searchedAccount => searchedAccount.Id == id)).FirstOrDefault();

    public async Task<SearchedAccount?> GetAsync(
        string username) =>
        (await database.GetAsync<SearchedAccount>(searchedAccount => searchedAccount.Username == username)).FirstOrDefault();


    public Task<int> AddAsync(
        SearchedAccount searchedAccount) =>
        database.AddAsync(searchedAccount, searchedAccount.Id.HasValue);


    public Task RemoveAsync(
        int id) =>
        database.RemoveAsync<SearchedAccount>(searchedAccount => searchedAccount.Id == id);

    public Task RemoveAsync(
        string username) =>
        database.RemoveAsync<SearchedAccount>(searchedAccount => searchedAccount.Username == username);
}