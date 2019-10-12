// ReSharper disable InconsistentNaming

namespace Tweetinvi
{
    public class TwitterLimits
    {
        public static TwitterLimits DEFAULTS { get; } = new TwitterLimits();

        /// <summary>
        /// Maximum numbers of users friendship that can retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-lookup</para>
        /// </summary>
        public short ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum numbers of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// </summary>
        public short ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing</para>
        /// </summary>
        public short ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of user ids that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids</para>
        /// </summary>
        public short ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list</para>
        /// </summary>
        public short ACCOUNT_GET_MUTED_USERS_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Max length of the profile's name
        /// <para>https://twitter.com/settings/profile</para>
        /// </summary>
        public short ACCOUNT_SETTINGS_PROFILE_NAME_MAX_LENGTH { get; set; } = 50;

        /// <summary>
        /// Max length of the profile's bio/description
        /// <para>https://twitter.com/settings/profile</para>
        /// </summary>
        public short ACCOUNT_SETTINGS_PROFILE_DESCRIPTION_MAX_LENGTH { get; set; } = 160;

        /// <summary>
        /// Max length of the profile's location
        /// <para>https://twitter.com/settings/profile</para>
        /// </summary>
        public short ACCOUNT_SETTINGS_PROFILE_LOCATION_MAX_LENGTH { get; set; } = 30;

        /// <summary>
        /// Max length of the profile's website url
        /// <para>https://twitter.com/settings/profile</para>
        /// </summary>
        public short ACCOUNT_SETTINGS_PROFILE_WEBSITE_URL_MAX_LENGTH { get; set; } = 100;

        /// <summary>
        /// Maximum numbers of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of tweets to retrieve in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me</para>
        /// </summary>
        public short TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE { get; set; } = 100;
        
        /// <summary>
        /// Maximum numbers of favorites that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list</para>
        /// </summary>
        public short TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE { get; set; } = 200;
        
        /// <summary>
        /// Maximum numbers of retweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id</para>
        /// </summary>
        public short TWEETS_GET_RETWEETS_MAX_SIZE { get; set; } = 100;
        
        /// <summary>
        /// Maximum numbers of retweeter ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids</para>
        /// </summary>
        public short TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE { get; set; } = 100;
        
        /// <summary>
        /// Maximum numbers of tweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup</para>
        /// </summary>
        public short TWEETS_GET_TWEETS_REQUEST_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum numbers of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup</para>
        /// </summary>
        public short USERS_GET_USERS_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum numbers of follower ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids</para>
        /// </summary>
        public short USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum numbers of friend ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids</para>
        /// </summary>
        public short USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE { get; set; } = 5000;

        public TwitterLimits()
        {
        }

        public TwitterLimits(TwitterLimits source)
        {
            ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE = source.ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE;
            ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE = source.ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE;
            ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE = source.ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE;
            ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE = source.ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE;
            ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE = source.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE;
            ACCOUNT_GET_MUTED_USERS_MAX_PAGE_SIZE = source.ACCOUNT_GET_MUTED_USERS_MAX_PAGE_SIZE;
            ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE = source.ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE;

            ACCOUNT_SETTINGS_PROFILE_NAME_MAX_LENGTH = source.ACCOUNT_SETTINGS_PROFILE_NAME_MAX_LENGTH;
            ACCOUNT_SETTINGS_PROFILE_LOCATION_MAX_LENGTH = source.ACCOUNT_SETTINGS_PROFILE_LOCATION_MAX_LENGTH;
            ACCOUNT_SETTINGS_PROFILE_WEBSITE_URL_MAX_LENGTH = source.ACCOUNT_SETTINGS_PROFILE_WEBSITE_URL_MAX_LENGTH;
            ACCOUNT_SETTINGS_PROFILE_DESCRIPTION_MAX_LENGTH = source.ACCOUNT_SETTINGS_PROFILE_DESCRIPTION_MAX_LENGTH;

            TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE = source.TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE;
            
            TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE = source.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE;
            TWEETS_GET_RETWEETS_MAX_SIZE = source.TWEETS_GET_RETWEETS_MAX_SIZE;
            TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE = source.TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE;
            TWEETS_GET_TWEETS_REQUEST_MAX_SIZE = source.TWEETS_GET_TWEETS_REQUEST_MAX_SIZE;

            USERS_GET_USERS_MAX_SIZE = source.USERS_GET_USERS_MAX_SIZE;
            USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE = source.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
            USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE = source.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
        }
    }
}