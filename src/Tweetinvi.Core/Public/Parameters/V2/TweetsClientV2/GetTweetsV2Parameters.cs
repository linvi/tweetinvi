using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetTweetsV2Parameters : IBaseTweetsV2Parameters
    {
        public long[] TweetIds { get; set; }
    }

    public class GetTweetsV2Parameters : BaseTweetsV2Parameters, IGetTweetsV2Parameters
    {
        public GetTweetsV2Parameters(params long[] tweetIds)
        {
            TweetIds = tweetIds;
        }

        public long[] TweetIds { get; set; }
    }
}