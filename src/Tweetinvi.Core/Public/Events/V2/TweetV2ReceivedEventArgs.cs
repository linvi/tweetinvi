using Tweetinvi.Models;
using Tweetinvi.Models.Responses;

namespace Tweetinvi.Events.V2
{
    public class TweetV2EventArgs
    {
        public TweetV2EventArgs(TweetV2Response response, string json) : this(response.Tweet, response.Includes, json)
        {
        }

        public TweetV2EventArgs(TweetV2 tweet, TweetIncludesV2 includes, string json)
        {
            Tweet = tweet;
            Includes = includes;
            Json = json;
        }

        public TweetV2 Tweet { get; }
        public TweetIncludesV2 Includes { get; }
        public string Json { get; }
    }

    /// <summary>
    /// Event informing that a tweet was received by a stream
    /// </summary>
    public class TweetV2ReceivedEventArgs : TweetV2EventArgs
    {
        public TweetV2ReceivedEventArgs(TweetV2Response response, string json) : base(response, json)
        {
        }
    }
}