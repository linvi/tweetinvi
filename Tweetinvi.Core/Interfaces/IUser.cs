using System;
using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Async;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// Contract defining what a user on twitter can do
    /// </summary>
    public interface IUser : IUserIdentifier, IUserAsync, IEquatable<IUser>
    {
        IUserDTO UserDTO { get; set; }
        IUserIdentifier UserIdentifier { get; }

        #region Twitter API Fields

        string Name { get; }

        string Description { get; }

        ITweetDTO Status { get; }

        DateTime CreatedAt { get; }

        string Location { get; }

        bool GeoEnabled { get; }

        string Url { get; }

        Language Language { get; }

        int StatusesCount { get; }

        int FollowersCount { get; }

        int FriendsCount { get; }

        bool Following { get; }

        bool Protected { get; }

        bool Verified { get; }

        IUserEntities Entities { get; }

        bool Notifications { get; }

        string ProfileImageUrl { get; }

        string ProfileImageUrlFullSize { get; }

        string ProfileImageUrl400x400 { get; }

        string ProfileImageUrlHttps { get; }

        bool FollowRequestSent { get; }

        bool DefaultProfile { get; }

        bool DefaultProfileImage { get; }

        int FavouritesCount { get; }

        int ListedCount { get; }

        string ProfileSidebarFillColor { get; }

        string ProfileSidebarBorderColor { get; }

        bool ProfileBackgroundTile { get; }

        string ProfileBackgroundColor { get; }

        string ProfileBackgroundImageUrl { get; }

        string ProfileBackgroundImageUrlHttps { get; }

        string ProfileBannerURL { get; }

        string ProfileTextColor { get; }

        string ProfileLinkColor { get; }

        bool ProfileUseBackgroundImage { get; }

        bool IsTranslator { get; }

        bool ShowAllInlineMedia { get; }

        bool ContributorsEnabled { get; }

        int? UtcOffset { get; }

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
        /// List of friend Ids
        /// </summary>
        List<long> FriendIds { get; set; }

        /// <summary>
        /// List of friends with their profile information
        /// Requires a query per friend user
        /// </summary>
        List<IUser> Friends { get; set; }

        /// <summary>
        /// List of follower ids
        /// </summary>
        List<long> FollowerIds { get; set; }

        /// <summary>
        /// List of followers with their profile information
        /// Requires a query per friend user
        /// </summary>
        List<IUser> Followers { get; set; }

        /// <summary>
        /// List of contributors of the account
        /// </summary>
        List<IUser> Contributors { get; set; }

        /// <summary>
        /// List of the account the user is contributing to
        /// </summary>
        List<IUser> Contributees { get; set; }

        /// <summary>
        /// List of tweets as displayed on the timeline
        /// </summary>
        List<ITweet> Timeline { get; set; }

        /// <summary>
        /// List of tweets that actually are retweets
        /// </summary>
        List<ITweet> Retweets { get; set; }

        /// <summary>
        /// List of retweets from friends
        /// </summary>
        List<ITweet> FriendsRetweets { get; set; }

        /// <summary>
        /// Tweets the user created that have been retweeted by followers
        /// </summary>
        List<ITweet> TweetsRetweetedByFollowers { get; set; }

        #endregion

        // Friends
        IEnumerable<long> GetFriendIds(int maxFriendsToRetrieve = 5000);
        IEnumerable<IUser> GetFriends(int maxFriendsToRetrieve = 250);

        // Followers
        IEnumerable<long> GetFollowerIds(int maxFriendsToRetrieve = 5000);
        IEnumerable<IUser> GetFollowers(int maxFriendsToRetrieve = 250);

        // Friendship
        IRelationshipDetails GetRelationshipWith(IUser user);

        // Timeline
        IEnumerable<ITweet> GetUserTimeline(int maximumNumberOfTweets = 40);

        IEnumerable<ITweet> GetUserTimeline(IUserTimelineParameters timelineParameters);

        // Get Favorites
        IEnumerable<ITweet> GetFavorites(int maximumNumberOfTweets = 40);

        // Lists
        IEnumerable<ITwitterList> GetOwnedLists(int maximumNumberOfListsToRetrieve);
        IEnumerable<ITwitterList> GetSubscribedLists(int maximumNumberOfListsToRetrieve = TweetinviConsts.TWITTER_LIST_GET_USER_SUBSCRIPTIONS_COUNT);

        // Block
        bool BlockUser();

        bool UnBlockUser();

        // Spam
        bool ReportUserForSpam();

        // Stream Profile Image
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