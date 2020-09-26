using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetsV2QueryGenerator
    {
        string GetTweetQuery(IGetTweetV2Parameters parameters);
        string GetTweetsQuery(IGetTweetsV2Parameters parameters);
    }

    public class TweetsV2QueryGenerator : ITweetsV2QueryGenerator
    {
        public string GetTweetQuery(IGetTweetV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.TweetV2_Get}/{parameters.TweetId}");
            AddTweetFieldsParameters(parameters, query);
            return query.ToString();
        }

        public string GetTweetsQuery(IGetTweetsV2Parameters parameters)
        {
            var tweetIds = string.Join(",", parameters.TweetIds);
            var query = new StringBuilder($"{Resources.TweetV2_Get}");
            query.AddParameterToQuery("ids", tweetIds);
            AddTweetFieldsParameters(parameters, query);
            return query.ToString();
        }

        private static void AddTweetFieldsParameters(IBaseTweetsV2Parameters parameters, StringBuilder query)
        {
            query.AddParameterToQuery("expansions", parameters.Expansions);
            query.AddParameterToQuery("media.fields", parameters.MediaFields);
            query.AddParameterToQuery("place.fields", parameters.PlaceFields);
            query.AddParameterToQuery("poll.fields", parameters.PollFields);
            query.AddParameterToQuery("tweet.fields", parameters.TweetFields);
            query.AddParameterToQuery("user.fields", parameters.UserFields);
        }
    }
}