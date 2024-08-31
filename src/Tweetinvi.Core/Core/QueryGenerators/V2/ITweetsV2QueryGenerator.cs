using System.Text;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Core.QueryGenerators.V2
{
    public interface ITweetsV2QueryGenerator
    {
        string GetTweetQuery(IGetTweetV2Parameters parameters);
        string GetTweetsQuery(IGetTweetsV2Parameters parameters);
        string GetChangeTweetReplyVisibilityQuery(IChangeTweetReplyVisibilityV2Parameters parameters);
        void AddTweetFieldsParameters(IBaseTweetsV2Parameters parameters, StringBuilder query);
        string GetTweetQuery(IPublishTweetV2Parameters parameters);
    }
}