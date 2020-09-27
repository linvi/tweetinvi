using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetTweetV2Parameters : IBaseTweetsV2Parameters
    {
        public long TweetId { get; set; }
    }

    public class GetTweetV2Parameters : BaseTweetsV2Parameters, IGetTweetV2Parameters
    {
        public GetTweetV2Parameters(long tweetId)
        {
            TweetId = tweetId;
            this.WithAllFields();
        }

        public long TweetId { get; set; }
    }
}