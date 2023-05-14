using InstagramApiSharp.Classes.Models;
using SQLite;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensions.Extensions.TextBlob;
using System;
using System.Collections.Generic;

namespace InstaTracker.Models;

public class Info
{
    public Info() { }

    public Info(
        string username,
        DateTime fetchedAt,
        long? followersCount = null,
        long? followingCount = null,
        long? fansCount = null,
        List<InstaUserShort>? followers = null,
        List<InstaUserShort>? following = null,
        List<InstaUserShort>? fans = null,
        int? id = null)
    {
        Username = username;
        FetchedAt = fetchedAt;
        FollowersCount = followersCount;
        FollowingCount = followingCount;
        FansCount = fansCount;
        Followers = followers;
        Following = following;
        Fans = fans;
        Id = id;
    }

    public string Username { get; set; } = default!;

    public DateTime FetchedAt { get; set; } = default!;

    public long? FollowersCount { get; set; } = null;

    public long? FollowingCount { get; set; } = null;

    public long? FansCount { get; set; } = null;

    [TextBlob(nameof(FollowersBlobbed))]
    public List<InstaUserShort>? Followers { get; set; } = null;
    public string? FollowersBlobbed { get; set; } = null;

    [TextBlob(nameof(FollowingBlobbed))]
    public List<InstaUserShort>? Following { get; set; } = null;
    public string? FollowingBlobbed { get; set; } = null;

    [TextBlob(nameof(FansBlobbed))]
    public List<InstaUserShort>? Fans { get; set; } = null;
    public string? FansBlobbed { get; set; } = null;


    [PrimaryKey]
    [AutoIncrement]
    public int? Id { get; set; } = null;
}