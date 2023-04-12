using SQLite;

namespace InstaTracker.Models;

public class SearchedAccount
{
    public SearchedAccount() { }

    public SearchedAccount(
        string username,
        string fullName,
        string? profilePicture,
        int? id = null)
    {
        Username = username;
        FullName = fullName;
        ProfilePicture = profilePicture;
        Id = id;
    }

    public string Username { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string? ProfilePicture { get; set; } = default!;

    [PrimaryKey]
    [AutoIncrement]
    public int? Id { get; set; } = null;
}