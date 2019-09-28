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
        /// Maximum numbers of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// </summary>
        public short ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE { get; set; } = 5000;
        
        /// <summary>
        /// Maximum numbers of user ids that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_IDS_MAX_PAGE_SIZE { get; set; } = 5000;
        
        /// <summary>
        /// Maximum numbers of users that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-list</para>
        /// </summary>
        public short ACCOUNT_GET_BLOCKED_USER_MAX_PAGE_SIZE { get; set; } = 5000;
        
        /// <summary>
        /// Maximum numbers of tweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-lookup</para>
        /// </summary>
        public short TWEETS_GET_TWEETS_REQUEST_MAX_SIZE { get; set; } = 100;
        
        /// <summary>
        /// Maximum numbers of retweets that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-statuses-retweets-id</para>
        /// </summary>
        public short TWEETS_GET_RETWEETS_MAX_SIZE { get; set; } = 100;
        
        /// <summary>
        /// Maximum numbers of favorites that can be retrieved in 1 request
        /// <para>https://dev.twitter.com/en/docs/tweets/post-and-engage/api-reference/get-favorites-list</para>
        /// </summary>
        public short TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE { get; set; } = 200;

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
            ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE = source.ACCOUNT_GET_USER_IDS_REQUESTING_FRIENDSHIP_MAX_PAGE_SIZE;
            
            TWEETS_GET_RETWEETS_MAX_SIZE = source.TWEETS_GET_RETWEETS_MAX_SIZE;
            TWEETS_GET_TWEETS_REQUEST_MAX_SIZE = source.TWEETS_GET_TWEETS_REQUEST_MAX_SIZE;
            
            USERS_GET_USERS_MAX_SIZE = source.USERS_GET_USERS_MAX_SIZE;
            USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE = source.USERS_GET_FOLLOWER_IDS_PAGE_MAX_SIZE;
            USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE = source.USERS_GET_FRIEND_IDS_PAGE_MAX_SIZE;
        }
    }
}
