using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Tweetinvi.Core.Interfaces.Models
{
    public class CredentialsRateLimits : ICredentialsRateLimits
    {
        public CredentialsRateLimits()
        {
            CreatedAt = DateTime.Now;
        }

        public DateTime CreatedAt { get; private set; }

        public string RateLimitContext { get; set; }
        public bool IsApplicationOnlyCredentials { get; set; }

        #region Account
        public IEndpointRateLimit AccountLoginVerificationEnrollmentLimit
        {
            get { return _resources.AccountRateLimits["/account/login_verification_enrollment"]; }
        }

        public IEndpointRateLimit AccountSettingsLimit
        {
            get { return _resources.AccountRateLimits["/account/settings"]; }
        }

        public IEndpointRateLimit AccountUpdateProfileLimit
        {
            get { return _resources.AccountRateLimits["/account/update_profile"]; }
        }

        public IEndpointRateLimit AccountVerifyCredentialsLimit
        {
            get { return _resources.AccountRateLimits["/account/verify_credentials"]; }
        }
        #endregion

        #region Application
        public IEndpointRateLimit ApplicationRateLimitStatusLimit
        {
            get { return _resources.ApplicationRateLimits["/application/rate_limit_status"]; }
        }
        #endregion

        #region Block
        public IEndpointRateLimit BlocksIdsLimit
        {
            get { return _resources.BlocksRateLimits["/blocks/ids"]; }
        }

        public IEndpointRateLimit BlocksListLimit
        {
            get { return _resources.BlocksRateLimits["/blocks/list"]; }
        }
        #endregion

        #region Device
        public IEndpointRateLimit DeviceTokenLimit
        {
            get { return _resources.DeviceRateLimits["/device/token"]; }
        }
        #endregion

        #region DirectMessages
        public IEndpointRateLimit DirectMessagesLimit
        {
            get { return _resources.DirectMessagesRateLimits["/direct_messages"]; }
        }

        public IEndpointRateLimit DirectMessagesSentLimit
        {
            get { return _resources.DirectMessagesRateLimits["/direct_messages/sent"]; }
        }

        public IEndpointRateLimit DirectMessagesSentAndReceivedLimit
        {
            get { return _resources.DirectMessagesRateLimits["/direct_messages/sent_and_received"]; }
        }

        public IEndpointRateLimit DirectMessagesShowLimit
        {
            get { return _resources.DirectMessagesRateLimits["/direct_messages/show"]; }
        }
        #endregion

        #region Favourite
        public IEndpointRateLimit FavoritesListLimit
        {
            get { return _resources.FavoritesRateLimits["/favorites/list"]; }
        }
        #endregion

        #region Followers
        public IEndpointRateLimit FollowersIdsLimit
        {
            get { return _resources.FollowersRateLimits["/followers/ids"]; }
        }

        public IEndpointRateLimit FollowersListLimit
        {
            get { return _resources.FollowersRateLimits["/followers/list"]; }
        }
        #endregion

        #region Friends
        public IEndpointRateLimit FriendsIdsLimit
        {
            get { return _resources.FriendsRateLimits["/friends/ids"]; }
        }

        public IEndpointRateLimit FriendsListLimit
        {
            get { return _resources.FriendsRateLimits["/friends/list"]; }
        }

        public IEndpointRateLimit FriendsFollowingIdsLimit
        {
            get { return _resources.FriendsRateLimits["/friends/following/ids"]; }
        }

        public IEndpointRateLimit FriendsFollowingListLimit
        {
            get { return _resources.FriendsRateLimits["/friends/following/list"]; }
        }

        #endregion

        #region Friendships
        public IEndpointRateLimit FriendshipsIncomingLimit
        {
            get { return _resources.FriendshipsRateLimits["/friendships/incoming"]; }
        }

        public IEndpointRateLimit FriendshipsLookupLimit
        {
            get { return _resources.FriendshipsRateLimits["/friendships/lookup"]; }
        }

        public IEndpointRateLimit FriendshipsNoRetweetsIdsLimit
        {
            get { return _resources.FriendshipsRateLimits["/friendships/no_retweets/ids"]; }
        }

        public IEndpointRateLimit FriendshipsOutgoingLimit
        {
            get { return _resources.FriendshipsRateLimits["/friendships/outgoing"]; }
        }

        public IEndpointRateLimit FriendshipsShowLimit
        {
            get { return _resources.FriendshipsRateLimits["/friendships/show"]; }
        }
        #endregion

        #region Geo
        public IEndpointRateLimit GeoGetPlaceFromIdLimit
        {
            get { return _resources.GeoRateLimits["/geo/id/:place_id"]; }
        }

        public IEndpointRateLimit GeoReverseGeoCodeLimit
        {
            get { return _resources.GeoRateLimits["/geo/reverse_geocode"]; }
        }

        public IEndpointRateLimit GeoSearchLimit
        {
            get { return _resources.GeoRateLimits["/geo/search"]; }
        }

        public IEndpointRateLimit GeoSimilarPlacesLimit
        {
            get { return _resources.GeoRateLimits["/geo/similar_places"]; }
        }
        #endregion

        #region Help
        public IEndpointRateLimit HelpConfigurationLimit
        {
            get { return _resources.HelpRateLimits["/help/configuration"]; }
        }

        public IEndpointRateLimit HelpLanguagesLimit
        {
            get { return _resources.HelpRateLimits["/help/languages"]; }
        }

        public IEndpointRateLimit HelpPrivacyLimit
        {
            get { return _resources.HelpRateLimits["/help/privacy"]; }
        }

        public IEndpointRateLimit HelpSettingsLimit
        {
            get { return _resources.HelpRateLimits["/help/settings"]; }
        }

        public IEndpointRateLimit HelpTosLimit
        {
            get { return _resources.HelpRateLimits["/help/tos"]; }
        }
        #endregion

        #region Lists
        public IEndpointRateLimit ListsListLimit
        {
            get { return _resources.ListsRateLimits["/lists/list"]; }
        }

        public IEndpointRateLimit ListsMembersLimit
        {
            get { return _resources.ListsRateLimits["/lists/members"]; }
        }

        public IEndpointRateLimit ListsMembersShowLimit
        {
            get { return _resources.ListsRateLimits["/lists/members/show"]; }
        }

        public IEndpointRateLimit ListsMembershipsLimit
        {
            get { return _resources.ListsRateLimits["/lists/memberships"]; }
        }

        public IEndpointRateLimit ListsOwnershipsLimit
        {
            get { return _resources.ListsRateLimits["/lists/ownerships"]; }
        }

        public IEndpointRateLimit ListsShowLimit
        {
            get { return _resources.ListsRateLimits["/lists/show"]; }
        }

        public IEndpointRateLimit ListsStatusesLimit
        {
            get { return _resources.ListsRateLimits["/lists/statuses"]; }
        }

        public IEndpointRateLimit ListsSubscribersLimit
        {
            get { return _resources.ListsRateLimits["/lists/subscribers"]; }
        }

        public IEndpointRateLimit ListsSubscribersShowLimit
        {
            get { return _resources.ListsRateLimits["/lists/subscribers/show"]; }
        }

        public IEndpointRateLimit ListsSubscriptionsLimit
        {
            get { return _resources.ListsRateLimits["/lists/subscriptions"]; }
        }
        #endregion

        #region Mutes
        public IEndpointRateLimit MutesUserList
        {
            get { return _resources.MutesRateLimits["/mutes/users/list"]; }
        }

        public IEndpointRateLimit MutesUserIds
        {
            get { return _resources.MutesRateLimits["/mutes/users/ids"]; }
        }

        #endregion

        #region SavedSearches
        public IEndpointRateLimit SavedSearchDestroyList
        {
            get { return _resources.SavedSearchesRateLimits["/saved_searches/destroy/:id"]; }
        }

        public IEndpointRateLimit SavedSearchesListLimit
        {
            get { return _resources.SavedSearchesRateLimits["/saved_searches/list"]; }
        }

        public IEndpointRateLimit SavedSearchesShowIdLimit
        {
            get { return _resources.SavedSearchesRateLimits["/saved_searches/show/:id"]; }
        }
        #endregion

        #region Search
        public IEndpointRateLimit SearchTweetsLimit
        {
            get { return _resources.SearchRateLimits["/search/tweets"]; }
        }
        #endregion

        #region Statuses

        public IEndpointRateLimit StatusesFriendsLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/friends"]; }
        }

        public IEndpointRateLimit StatusesHomeTimelineLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/home_timeline"]; }
        }

        public IEndpointRateLimit StatusesLookupLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/lookup"]; }
        }

        public IEndpointRateLimit StatusesMentionsTimelineLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/mentions_timeline"]; }
        }

        public IEndpointRateLimit StatusesOembedLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/oembed"]; }
        }

        public IEndpointRateLimit StatusesRetweetersIdsLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/retweeters/ids"]; }
        }

        public IEndpointRateLimit StatusesRetweetsIdLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/retweets/:id"]; }
        }

        public IEndpointRateLimit StatusesRetweetsOfMeLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/retweets_of_me"]; }
        }

        public IEndpointRateLimit StatusesShowIdLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/show/:id"]; }
        }

        public IEndpointRateLimit StatusesUserTimelineLimit
        {
            get { return _resources.StatusesRateLimits["/statuses/user_timeline"]; }
        }
        #endregion

        #region Trends
        public IEndpointRateLimit TrendsAvailableLimit
        {
            get { return _resources.TrendsRateLimits["/trends/available"]; }
        }

        public IEndpointRateLimit TrendsClosestLimit
        {
            get { return _resources.TrendsRateLimits["/trends/closest"]; }
        }

        public IEndpointRateLimit TrendsPlaceLimit
        {
            get { return _resources.TrendsRateLimits["/trends/place"]; }
        }
        #endregion

        #region Users

        public IEndpointRateLimit UsersDerivedInfoLimit
        {
            get { return _resources.UsersRateLimits["/users/derived_info"]; }
        }

        public IEndpointRateLimit UsersLookupLimit
        {
            get { return _resources.UsersRateLimits["/users/lookup"]; }
        }

        public IEndpointRateLimit UsersProfileBannerLimit
        {
            get { return _resources.UsersRateLimits["/users/profile_banner"]; }
        }

        public IEndpointRateLimit UsersReportSpamLimit
        {
            get { return _resources.UsersRateLimits["/users/report_spam"]; }
        }

        public IEndpointRateLimit UsersSearchLimit
        {
            get { return _resources.UsersRateLimits["/users/search"]; }
        }

        public IEndpointRateLimit UsersShowIdLimit
        {
            get { return _resources.UsersRateLimits["/users/show/:id"]; }
        }

        public IEndpointRateLimit UsersSuggestionsLimit
        {
            get { return _resources.UsersRateLimits["/users/suggestions"]; }
        }

        public IEndpointRateLimit UsersSuggestionsSlugLimit
        {
            get { return _resources.UsersRateLimits["/users/suggestions/:slug"]; }
        }

        public IEndpointRateLimit UsersSuggestionsSlugMembersLimit
        {
            get { return _resources.UsersRateLimits["/users/suggestions/:slug/members"]; }
        }
        #endregion


        private class RateLimitResources
        {
            [JsonProperty("account")]
            public Dictionary<string, IEndpointRateLimit> AccountRateLimits { get; set; }

            [JsonProperty("application")]
            public Dictionary<string, IEndpointRateLimit> ApplicationRateLimits { get; set; }

            [JsonProperty("blocks")]
            public Dictionary<string, IEndpointRateLimit> BlocksRateLimits { get; set; }

            [JsonProperty("device")]
            public Dictionary<string, IEndpointRateLimit> DeviceRateLimits { get; set; }

            [JsonProperty("direct_messages")]
            public Dictionary<string, IEndpointRateLimit> DirectMessagesRateLimits { get; set; }

            [JsonProperty("favorites")]
            public Dictionary<string, IEndpointRateLimit> FavoritesRateLimits { get; set; }

            [JsonProperty("followers")]
            public Dictionary<string, IEndpointRateLimit> FollowersRateLimits { get; set; }

            [JsonProperty("friends")]
            public Dictionary<string, IEndpointRateLimit> FriendsRateLimits { get; set; }

            [JsonProperty("friendships")]
            public Dictionary<string, IEndpointRateLimit> FriendshipsRateLimits { get; set; }

            [JsonProperty("geo")]
            public Dictionary<string, IEndpointRateLimit> GeoRateLimits { get; set; }

            [JsonProperty("help")]
            public Dictionary<string, IEndpointRateLimit> HelpRateLimits { get; set; }

            [JsonProperty("lists")]
            public Dictionary<string, IEndpointRateLimit> ListsRateLimits { get; set; }

            [JsonProperty("mutes")]
            public Dictionary<string, IEndpointRateLimit> MutesRateLimits { get; set; }

            [JsonProperty("saved_searches")]
            public Dictionary<string, IEndpointRateLimit> SavedSearchesRateLimits { get; set; }

            [JsonProperty("search")]
            public Dictionary<string, IEndpointRateLimit> SearchRateLimits { get; set; }

            [JsonProperty("statuses")]
            public Dictionary<string, IEndpointRateLimit> StatusesRateLimits { get; set; }

            [JsonProperty("trends")]
            public Dictionary<string, IEndpointRateLimit> TrendsRateLimits { get; set; }

            [JsonProperty("users")]
            public Dictionary<string, IEndpointRateLimit> UsersRateLimits { get; set; }
        }

        [JsonProperty("rate_limit_context")]
        private JObject _rateLimitContext
        {
            set
            {
                JToken jsonObj;
                if (value.TryGetValue("access_token", out jsonObj))
                {
                    RateLimitContext = jsonObj.ToObject<string>();
                }
                else if (value.TryGetValue("application", out jsonObj))
                {
                    RateLimitContext = jsonObj.ToObject<string>();
                    IsApplicationOnlyCredentials = true;
                }
            }
        }

        [JsonProperty("resources")]
        private RateLimitResources _resources { get; set; }
    }
}