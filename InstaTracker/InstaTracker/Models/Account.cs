using InstagramApiSharp.Classes;
using SQLite;

namespace InstaTracker.Models;

public class Account
{
    public Account() { }

    public Account(
        string username,
        string password,
        string? stateJson = null,
        int? id = null)
    {
        Username = username;
        Password = password;
        StateJson = stateJson;
        Id = id;
    }

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? StateJson { get; set; } = null;

    [PrimaryKey]
    [AutoIncrement]
    public int? Id { get; set; } = null;
}