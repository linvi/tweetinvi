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

        public static GetTweetV2Parameters WithNoFields(long tweetId)
        {
            var parameters = new GetTweetV2Parameters(tweetId);
            parameters.ClearAllFields();
            return parameters;
        }

        public long TweetId { get; set; }
    }
}