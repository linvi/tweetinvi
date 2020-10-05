using System.Linq;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetTweetsV2Parameters : IBaseTweetsV2Parameters
    {
        public string[] TweetIds { get; set; }
    }

    public class GetTweetsV2Parameters : BaseTweetsV2Parameters, IGetTweetsV2Parameters
    {
        public GetTweetsV2Parameters(params long[] tweetIds)
        {
            TweetIds = tweetIds.Select(x => x.ToString()).ToArray();
            this.WithAllFields();
        }

        public GetTweetsV2Parameters(params string[] tweetIds)
        {
            TweetIds = tweetIds;
            this.WithAllFields();
        }

        public string[] TweetIds { get; set; }
    }
}