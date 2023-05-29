using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using InstaTracker.Services;
using Serilog;
using System.Threading.Tasks;

namespace InstaTracker.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    readonly ILogger logger;
    readonly Navigation navigation;

    public HomeViewModel(
        ILogger logger,
        Navigation navigation)
    {
        this.logger = logger;
        this.navigation = navigation;
    }


    [RelayCommand]
    void NavigateToSearch() =>
        navigation.NavigateToTab(1);

    [RelayCommand]
    async Task NavigateToLogin()
    {
        navigation.NavigateToTab(1);

        //JsonConverter c = new(logger);
        //var v = App.Provider.GetRequiredService<InfoViewModel>();
        //v.FriendshipStatus = c.ToObject<InstaStoryFriendshipStatus>("{\r\n    \"Following\": false,\r\n    \"FollowedBy\": false,\r\n    \"Blocking\": false,\r\n    \"Muting\": false,\r\n    \"IsPrivate\": true,\r\n    \"IncomingRequest\": false,\r\n    \"OutgoingRequest\": false,\r\n    \"IsBlockingReel\": false,\r\n    \"IsMutingReel\": false,\r\n    \"IsBestie\": false\r\n  }");
        //v.AccountInfo = c.ToObject<InstaUserInfo>("{\r\n    \"Pk\": 50895549834,\r\n    \"Username\": \"y14.laura\",\r\n    \"FullName\": \"Laura\",\r\n    \"IsPrivate\": true,\r\n    \"ProfilePicUrl\": \"https://scontent-muc2-1.cdninstagram.com/v/t51.2885-19/340007556_246388481170664_6857562516574423397_n.jpg?stp=dst-jpg_s150x150&_nc_ht=scontent-muc2-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_3_X0jRyutYAX-kCrXj&edm=AKralEIBAAAA&ccb=7-5&oh=00_AfBvI1tNaXiWfb9XC-uANxWl8yQAUtZyHq-kVRes1stO5w&oe=6464084F&_nc_sid=5e3072\",\r\n    \"ProfilePicId\": \"3076534545558160217_50895549834\",\r\n    \"IsVerified\": false,\r\n    \"HasAnonymousProfilePicture\": false,\r\n    \"MediaCount\": 0,\r\n    \"GeoMediaCount\": 0,\r\n    \"FollowerCount\": 292,\r\n    \"FollowingCount\": 391,\r\n    \"FollowingTagCount\": 0,\r\n    \"Biography\": \"Sc: Laura-y14 🇩🇪🇮🇹    \\nPrvt acc: @laury.prvtshitt\",\r\n    \"CanLinkEntitiesInBio\": false,\r\n    \"ExternalUrl\": \"\",\r\n    \"ExternalLynxUrl\": null,\r\n    \"HasBiographyTranslation\": false,\r\n    \"CanBoostPost\": false,\r\n    \"CanSeeOrganicInsights\": false,\r\n    \"ShowInsightsTerms\": false,\r\n    \"CanConvertToBusiness\": false,\r\n    \"CanCreateSponsorTags\": false,\r\n    \"CanBeTaggedAsSponsor\": false,\r\n    \"TotalIGTVVideos\": 0,\r\n    \"TotalArEffects\": 0,\r\n    \"ReelAutoArchive\": null,\r\n    \"IsProfileActionNeeded\": false,\r\n    \"UsertagsCount\": 0,\r\n    \"UsertagReviewEnabled\": false,\r\n    \"IsNeedy\": false,\r\n    \"HasRecommendAccounts\": false,\r\n    \"IsFavorite\": false,\r\n    \"HasChaining\": true,\r\n    \"HasPlacedOrders\": false,\r\n    \"CanTagProductsFromMerchants\": false,\r\n    \"ShowBusinessConversionIcon\": false,\r\n    \"ShowConversionEditEntry\": false,\r\n    \"AggregatePromoteEngagement\": false,\r\n    \"AllowedCommenterType\": null,\r\n    \"IsVideoCreator\": false,\r\n    \"HasProfileVideoFeed\": false,\r\n    \"IsEligibleToShowFBCrossSharingNux\": false,\r\n    \"PageIdForNewSumaBizAccount\": null,\r\n    \"AccountType\": 1,\r\n    \"ProfileContext\": \"\",\r\n    \"ProfileContextMutualFollowIds\": null,\r\n    \"IsBusiness\": false,\r\n    \"IncludeDirectBlacklistStatus\": false,\r\n    \"HasUnseenBestiesMedia\": false,\r\n    \"AutoExpandChaining\": false,\r\n    \"BiographyWithEntities\": {\r\n      \"NuxType\": null,\r\n      \"Text\": \"Sc: Laura-y14 🇩🇪🇮🇹    \\nPrvt acc: @laury.prvtshitt\",\r\n      \"Entities\": [\r\n        {\r\n          \"Hashtag\": null,\r\n          \"User\": {\r\n            \"Id\": 55124002675,\r\n            \"Name\": null\r\n          }\r\n        }\r\n      ]\r\n    },\r\n    \"IsEligibleForSchool\": false,\r\n    \"IsFavoriteForStories\": false,\r\n    \"ProfileContextIds\": [],\r\n    \"FriendshipStatus\": null,\r\n    \"HdProfilePicVersions\": [\r\n      {\r\n        \"Uri\": \"https://scontent-muc2-1.cdninstagram.com/v/t51.2885-19/340007556_246388481170664_6857562516574423397_n.jpg?stp=dst-jpg_s320x320&_nc_ht=scontent-muc2-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_3_X0jRyutYAX-kCrXj&edm=AKralEIBAAAA&ccb=7-5&oh=00_AfA3eRqjR24kVPQ4brTdfswB22P7Na2Aj21k4N8gySRE5w&oe=6464084F&_nc_sid=5e3072\",\r\n        \"Width\": 320,\r\n        \"Height\": 320,\r\n        \"ImageBytes\": null\r\n      },\r\n      {\r\n        \"Uri\": \"https://scontent-muc2-1.cdninstagram.com/v/t51.2885-19/340007556_246388481170664_6857562516574423397_n.jpg?stp=dst-jpg_s640x640&_nc_ht=scontent-muc2-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_3_X0jRyutYAX-kCrXj&edm=AKralEIBAAAA&ccb=7-5&oh=00_AfC9QHN3XfZp2TZbSc1nOQuctNy4_A4uZ50_T072GXPwlQ&oe=6464084F&_nc_sid=5e3072\",\r\n        \"Width\": 640,\r\n        \"Height\": 640,\r\n        \"ImageBytes\": null\r\n      }\r\n    ],\r\n    \"HdProfilePicUrlInfo\": {\r\n      \"Uri\": \"https://scontent-muc2-1.cdninstagram.com/v/t51.2885-19/340007556_246388481170664_6857562516574423397_n.jpg?_nc_ht=scontent-muc2-1.cdninstagram.com&_nc_cat=108&_nc_ohc=_3_X0jRyutYAX-kCrXj&edm=AKralEIBAAAA&ccb=7-5&oh=00_AfBIFJKbkobGGeHW16_WFuYFdWtolcCa_q0HFiHughxqTg&oe=6464084F&_nc_sid=5e3072\",\r\n      \"Width\": 1080,\r\n      \"Height\": 1080,\r\n      \"ImageBytes\": null\r\n    },\r\n    \"PublicPhoneNumber\": \"\",\r\n    \"ContactPhoneNumber\": \"\",\r\n    \"PublicPhoneCountryCode\": \"\",\r\n    \"ShoppablePostsCount\": 0,\r\n    \"CityId\": 0,\r\n    \"CanBeReportedAsFraud\": false,\r\n    \"Category\": null,\r\n    \"IsFavoriteForHighlights\": false,\r\n    \"IsCallToActionEnabled\": false,\r\n    \"DirectMessaging\": null,\r\n    \"Latitude\": 0,\r\n    \"FbPageCallToActionId\": null,\r\n    \"BusinessContactMethod\": 0,\r\n    \"ZipCode\": null,\r\n    \"IsInterestAccount\": false,\r\n    \"Longitude\": 0,\r\n    \"CityName\": null,\r\n    \"AddressStreet\": null,\r\n    \"HasHighlightReels\": false,\r\n    \"PublicEmail\": null,\r\n    \"ShowShoppableFeed\": false,\r\n    \"IsPotentialBusiness\": false,\r\n    \"IsBestie\": false,\r\n    \"ShowAccountTransparencyDetails\": false,\r\n    \"HighlightReshareDisabled\": false,\r\n    \"PageName\": null,\r\n    \"PageId\": 0\r\n  }");
        //v.Info = c.ToObject<Info>("{\r\n    \"Username\": \"y14.laura\",\r\n    \"FetchedAt\": \"2023-05-13T13:46:47.308367Z\",\r\n    \"FollowersCount\": 292,\r\n    \"FollowingCount\": 391,\r\n    \"FansCount\": null,\r\n    \"Followers\": null,\r\n    \"Following\": null,\r\n    \"Fans\": null,\r\n    \"Id\": null\r\n  }");
        //v.CanLoad = false;

        //await navigation.NavigateAsync(new InfoView(v));


        //navigation.NavigateToTab(2);
    }
}