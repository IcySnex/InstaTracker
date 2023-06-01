using InstagramApiSharp.Classes.Models;
using System.Collections.Generic;

namespace InstaTracker.Helpers;

class InstaUserShortComparer : IEqualityComparer<InstaUserShort>
{
    public bool Equals(InstaUserShort user1, InstaUserShort user2) =>
        user1.UserName == user2.UserName;

    public int GetHashCode(InstaUserShort user) =>
        user.UserName.GetHashCode();
}
