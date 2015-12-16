using System;

namespace Tweetinvi.Core
{
    public static class TweetinviConsts
    {
        public const int MAX_TWEET_SIZE = 140;
        public const int MEDIA_CONTENT_SIZE = 24;


        public const int STATUS_CODE_TOO_MANY_REQUEST = 429;

        // https://dev.twitter.com/rest/reference/get/direct_messages
        public const int MESSAGE_GET_COUNT = 200;

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
        public const int TWITTER_LIST_GET_TWEETS_COUNT = 100;

        // https://dev.twitter.com/rest/reference/get/lists/subscriptions
        public const int TWITTER_LIST_GET_USER_SUBSCRIPTIONS_COUNT = 1000;

        // 
        public const int TWITTER_LIST_OWNED_COUNT = 1000;

        // https://dev.twitter.com/rest/reference/post/lists/members/create_all
        // https://dev.twitter.com/rest/reference/post/lists/members/destroy_all
        public const int TWITTER_LIST_ADD_OR_REMOVE_MULTIPLE_MEMBERS_MAX = 100;

        // https://dev.twitter.com/rest/reference/get/friendships/lookup
        public const int FRIENDSHIP_MAX_NUMBER_OF_FRIENDSHIP_TO_GET_IN_A_SINGLE_QUERY = 100;
    }
}