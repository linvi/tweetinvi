using Tweetinvi.Core.Helpers;

// ReSharper disable InvalidXmlDocComment
// ReSharper disable InconsistentNaming
namespace Tweetinvi.Controllers.Properties
{
    public static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/settings.json.
        /// </summary>
        public static string Account_GetSettings = "https://api.twitter.com/1.1/account/settings.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/mutes/users/create.json?.
        /// </summary>
        public static string Account_Mute_Create = "https://api.twitter.com/1.1/mutes/users/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/mutes/users/destroy.json?.
        /// </summary>
        public static string Account_Mute_Destroy = "https://api.twitter.com/1.1/mutes/users/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/mutes/users/ids.json?.
        /// </summary>
        public static string Account_Mute_GetUserIds = "https://api.twitter.com/1.1/mutes/users/ids.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/mutes/users/ids.json?.
        /// </summary>
        public static string Account_Mute_GetUsers = "https://api.twitter.com/1.1/mutes/users/list.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/settings.json?.
        /// </summary>
        public static string Account_UpdateSettings = "https://api.twitter.com/1.1/account/settings.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/update_profile.json.
        /// </summary>
        public static string Account_UpdateProfile = "https://api.twitter.com/1.1/account/update_profile.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/update_profile_image.json.
        /// </summary>
        public static string Account_UpdateProfileImage = "https://api.twitter.com/1.1/account/update_profile_image.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/update_profile_banner.json.
        /// </summary>
        public static string Account_UpdateProfileBanner = "https://api.twitter.com/1.1/account/update_profile_banner.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/remove_profile_banner.json.
        /// </summary>
        public static string Account_RemoveProfileBanner = "https://api.twitter.com/1.1/account/remove_profile_banner.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/create.json?{0}.
        /// </summary>
        public static string Friendship_Create = "https://api.twitter.com/1.1/friendships/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/destroy.json?{0}.
        /// </summary>
        public static string Friendship_Destroy = "https://api.twitter.com/1.1/friendships/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/no_retweets/ids.json.
        /// </summary>
        public static string Friendship_FriendIdsWithNoRetweets = "https://api.twitter.com/1.1/friendships/no_retweets/ids.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/incoming.json?.
        /// </summary>
        public static string Friendship_GetIncomingIds = "https://api.twitter.com/1.1/friendships/incoming.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/outgoing.json?.
        /// </summary>
        public static string Friendship_GetOutgoingIds = "https://api.twitter.com/1.1/friendships/outgoing.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/show.json?.
        /// </summary>
        public static string Friendship_GetRelationship = "https://api.twitter.com/1.1/friendships/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/lookup.json?.
        /// </summary>
        public static string Friendship_GetRelationships = "https://api.twitter.com/1.1/friendships/lookup.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friendships/update.json?.
        /// </summary>
        public static string Friendship_Update = "https://api.twitter.com/1.1/friendships/update.json?";

        /// <summary>
        ///   Looks up a localized string similar to long={0}&amp;lat={1}.
        /// </summary>
        public static string Geo_CoordinatesParameter = "long={0}&lat={1}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/geo/id/{0}.json.
        /// </summary>
        public static string Geo_GetPlaceFromId = "https://api.twitter.com/1.1/geo/id/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/geo/search.json.
        /// </summary>
        public static string Geo_SearchGeo = "https://api.twitter.com/1.1/geo/search.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/geo/reverse_geocode.json.
        /// </summary>
        public static string Geo_SearchGeoReverse = "https://api.twitter.com/1.1/geo/reverse_geocode.json";

        /// <summary>
        ///   Looks up a localized string similar to place_id={0}.
        /// </summary>
        public static string Geo_PlaceIdParameter = "place_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/application/rate_limit_status.json.
        /// </summary>
        public static string Help_GetRateLimit = "https://api.twitter.com/1.1/application/rate_limit_status.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/help/configuration.json.
        /// </summary>
        public static string Help_GetTwitterConfiguration = "https://api.twitter.com/1.1/help/configuration.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/help/languages.json.
        /// </summary>
        public static string Help_GetSupportedLanguages = "https://api.twitter.com/1.1/help/languages.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/create.json?.
        /// </summary>
        public static string List_Create = "https://api.twitter.com/1.1/lists/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/show.json?.
        /// </summary>
        public static string List_Get = "https://api.twitter.com/1.1/lists/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/list.json?.
        /// </summary>
        public static string List_GetUserLists = "https://api.twitter.com/1.1/lists/list.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/update.json?.
        /// </summary>
        public static string List_Update = "https://api.twitter.com/1.1/lists/update.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/destroy.json?.
        /// </summary>
        public static string List_Destroy = "https://api.twitter.com/1.1/lists/destroy.json?";


        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members/create.json?.
        /// </summary>
        public static string List_Members_Create = "https://api.twitter.com/1.1/lists/members/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members.json?.
        /// </summary>
        public static string List_Members_List = "https://api.twitter.com/1.1/lists/members.json?";


        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members/show.json?.
        /// </summary>
        public static string List_CheckMembership = "https://api.twitter.com/1.1/lists/members/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members/create_all.json?.
        /// </summary>
        public static string List_CreateMembers = "https://api.twitter.com/1.1/lists/members/create_all.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members/destroy.json?.
        /// </summary>
        public static string List_DestroyMember = "https://api.twitter.com/1.1/lists/members/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/members/destroy_all.json?.
        /// </summary>
        public static string List_DestroyMembers = "https://api.twitter.com/1.1/lists/members/destroy_all.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/subscribers.json?.
        /// </summary>
        public static string List_GetSubscribers = "https://api.twitter.com/1.1/lists/subscribers.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/statuses.json?.
        /// </summary>
        public static string List_GetTweetsFromList = "https://api.twitter.com/1.1/lists/statuses.json?";

        /// <summary>
        ///   Looks up a localized string similar to &amp;owner_id={0}.
        /// </summary>
        public static string List_OwnerIdParameter = "&owner_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;owner_screen_name={0}.
        /// </summary>
        public static string List_OwnerScreenNameParameter = "&owner_screen_name={0}";

        /// <summary>
        /// Looks up a localized string similar to https://api.twitter.com/1.1/lists/memberships.json?.
        /// </summary>
        public static string List_GetUserMemberships = "https://api.twitter.com/1.1/lists/memberships.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/ownerships.json?.
        /// </summary>
        public static string List_OwnedByUser = "https://api.twitter.com/1.1/lists/ownerships.json?";

        /// <summary>
        ///   Looks up a localized string similar to &amp;slug={0}.
        /// </summary>
        public static string List_SlugParameter = "&slug={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/subscribers/create.json?.
        /// </summary>
        public static string List_Subscribe = "https://api.twitter.com/1.1/lists/subscribers/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/subscribers/destroy.json?.
        /// </summary>
        public static string List_Unsubscribe = "https://api.twitter.com/1.1/lists/subscribers/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/subscriptions.json?.
        /// </summary>
        public static string List_UserSubscriptions = "https://api.twitter.com/1.1/lists/subscriptions.json?";

        /// <summary>
        ///   Looks up a localized string similar tohttps://api.twitter.com/1.1/lists/subscribers/show.json.
        /// </summary>
        public static string List_CheckSubscriber = "https://api.twitter.com/1.1/lists/subscribers/show.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/direct_messages/events/list.json.
        /// </summary>
        public static string Message_GetMessages = "https://api.twitter.com/1.1/direct_messages/events/list.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/direct_messages/events/new.json.
        /// </summary>
        public static string Message_Create = "https://api.twitter.com/1.1/direct_messages/events/new.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/direct_messages/show.json?.
        /// </summary>
        public static string Message_Get = "https://api.twitter.com/1.1/direct_messages/events/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/direct_messages/events/destroy.json?.
        /// </summary>
        public static string Message_Destroy = "https://api.twitter.com/1.1/direct_messages/events/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to &amp;count={0}.
        /// </summary>
        public static string QueryParameter_Count = "&count={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;include_entities={0}.
        /// </summary>
        public static string QueryParameter_IncludeEntities = "&include_entities={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;include_rts={0}.
        /// </summary>
        public static string QueryParameter_IncludeRetweets = "&include_rts={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;max_id={0}.
        /// </summary>
        public static string QueryParameter_MaxId = "&max_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;page_number={0}.
        /// </summary>
        public static string QueryParameter_PageNumber = "&page_number={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;since_id={0}.
        /// </summary>
        public static string QueryParameter_SinceId = "&since_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;skip_status={0}.
        /// </summary>
        public static string QueryParameter_SkipStatus = "&skip_status={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;trim_user={0}.
        /// </summary>
        public static string QueryParameter_TrimUser = "&trim_user={0}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;cursor={0}.
        /// </summary>
        public static string QueryParameter_Cursor = "&cursor={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/destroy/{0}.json.
        /// </summary>
        public static string SavedSearch_Destroy = "https://api.twitter.com/1.1/saved_searches/destroy/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/list.json.
        /// </summary>
        public static string SavedSearches_GetList = "https://api.twitter.com/1.1/saved_searches/list.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/search/tweets.json.
        /// </summary>
        public static string Search_SearchTweets = "https://api.twitter.com/1.1/search/tweets.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/search.json.
        /// </summary>
        public static string Search_SearchUsers = "https://api.twitter.com/1.1/users/search.json";

        /// <summary>
        ///   Looks up a localized string similar to {0},{1},{2}{3}.
        /// </summary>
        public static string SearchParameter_GeoCode = "{0},{1},{2}{3}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/home_timeline.json?.
        /// </summary>
        public static string Timeline_GetHomeTimeline = "https://api.twitter.com/1.1/statuses/home_timeline.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/mentions_timeline.json?.
        /// </summary>
        public static string Timeline_GetMentionsTimeline = "https://api.twitter.com/1.1/statuses/mentions_timeline.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/retweets_of_me.json?.
        /// </summary>
        public static string Timeline_GetRetweetsOfMeTimeline = "https://api.twitter.com/1.1/statuses/retweets_of_me.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/user_timeline.json?.
        /// </summary>
        public static string Timeline_GetUserTimeline = "https://api.twitter.com/1.1/statuses/user_timeline.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/trends/place.json?id={0}.
        /// </summary>
        public static string Trends_GetTrendsFromWoeId = "https://api.twitter.com/1.1/trends/place.json?id={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/trends/available.json.
        /// </summary>
        public static string Trends_GetAvailableTrendsLocations = "https://api.twitter.com/1.1/trends/available.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/trends/closest.json.
        /// </summary>
        public static string Trends_GetClosestTrendsLocations = "https://api.twitter.com/1.1/trends/closest.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/destroy/{0}.json.
        /// </summary>
        public static string Tweet_Destroy = "https://api.twitter.com/1.1/statuses/destroy/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/favorites/create.json?.
        /// </summary>
        public static string Tweet_Favorite_Create = "https://api.twitter.com/1.1/favorites/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/favorites/destroy.json?.
        /// </summary>
        public static string Tweet_Favorite_Destroy = "https://api.twitter.com/1.1/favorites/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/oembed.json?.
        /// </summary>
        public static string Tweet_GenerateOEmbed = "https://api.twitter.com/1.1/statuses/oembed.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/show.json?.
        /// </summary>
        public static string Tweet_Get = "https://api.twitter.com/1.1/statuses/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/retweeters/ids.json.
        /// </summary>
        public static string Tweet_GetRetweeters = "https://api.twitter.com/1.1/statuses/retweeters/ids.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/lookup.json?.
        /// </summary>
        public static string Tweet_Lookup = "https://api.twitter.com/1.1/statuses/lookup.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/update.json?.
        /// </summary>
        public static string Tweet_Publish = "https://api.twitter.com/1.1/statuses/update.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/retweets/{0}.json.
        /// </summary>
        public static string Tweet_Retweet_GetRetweets = "https://api.twitter.com/1.1/statuses/retweets/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/unretweet/{0}.json.
        /// </summary>
        public static string Tweet_DestroyRetweet = "https://api.twitter.com/1.1/statuses/unretweet/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/statuses/retweet/{0}.json.
        /// </summary>
        public static string Tweet_Retweet_Publish = "https://api.twitter.com/1.1/statuses/retweet/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://upload.twitter.com/1.1/media/upload.json.
        /// </summary>
        public static string Upload_URL = "https://upload.twitter.com/1.1/media/upload.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account/verify_credentials.json.
        /// </summary>
        public static string User_GetCurrentUser = "https://api.twitter.com/1.1/account/verify_credentials.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/blocks/create.json?.
        /// </summary>
        public static string User_Block_Create = "https://api.twitter.com/1.1/blocks/create.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/blocks/destroy.json?.
        /// </summary>
        public static string User_Block_Destroy = "https://api.twitter.com/1.1/blocks/destroy.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/blocks/list.json?.
        /// </summary>
        public static string User_Block_List = "https://api.twitter.com/1.1/blocks/list.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/blocks/ids.json?.
        /// </summary>
        public static string User_Block_List_Ids = "https://api.twitter.com/1.1/blocks/ids.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/favorites/list.json?{0}&amp;count={1}.
        /// </summary>
        public static string User_GetFavourites = "https://api.twitter.com/1.1/favorites/list.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/followers/ids.json?.
        /// </summary>
        public static string User_GetFollowers = "https://api.twitter.com/1.1/followers/ids.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/friends/ids.json?.
        /// </summary>
        public static string User_GetFriends = "https://api.twitter.com/1.1/friends/ids.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/show.json?.
        /// </summary>
        public static string User_GetUser = "https://api.twitter.com/1.1/users/show.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/lookup.json?.
        /// </summary>
        public static string User_GetUsers = "https://api.twitter.com/1.1/users/lookup.json?";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/report_spam.json?.
        /// </summary>
        public static string User_Report_Spam = "https://api.twitter.com/1.1/users/report_spam.json?";

        /// <summary>
        ///   Looks up a localized string similar to Upload STATUS can only be retrieved for uploaded media. The FINALIZE query must be invoked.
        /// </summary>
        public static string Exception_Upload_Status_NotUploaded = "Upload STATUS can only be retrieved for uploaded media. The FINALIZE query must be invoked.";

        /// <summary>
        ///   Looks up a localized string similar to Upload STATUS can only be invoked on uploads with processing metadata. Set the `media_category` to `tweet_video` to solve this issue.
        /// </summary>
        public static string Exception_Upload_Status_No_ProcessingInfo = "Upload STATUS can only be invoked on uploads with processing metadata. Set the `media_category` to `tweet_video` to solve this issue.";

        /// <summary>
        ///   Looks up a localized string similar to oob.
        /// </summary>
        public static string Auth_PinCodeUrl = "oob";

        /// <summary>
        ///   Looks up a localized string similar to authorization_id.
        /// </summary>
        public static string Auth_ProcessIdKey = "authorization_id";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/request_token.
        /// </summary>
        public static string Auth_CreateBearerToken = "https://api.twitter.com/oauth2/token";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/request_token.
        /// </summary>
        public static string Auth_RequestToken = "https://api.twitter.com/oauth/request_token";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/access_token.
        /// </summary>
        public static string Auth_RequestAccessToken = "https://api.twitter.com/oauth/access_token";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/authorize.
        /// </summary>
        public static string Auth_AuthorizeBaseUrl = "https://api.twitter.com/oauth/authorize";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth2/invalidate_token.
        /// </summary>
        public static string Auth_InvalidateBearerToken = "https://api.twitter.com/oauth2/invalidate_token";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/oauth/invalidate_token.
        /// </summary>
        public static string Auth_InvalidateAccessToken = "https://api.twitter.com/1.1/oauth/invalidate_token";

        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&oauth_callback_confirmed=(?<oauth_callback_confirmed>(?:\\w)*).
        /// </summary>
        public static string Auth_RequestTokenParserRegex = "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&oauth_callback_confirmed=(?<oauth_callback_confirmed>(?:\\w)*)";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/account_activity/all.
        /// </summary>
        public static string Webhooks_AccountActivity_All = "https://api.twitter.com/1.1/account_activity/all";

        /// <summary>
        ///   Looks up a localized string similar to /webhooks.json?.
        /// </summary>
        public static string Webhooks_AccountActivity_GetAllWebhooks = "https://api.twitter.com/1.1/account_activity/all/webhooks.json";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}