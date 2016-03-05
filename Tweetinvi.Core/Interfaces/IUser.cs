using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// Contract defining what a user on twitter can do.
    /// For more information visit : https://dev.twitter.com/overview/api/users
    /// </summary>
    public interface IUser : IUserIdentifier, IUserAsync, IEquatable<IUser>
    {
        /// <summary>
        /// Property used to store the twitter properties
        /// </summary>
        IUserDTO UserDTO { get; set; }

        /// <summary>
        /// User identifier containing Id and ScreenName
        /// </summary>
        IUserIdentifier UserIdentifier { get; }

        #region Twitter API Fields

        /// <summary>
        /// The name of the user, as they’ve defined it.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Text describing the user account.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Latest tweet published by the user.
        /// </summary>
        ITweetDTO Status { get; }

        /// <summary>
        /// Date when the user account was created on Twitter.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// The user-defined location for this account’s profile. 
        /// </summary>
        string Location { get; }

        /// <summary>
        /// When true, indicates that the user has enabled the possibility of geotagging their Tweets. 
        /// This field must be true for the current user to attach geographic data.
        /// </summary>
        bool GeoEnabled { get; }

        /// <summary>
        /// A URL provided by the user in association with their profile.
        /// </summary>
        string Url { get; }

        /// <summary>
        /// Primary language of the user account.
        /// </summary>
        Language Language { get; }

        /// <summary>
        /// Number of tweets (including retweets) the user published.
        /// </summary>
        int StatusesCount { get; }

        /// <summary>
        /// Number of followers this user has
        /// </summary>
        int FollowersCount { get; }

        /// <summary>
        /// The number of users this account is following (AKA their “followings”).
        /// </summary>
        int FriendsCount { get; }

        /// <summary>
        /// When true, indicates that the authenticated user is following this user.
        /// </summary>
        bool Following { get; }

        /// <summary>
        /// When true, indicates that this user has chosen to protect their Tweets.
        /// For more information visit : https://support.twitter.com/articles/14016.
        /// </summary>
        bool Protected { get; }

        /// <summary>
        /// When true, indicates that the user has a verified account.
        /// </summary>
        bool Verified { get; }

        /// <summary>
        /// Entities which have been parsed out of the url or description fields defined by the user.
        /// </summary>
        IUserEntities Entities { get; }

        /// <summary>
        /// Indicates whether the authenticated user has chosen to receive this user’s tweets by SMS
        /// </summary>
        bool Notifications { get; }

        /// <summary>
        /// URL pointing to the user’s avatar image.
        /// </summary>
        string ProfileImageUrl { get; }

        /// <summary>
        /// URL pointing to the user’s avatar image.
        /// </summary>
        string ProfileImageUrlFullSize { get; }

        /// <summary>
        /// URL of the user 400x400 profile image
        /// </summary>
        string ProfileImageUrl400x400 { get; }

        /// <summary>
        /// URL pointing to the user’s avatar image.
        /// </summary>
        string ProfileImageUrlHttps { get; }

        /// <summary>
        /// When true, indicates that the authenticating user has issued a follow request 
        /// to this protected user account.
        /// </summary>
        bool FollowRequestSent { get; }

        /// <summary>
        /// Indicates whether the user is using Twitter default theme profile
        /// </summary>
        bool DefaultProfile { get; }

        /// <summary>
        /// Indicates whether the user has uploaded his own profile image
        /// </summary>
        bool DefaultProfileImage { get; }

        /// <summary>
        /// Number of tweets this user has favourited in the account’s lifetime.
        /// </summary>
        int FavouritesCount { get; }

        /// <summary>
        /// The number of public lists that this user is a member of.
        /// </summary>
        int ListedCount { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display sidebar backgrounds with in their Twitter UI.
        /// </summary>
        string ProfileSidebarFillColor { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display sidebar borders with in their Twitter UI.
        /// </summary>
        string ProfileSidebarBorderColor { get; }

        /// <summary>
        /// When true, indicates that the user’s profile_background_image_url should be tiled when displayed.
        /// </summary>
        bool ProfileBackgroundTile { get; }

        /// <summary>
        /// The hexadecimal color chosen by the user for their background.
        /// </summary>
        string ProfileBackgroundColor { get; }

        /// <summary>
        /// URL pointing to the background image the user has uploaded for their profile.
        /// </summary>
        string ProfileBackgroundImageUrl { get; }

        /// <summary>
        /// URL pointing to the background image the user has uploaded for their profile.
        /// </summary>
        string ProfileBackgroundImageUrlHttps { get; }

        /// <summary>
        /// URL pointing to the standard web representation of the user’s uploaded profile banner. 
        /// By adding a final path element of the URL, you can obtain different image sizes 
        /// optimized for specific displays.
        /// </summary>
        string ProfileBannerURL { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display text with in their Twitter UI.
        /// </summary>
        string ProfileTextColor { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display links with in their Twitter UI.
        /// </summary>
        string ProfileLinkColor { get; }

        /// <summary>
        /// When true, indicates the user wants their uploaded background image to be used.
        /// </summary>
        bool ProfileUseBackgroundImage { get; }

        /// <summary>
        /// When true, indicates that the user is a participant in Twitter’s translator community.
        /// </summary>
        bool IsTranslator { get; }

        /// <summary>
        /// Indicates that the user would like to see media inline.
        /// </summary>
        bool ShowAllInlineMedia { get; }

        /// <summary>
        /// Indicates that the account has the contributor mode enabled
        /// </summary>
        bool ContributorsEnabled { get; }

        /// <summary>
        /// The offset from GMT/UTC in seconds.
        /// </summary>
        int? UtcOffset { get; }

        /// <summary>
        ///  A string describing the Time Zone this user declares themselves within.
        /// </summary>
        string TimeZone { get; }

        // The withheld properties are not always provided in the json result

        /// <summary>
        /// If a user is withheld in a country, the information will be listed there
        /// </summary>
        IEnumerable<string> WithheldInCountries { get; }

        /// <summary>
        /// States whether the user or his tweets are being withheld in a specific country
        /// </summary>
        string WithheldScope { get; }

        #endregion

        #region Tweetinvi API Fields

        /// <summary>
        /// Property allowing the developers to store friend ids.
        /// </summary>
        List<long> FriendIds { get; set; }

        /// <summary>
        /// Property allowing the developers to store friends.
        /// </summary>
        List<IUser> Friends { get; set; }

        /// <summary>
        /// List of follower ids
        /// </summary>
        List<long> FollowerIds { get; set; }

        /// <summary>
        /// Property allowing the developers to store follower ids.
        /// </summary>
        List<IUser> Followers { get; set; }

        /// <summary>
        /// Property allowing the developers to store contributors.
        /// </summary>
        List<IUser> Contributors { get; set; }

        /// <summary>
        /// Property allowing the developers to store the user accounts 
        /// the current user is contributing to.
        /// </summary>
        List<IUser> Contributees { get; set; }

        /// <summary>
        /// Property allowing the developers to store contributors.
        /// </summary>
        List<ITweet> Timeline { get; set; }

        /// <summary>
        /// Property allowing the developers to store retweets.
        /// </summary>
        List<ITweet> Retweets { get; set; }

        /// <summary>
        /// Property allowing the developers to store retweets from friend.
        /// </summary>
        List<ITweet> FriendsRetweets { get; set; }

        /// <summary>
        /// Property allowing the developers to store tweets created by the user 
        /// that have been retweeted by followers.
        /// </summary>
        List<ITweet> TweetsRetweetedByFollowers { get; set; }

        #endregion

        // Friends

        /// <summary>
        /// Get a list of the user's friend ids.
        /// </summary>
        IEnumerable<long> GetFriendIds(int maxFriendsToRetrieve = 5000);

        /// <summary>
        /// Get a list of the user's friends.
        /// </summary>
        IEnumerable<IUser> GetFriends(int maxFriendsToRetrieve = 250);

        // Followers

        /// <summary>
        /// Get a list of the user's follower ids.
        /// </summary>
        IEnumerable<long> GetFollowerIds(int maxFriendsToRetrieve = 5000);

        /// <summary>
        /// Get a list of the user's followers.
        /// </summary>
        IEnumerable<IUser> GetFollowers(int maxFriendsToRetrieve = 250);

        // Friendship

        /// <summary>
        /// Get the relationship details between the user and another one.
        /// </summary>
        IRelationshipDetails GetRelationshipWith(IUser user);

        // Timeline

        /// <summary>
        /// Get the tweets published by the user.
        /// </summary>
        IEnumerable<ITweet> GetUserTimeline(int maximumNumberOfTweets = 40);

        /// <summary>
        /// Get the tweets published by the user.
        /// </summary>
        IEnumerable<ITweet> GetUserTimeline(IUserTimelineParameters timelineParameters);

        // Get Favorites

        /// <summary>
        /// Get the tweets favourited by the user.
        /// </summary>
        IEnumerable<ITweet> GetFavorites(int maximumNumberOfTweets = 40);

        /// <summary>
        /// Get the tweets favourited by the user.
        /// </summary>
        IEnumerable<ITweet> GetFavorites(IGetUserFavoritesParameters parameters);

        // Lists

        /// <summary>
        /// Get the lists owned by the user.
        /// </summary>
        IEnumerable<ITwitterList> GetOwnedLists(int maximumNumberOfListsToRetrieve);

        /// <summary>
        /// Get the lists the user has subscribed to.
        /// </summary>
        IEnumerable<ITwitterList> GetSubscribedLists(int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT);

        // Block

        /// <summary>
        /// Make the authenticated user block the user.
        /// </summary>
        bool BlockUser();

        /// <summary>
        /// Make the authenticated user unblock the user.
        /// </summary>
        bool UnBlockUser();

        // Spam

        /// <summary>
        /// Report the user for spam.
        /// </summary>
        bool ReportUserForSpam();

        // Stream Profile Image

        /// <summary>
        /// Get a stream to get the profile image of this user.
        /// </summary>
        Stream GetProfileImageStream(ImageSize imageSize = ImageSize.normal);

        /// <summary>
        /// Get the list of contributors to the account of the current user
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributors
        /// </summary>
        /// <returns>The list of contributors to the account of the current user</returns>
        IEnumerable<IUser> GetContributors(bool createContributorList = false);

        /// <summary>
        /// Get the list of accounts the current user is allowed to update
        /// Update the matching attribute of the current user if the parameter is true
        /// Return the list of contributees
        /// </summary>
        /// <returns>The list of accounts the current user is allowed to update</returns>
        IEnumerable<IUser> GetContributees(bool createContributeeList = false);
    }
}