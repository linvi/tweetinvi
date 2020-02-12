// ReSharper disable InconsistentNaming

namespace Tweetinvi
{
    public class TwitterLimits
    {
        public static TwitterLimits DEFAULTS { get; } = new TwitterLimits();

        /// <summary>
        /// Maximum number of users friendship that can retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-lookup</para>
        /// </summary>
        public short ACCOUNT_GET_RELATIONSHIPS_WITH_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// </summary>
        public short ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing</para>
        /// </summary>
        public short ACCOUNT_GET_REQUESTED_USER_IDS_TO_FOLLOW_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of user ids that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-mutes-users-ids</para>
        /// </summary>
        public short ACCOUNT_GET_MUTED_USER_IDS_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of users that can be retrieved in 1 request
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
        /// Maximum number of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list</para>
        /// </summary>
        public short LISTS_CREATE_NAME_MAX_SIZE { get; set; } = 25;

        /// <summary>
        /// Maximum number of lists that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-ownerships</para>
        /// </summary>
        public short LISTS_GET_USER_OWNED_LISTS_MAX_SIZE { get; set; } = 1000;

        /// <summary>
        /// Maximum number of tweets that can be retrieved from a list in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-statuses</para>
        /// </summary>
        public short LISTS_GET_TWEETS_MAX_PAGE_SIZE { get; set; } = 200;

        /// <summary>
        /// Maximum number of lists that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships</para>
        /// </summary>
        public short LISTS_GET_USER_MEMBERSHIPS_MAX_SIZE { get; set; } = 1000;

        /// <summary>
        /// Maximum number of lists that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-memberships</para>
        /// </summary>
        public short LISTS_GET_USER_MEMBERSHIPS_LISTS_MAX_SIZE { get; set; } = 1000;

        /// <summary>
        /// Maximum number of users that can be added to a list in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-create_all</para>
        /// </summary>
        public short LISTS_ADD_MEMBERS_MAX_USERS { get; set; } = 100;

        /// <summary>
        /// Maximum number of users that can be removed from a list in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/post-lists-members-destroy_all</para>
        /// </summary>
        public short LISTS_REMOVE_MEMBERS_MAX_USERS { get; set; } = 100;

        /// <summary>
        /// Maximum number of users that can be retrieved in 1 request
        /// <para>https://developer.twitter.com/en/docs/accounts-and-users/create-manage-lists/api-reference/get-lists-members</para>
        /// </summary>
        public short LISTS_GET_MEMBERS_MAX_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of tweets to retrieve in 1 request
        /// <para>https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-home_timeline</para>
        /// </summary>
        public short TIMELINE_HOME_PAGE_MAX_PAGE_SIZE { get; set; } = 200;

        /// <summary>
        /// Maximum number of tweets to retrieve in 1 request
        /// <para>https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-mentions_timeline</para>
        /// </summary>
        public short TIMELINE_MENTIONS_PAGE_MAX_PAGE_SIZE { get; set; } = 200;

        /// <summary>
        /// Maximum number of tweets to retrieve in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets_of_me</para>
        /// </summary>
        public short TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of tweets to retrieve in 1 request
        /// <para>https://developer.twitter.com/en/docs/tweets/timelines/api-reference/get-statuses-user_timeline</para>
        /// </summary>
        public short TIMELINE_USER_PAGE_MAX_PAGE_SIZE { get; set; } = 200;

        /// <summary>
        /// Maximum number of favorites that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list</para>
        /// </summary>
        public short TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE { get; set; } = 200;

        /// <summary>
        /// Maximum number of retweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id</para>
        /// </summary>
        public short TWEETS_GET_RETWEETS_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of retweeter ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweeters-ids</para>
        /// </summary>
        public short TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of tweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup</para>
        /// </summary>
        public short TWEETS_GET_TWEETS_REQUEST_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup</para>
        /// </summary>
        public short USERS_GET_USERS_MAX_SIZE { get; set; } = 100;

        /// <summary>
        /// Maximum number of follower ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-followers-ids</para>
        /// </summary>
        public short USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE { get; set; } = 5000;

        /// <summary>
        /// Maximum number of friend ids that can be retrieved in 1 request
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

            LISTS_CREATE_NAME_MAX_SIZE = source.LISTS_CREATE_NAME_MAX_SIZE;
            LISTS_GET_MEMBERS_MAX_SIZE = source.LISTS_GET_MEMBERS_MAX_SIZE;
            LISTS_GET_USER_OWNED_LISTS_MAX_SIZE = source.LISTS_GET_USER_OWNED_LISTS_MAX_SIZE;
            LISTS_GET_TWEETS_MAX_PAGE_SIZE = source.LISTS_GET_TWEETS_MAX_PAGE_SIZE;
            LISTS_GET_USER_MEMBERSHIPS_MAX_SIZE = source.LISTS_GET_USER_MEMBERSHIPS_MAX_SIZE;
            LISTS_GET_USER_MEMBERSHIPS_LISTS_MAX_SIZE = source.LISTS_GET_USER_MEMBERSHIPS_LISTS_MAX_SIZE;
            LISTS_ADD_MEMBERS_MAX_USERS = source.LISTS_ADD_MEMBERS_MAX_USERS;
            LISTS_REMOVE_MEMBERS_MAX_USERS = source.LISTS_REMOVE_MEMBERS_MAX_USERS;

            TIMELINE_HOME_PAGE_MAX_PAGE_SIZE = source.TIMELINE_HOME_PAGE_MAX_PAGE_SIZE;
            TIMELINE_MENTIONS_PAGE_MAX_PAGE_SIZE = source.TIMELINE_MENTIONS_PAGE_MAX_PAGE_SIZE;
            TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE = source.TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE;
            TIMELINE_USER_PAGE_MAX_PAGE_SIZE = source.TIMELINE_USER_PAGE_MAX_PAGE_SIZE;

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