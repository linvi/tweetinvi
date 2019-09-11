using System.Diagnostics.CodeAnalysis;

namespace Tweetinvi
{
    public class TwitterLimits
    {
        public TwitterLimits()
        {
            Tweets = new TweetLimits();
            Users = new UserLimits();
        }

        public TwitterLimits(TwitterLimits source)
        {
            Tweets = new TweetLimits(source?.Tweets);
            Users = new UserLimits(source?.Users);
        }

        public TweetLimits Tweets { get; }
        public UserLimits Users { get; }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class TweetLimits
    {
        public short GetTweetsRequestMaxSize { get; set; } = 100;
        public short GetRetweetsRequestMaxSize { get; set; } = 100;

        public TweetLimits()
        {
        }

        public TweetLimits(TweetLimits source)
        {
            if (source == null)
            {
                return;
            }

            GetTweetsRequestMaxSize = source.GetTweetsRequestMaxSize;
            GetRetweetsRequestMaxSize = source.GetRetweetsRequestMaxSize;
        }
    }

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class UserLimits
    {
        public UserLimits()
        {
        }

        public UserLimits(UserLimits source)
        {
            if (source == null) { return; }

            GetUsersMaxSize = source.GetUsersMaxSize;
        }

        /// <summary>
        /// Maximum numbers of users that can be retrieved in 1 request
        /// https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-lookup
        /// </summary>
        public int GetUsersMaxSize { get; set; } = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
    }
}
