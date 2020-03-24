using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;
using Tweetinvi.Client;
// ReSharper disable UnusedMember.Global

namespace Tweetinvi.Models
{
    /// <summary>
    /// Contract defining what a user on twitter can do.
    /// For more information visit : https://dev.twitter.com/overview/api/users
    /// </summary>
    public interface IUser : IUserIdentifier, IEquatable<IUser>
    {
        /// <summary>
        /// Client used by the instance to perform any request to Twitter
        /// </summary>
        ITwitterClient Client { get; set; }

        /// <summary>
        /// Property used to store the twitter properties
        /// </summary>
        IUserDTO UserDTO { get; set; }

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
        /// When true, indicates that the user has enabled the possibility of geo tagging their Tweets.
        /// This field must be true for the current user to attach geographic data.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? GeoEnabled { get; }

        /// <summary>
        /// A URL provided by the user in association with their profile.
        /// </summary>
        string Url { get; }

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
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? Following { get; }

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
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? Notifications { get; }

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
        // ReSharper disable once InconsistentNaming
        string ProfileImageUrl400x400 { get; }

        /// <summary>
        /// When true, indicates that the authenticating user has issued a follow request
        /// to this protected user account.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? FollowRequestSent { get; }

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
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileSidebarFillColor { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display sidebar borders with in their Twitter UI.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileSidebarBorderColor { get; }

        /// <summary>
        /// When true, indicates that the user’s profile_background_image_url should be tiled when displayed.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool ProfileBackgroundTile { get; }

        /// <summary>
        /// The hexadecimal color chosen by the user for their background.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileBackgroundColor { get; }

        /// <summary>
        /// URL pointing to the background image the user has uploaded for their profile.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileBackgroundImageUrl { get; }

        /// <summary>
        /// URL pointing to the background image the user has uploaded for their profile.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileBackgroundImageUrlHttps { get; }

        /// <summary>
        /// URL pointing to the standard web representation of the user’s uploaded profile banner.
        /// By adding a final path element of the URL, you can obtain different image sizes
        /// optimized for specific displays.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileBannerURL { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display text with in their Twitter UI.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileTextColor { get; }

        /// <summary>
        /// The hexadecimal color the user has chosen to display links with in their Twitter UI.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        string ProfileLinkColor { get; }

        /// <summary>
        /// When true, indicates the user wants their uploaded background image to be used.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool ProfileUseBackgroundImage { get; }

        /// <summary>
        /// When true, indicates that the user is a participant in Twitter’s translator community.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? IsTranslator { get; }

        /// <summary>
        /// Indicates that the account has the contributor mode enabled
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        bool? ContributorsEnabled { get; }

        /// <summary>
        /// The offset from GMT/UTC in seconds.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
        int? UtcOffset { get; }

        /// <summary>
        ///  A string describing the Time Zone this user declares themselves within.
        /// </summary>
        [Obsolete("Twitter documentation states that this property is deprecated but they currently keep returning data.")]
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

        // Friends

        /// <summary>
        /// Get a list of the user's friend ids.
        /// </summary>
        ITwitterIterator<long> GetFriendIds();

        /// <summary>
        /// Get a list of the user's friends.
        /// </summary>
        IMultiLevelCursorIterator<long, IUser> GetFriends();

        // Followers

        /// <summary>
        /// Get a list of the user's follower ids.
        /// </summary>
        ITwitterIterator<long> GetFollowerIds();

        IMultiLevelCursorIterator<long, IUser> GetFollowers();

        // Friendship

        /// <summary>
        /// Get the relationship details between the user and another one.
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWith(IUserIdentifier user);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWith(long userId);

        /// <summary>
        /// Get the relationship between the authenticated user (source) and another user (target).
        /// </summary>
        Task<IRelationshipDetails> GetRelationshipWith(string username);

        // Timeline

        /// <inheritdoc cref="ITimelinesClient.GetUserTimeline(IUserIdentifier)"/>
        Task<ITweet[]> GetUserTimeline();

        // Get Favorites

        /// <inheritdoc cref="ITweetsClient.GetUserFavoriteTweets(IUserIdentifier)"/>
        Task<ITweet[]> GetFavoriteTweets();

        // Lists

        /// <inheritdoc cref="IListsClient.GetListsOwnedByUser(IUserIdentifier)"/>
        Task<ITwitterList[]> GetOwnedLists();

        /// <inheritdoc cref="IListsClient.GetUserListSubscriptions(IUserIdentifier)"/>
        Task<ITwitterList[]> GetListSubscriptions();

        // Block

        /// <summary>
        /// Make the authenticated user block the user.
        /// </summary>
        Task BlockUser();

        /// <summary>
        /// Make the authenticated user unblock the user.
        /// </summary>
        Task UnblockUser();

        // Spam

        /// <summary>
        /// Report the user for spam.
        /// </summary>
        Task ReportUserForSpam();

        // Stream Profile Image

        /// <summary>
        /// Get a stream to get the profile image of this user.
        /// </summary>
        Task<Stream> GetProfileImageStream();

        /// <summary>
        /// Get a stream to get the profile image of this user.
        /// </summary>
        Task<Stream> GetProfileImageStream(ImageSize imageSize);
    }
}