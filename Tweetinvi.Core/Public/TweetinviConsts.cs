namespace Tweetinvi
{
    /// <summary>
    /// Application wide constants.
    /// </summary>
    public static class TweetinviConsts
    {
        // https://dev.twitter.com/rest/reference/get/help/configuration
        public static int MAX_TWEET_SIZE = 280;
        public static int MEDIA_CONTENT_SIZE = 24;
        public static int HTTP_LINK_SIZE = 23;
        public static int HTTPS_LINK_SIZE = 23;

        public const int STATUS_CODE_TOO_MANY_REQUEST = 429;

        // https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events
        public const int MESSAGE_GET_COUNT = 50;

        // https://dev.twitter.com/rest/reference/get/statuses/retweets_of_me
        public const int TIMELINE_RETWEETS_OF_ME_COUNT = 100;

        // https://dev.twitter.com/rest/reference/get/statuses/home_timeline
        public const int TIMELINE_HOME_COUNT = 200;

        // https://dev.twitter.com/rest/reference/get/statuses/mentions_timeline
        public const int TIMELINE_MENTIONS_COUNT = 200;

        // https://dev.twitter.com/rest/reference/get/statuses/user_timeline
        public const int TIMELINE_USER_COUNT = 200;

        // https://dev.twitter.com/rest/reference/get/search/tweets
        public const int SEARCH_TWEETS_COUNT = 100;

        // https://dev.twitter.com/rest/reference/get/users/search
        public const int SEARCH_USERS_COUNT = 20;

        // https://dev.twitter.com/rest/reference/get/lists/statuses
        public const int LIST_GET_TWEETS_COUNT = 100;

        // https://dev.twitter.com/rest/reference/get/lists/subscriptions
        public const int LIST_GET_USER_SUBSCRIPTIONS_COUNT = 1000;

        // 
        public const int LIST_OWNED_COUNT = 1000;

        // https://dev.twitter.com/rest/reference/post/lists/members/create_all
        // https://dev.twitter.com/rest/reference/post/lists/members/destroy_all
        public const int LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX = 100;

        // https://dev.twitter.com/rest/reference/get/friendships/lookup
        public const int FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY = 100;

        // https://dev.twitter.com/rest/reference/post/media/upload
        // https://dev.twitter.com/rest/public/uploading-media
        public static int UPLOAD_MAX_IMAGE_SIZE = 5 * 1024 * 1024;
        public static int UPLOAD_MAX_VIDEO_SIZE = 15 * 1024 * 1024;
        public static int UPLOAD_MAX_CHUNK_SIZE = 4 * 1024 * 1024;

        // Update Account
        public static int UPDATE_ACCOUNT_MAX_NAME_SIZE = 20;
        public static int UPDATE_ACCOUNT_MAX_URL_SIZE = 100;
        public static int UPDATE_ACCOUNT_MAX_LOCATION_SIZE = 30;
        public static int UPDATE_ACCOUNT_MAX_DESCRIPTION_SIZE = 160;

        // https://developer.twitter.com/en/docs/direct-messages/quick-replies/api-reference/options
        public const int MESSAGE_QUICK_REPLY_MAX_OPTIONS = 20;
        public const int MESSAGE_QUICK_REPLY_LABEL_MAX_LENGTH = 36;
        public const int MESSAGE_QUICK_REPLY_DESCRIPTION_MAX_LENGTH = 72;
        public const int MESSAGE_QUICK_REPLY_METADATA_MAX_LENGTH = 1000;

        // https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup
        public const int USERS_LOOKUP_MAX_PER_REQ = 100;

        // https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friends-ids.html
        public const int FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ = 5000;

        // https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-outgoing
        public const int FRIENDSHIPS_OUTGOING_IDS_MAX_PER_REQ = 5000;

        // Request to get User objects for incoming/outgoing friendship requests requires a request to get the IDs,
        //  and then a request to get the User objects for those IDs.
        // Min: USERS_LOOKUP_MAX_PER_REQ, FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ
        public const int FRIENDSHIPS_INCOMING_USERS_MAX_PER_REQ = USERS_LOOKUP_MAX_PER_REQ;
        // Min: USERS_LOOKUP_MAX_PER_REQ, FRIENDSHIPS_OUTGOING_IDS_MAX_PER_REQ
        public const int FRIENDSHIPS_OUTGOING_USERS_MAX_PER_REQ = USERS_LOOKUP_MAX_PER_REQ;
    }
}