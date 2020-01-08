using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Tweetinvi User.
    /// </summary>
    public class User : IUser
    {
        private const string REGEX_PROFILE_IMAGE_SIZE = "_[^\\W_]+(?=(?:\\.[a-zA-Z0-9_]+$))";
        public ITwitterClient Client { get; set; }

        // ReSharper disable once InconsistentNaming
        private readonly ITwitterListController _twitterListController;

        public IUserDTO UserDTO { get; set; }

        public IUserIdentifier UserIdentifier
        {
            get { return UserDTO; }
        }

        #region Public Attributes

        #region Twitter API Attributes

        // This region represents the information accessible from a Twitter API
        // when querying for a User

        public long? Id
        {
            get { return UserDTO?.Id; }
            set { throw new InvalidOperationException("Cannot set the Id of a user"); }
        }

        public string IdStr
        {
            get { return UserDTO?.IdStr; }
            set { throw new InvalidOperationException("Cannot set the Id of a user"); }
        }

        public string ScreenName
        {
            get { return UserDTO?.ScreenName; }
            set { throw new InvalidOperationException("Cannot set the ScreenName of a user"); }
        }

        public string Name
        {
            get { return UserDTO.Name; }
        }

        public string Description
        {
            get { return UserDTO.Description; }
        }

        public ITweetDTO Status
        {
            get { return UserDTO.Status; }
        }

        public DateTime CreatedAt
        {
            get { return UserDTO.CreatedAt; }
        }

        public string Location
        {
            get { return UserDTO.Location; }
        }

        public bool GeoEnabled
        {
            get { return UserDTO.GeoEnabled; }
        }

        public string Url
        {
            get { return UserDTO.Url; }
        }

        public int StatusesCount
        {
            get { return UserDTO.StatusesCount; }
        }

        public int FollowersCount
        {
            get { return UserDTO.FollowersCount; }
        }

        public int FriendsCount
        {
            get { return UserDTO.FriendsCount; }
        }

        public bool Following
        {
            get { return UserDTO.Following; }
        }

        public bool Protected
        {
            get { return UserDTO.Protected; }
        }

        public bool Verified
        {
            get { return UserDTO.Verified; }
        }

        public IUserEntities Entities
        {
            get { return UserDTO.Entities; }
        }

        public string ProfileImageUrl
        {
            get { return UserDTO.ProfileImageUrl; }
        }

        public string ProfileImageUrlFullSize
        {
            get
            {
                var profileImageURL = ProfileImageUrl;
                if (string.IsNullOrEmpty(profileImageURL))
                {
                    return null;
                }

                return Regex.Replace(profileImageURL, REGEX_PROFILE_IMAGE_SIZE, string.Empty);
            }
        }

        public string ProfileImageUrl400x400
        {
            get
            {
                var profileImageURL = ProfileImageUrl;
                if (string.IsNullOrEmpty(profileImageURL))
                {
                    return null;
                }

                return Regex.Replace(profileImageURL, REGEX_PROFILE_IMAGE_SIZE, "_400x400");
            }
        }

        public string ProfileImageUrlHttps
        {
            get { return UserDTO.ProfileImageUrlHttps; }
        }

        public bool FollowRequestSent
        {
            get { return UserDTO.FollowRequestSent; }
        }

        public bool DefaultProfile
        {
            get { return UserDTO.DefaultProfile; }
        }

        public bool DefaultProfileImage
        {
            get { return UserDTO.DefaultProfileImage; }
        }

        public int FavouritesCount
        {
            get { return UserDTO.FavoritesCount ?? 0; }
        }

        public int ListedCount
        {
            get { return UserDTO.ListedCount ?? 0; }
        }

        public string ProfileSidebarFillColor
        {
            get { return UserDTO.ProfileSidebarFillColor; }
        }

        public string ProfileSidebarBorderColor
        {
            get { return UserDTO.ProfileSidebarBorderColor; }
        }

        public bool ProfileBackgroundTile
        {
            get { return UserDTO.ProfileBackgroundTile; }
        }

        public string ProfileBackgroundColor
        {
            get { return UserDTO.ProfileBackgroundColor; }
        }

        public string ProfileBackgroundImageUrl
        {
            get { return UserDTO.ProfileBackgroundImageUrl; }
        }

        public string ProfileBackgroundImageUrlHttps
        {
            get { return UserDTO.ProfileBackgroundImageUrlHttps; }
        }

        public string ProfileBannerURL
        {
            get { return UserDTO.ProfileBannerURL; }
        }

        public string ProfileTextColor
        {
            get { return UserDTO.ProfileTextColor; }
        }

        public string ProfileLinkColor
        {
            get { return UserDTO.ProfileLinkColor; }
        }

        public bool ProfileUseBackgroundImage
        {
            get { return UserDTO.ProfileUseBackgroundImage; }
        }

        public bool IsTranslator
        {
            get { return UserDTO.IsTranslator; }
        }

        public bool ContributorsEnabled
        {
            get { return UserDTO.ContributorsEnabled; }
        }

        public int? UtcOffset
        {
            get { return UserDTO.UtcOffset; }
        }

        public string TimeZone
        {
            get { return UserDTO.TimeZone; }
        }

        public IEnumerable<string> WithheldInCountries
        {
            get { return UserDTO.WithheldInCountries; }
        }

        public string WithheldScope
        {
            get { return UserDTO.WithheldScope; }
        }

        [Obsolete("Twitter's documentation states that this property is deprecated")]
        public bool Notifications
        {
            get { return UserDTO.Notifications; }
        }

        #endregion

        #region Tweetinvi API Attributes

        public List<long> FriendIds { get; set; }
        public List<IUser> Friends { get; set; }
        public List<long> FollowerIds { get; set; }
        public List<IUser> Followers { get; set; }
        public List<IUser> Contributors { get; set; }
        public List<IUser> Contributees { get; set; }
        public List<ITweet> Timeline { get; set; }
        public List<ITweet> Retweets { get; set; }
        public List<ITweet> FriendsRetweets { get; set; }
        public List<ITweet> TweetsRetweetedByFollowers { get; set; }

        #endregion

        #endregion

        public User(IUserDTO userDTO, ITwitterListController twitterListController)
        {
            UserDTO = userDTO;
            _twitterListController = twitterListController;
        }

        // Friends
        public virtual ITwitterIterator<long> GetFriendIds()
        {
            return Client?.Users.GetFriendIds(new GetFriendIdsParameters(this));
        }

        public virtual IMultiLevelCursorIterator<long, IUser> GetFriends()
        {
            return Client?.Users.GetFriends(new GetFriendsParameters(this));
        }

        // Followers
        public virtual ITwitterIterator<long> GetFollowerIds()
        {
            return Client?.Users.GetFollowerIds(new GetFollowerIdsParameters(this));
        }

        public virtual IMultiLevelCursorIterator<long, IUser> GetFollowers()
        {
            return Client?.Users.GetFollowers(new GetFollowersParameters(this));
        }

        // Relationship
        public Task<IRelationshipDetails> GetRelationshipWith(IUserIdentifier user)
        {
            return Client.Users.GetRelationshipBetween(this, user);
        }

        public Task<IRelationshipDetails> GetRelationshipWith(long? userId)
        {
            return Client.Users.GetRelationshipBetween(this, userId);
        }

        public Task<IRelationshipDetails> GetRelationshipWith(string username)
        {
            return Client.Users.GetRelationshipBetween(this, username);
        }

        // Timeline
        public ITwitterIterator<ITweet, long?> GetUserTimelineIterator()
        {
            return Client.Timeline.GetUserTimelineIterator(this);
        }

        // Favorites
        public virtual ITwitterIterator<ITweet, long?> GetFavoriteTweets()
        {
            return Client.Tweets.GetFavoriteTweets(this);
        }

        // Lists
        public Task<IEnumerable<ITwitterList>> GetSubscribedLists(int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_GET_USER_SUBSCRIPTIONS_COUNT)
        {
            return _twitterListController.GetUserSubscribedLists(this, maximumNumberOfListsToRetrieve);
        }

        public Task<IEnumerable<ITwitterList>> GetOwnedLists(int maximumNumberOfListsToRetrieve = TweetinviConsts.LIST_OWNED_COUNT)
        {
            return _twitterListController.GetUserOwnedLists(this, maximumNumberOfListsToRetrieve);
        }

        // Block User
        public virtual Task BlockUser()
        {
            return Client.Account.BlockUser(this);
        }

        public virtual Task UnBlockUser()
        {
            return Client.Account.UnBlockUser(this);
        }

        // Spam
        public virtual Task ReportUserForSpam()
        {
            return Client.Account.ReportUserForSpam(this);
        }

        // Stream Profile Image
        public Task<Stream> GetProfileImageStream()
        {
            return GetProfileImageStream(ImageSize.Normal);
        }

        public Task<Stream> GetProfileImageStream(ImageSize imageSize)
        {
            return Client.Users.GetProfileImageStream(new GetProfileImageParameters(this)
            {
                ImageSize = imageSize
            });
        }

        // Contributors
        public IEnumerable<IUser> GetContributors(bool createContributorList = false)
        {
            // string query = Resources.User_GetContributors;
            throw new NotImplementedException();
        }

        // Contributees
        public IEnumerable<IUser> GetContributees(bool createContributeeList = false)
        {
            // string query = Resources.User_GetContributees;
            throw new NotImplementedException();
        }

        public bool Equals(IUser other)
        {
            if (other == null)
            {
                return false;
            }

            return Id == other.Id || ScreenName == other.ScreenName;
        }

        public override string ToString()
        {
            return UserDTO?.Name ?? "Undefined";
        }
    }
}