using SQLite;

namespace InstaTracker.Models;

public class SearchedAccount
{
    public SearchedAccount() { }

    public SearchedAccount(
        string username,
        string fullName,
        string? profilePicture,
        bool isPrivate,
        bool isFollowing,
        string? searchScoialContext,
        int? id = null)
    {
        Username = username;
        FullName = fullName;
        ProfilePicture = profilePicture;
        IsPrivate = isPrivate;
        IsFollowing = isFollowing;
        SearchSocialContext = searchScoialContext;
        Id = id;
    }

    public string Username { get; set; } = default!;

    public string FullName { get; set; } = default!;

    public string? ProfilePicture { get; set; } = default!;

    public bool IsPrivate { get; set; } = default!;
    
    public bool IsFollowing { get; set; } = default!;

    public string? SearchSocialContext { get; set; } = default!;

    [PrimaryKey]
    [AutoIncrement]
    public int? Id { get; set; } = null;
}