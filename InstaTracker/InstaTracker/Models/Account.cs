using SQLite;

namespace InstaTracker.Models;

public class Account
{
    public Account() { }

    public Account(
        string username,
        string? password,
        string fullName,
        string? profilePicture,
        string stateJson,
        int? id = null)
    {
        Username = username;
        Password = password;
        FullName = fullName;
        ProfilePicture = profilePicture;
        StateJson = stateJson;
        Id = id;
    }

    public string Username { get; set; } = default!;

    public string? Password { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string? ProfilePicture { get; set; } = default!;

    public string StateJson { get; set; } = default!;

    [PrimaryKey]
    [AutoIncrement]
    public int? Id { get; set; } = null;
}