using Tweetinvi.Models.V2;
using Tweetinvi.Models.V2.Responses;

namespace Tweetinvi.Events.V2
{
    public class TweetV2EventArgs
    {
        public TweetV2EventArgs(TweetResponseDTO response, string json) : this(response.data, response.includes, json)
        {
        }

        public TweetV2EventArgs(TweetDTO tweet, TweetIncludesDTO includes, string json)
        {
            Tweet = tweet;
            Includes = includes;
            Json = json;
        }

        public TweetDTO Tweet { get; }
        public TweetIncludesDTO Includes { get; }
        public string Json { get; }
    }

    /// <summary>
    /// Event informing that a tweet was received by a stream
    /// </summary>
    public class TweetV2ReceivedEventArgs : TweetV2EventArgs
    {
        public TweetV2ReceivedEventArgs(TweetResponseDTO response, string json) : base(response, json)
        {
        }
    }
}