using System.Collections.Generic;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public static class UserResponseFields
    {
        public static class Expansions
        {
            public static HashSet<string> ALL => new HashSet<string>
            {
                PinnedTweetId
            };

            public const string PinnedTweetId = "pinned_tweet_id";
        }

        public static readonly TweetFields Tweet = new TweetFields();
        public static readonly UserFields User = new UserFields();
    }
}