using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetsV2QueryGenerator : ITweetsV2QueryGenerator
    {
        public string GetTweetQuery(IGetTweetV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.TweetV2_Get}/{parameters.TweetId}");
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetTweetsQuery(IGetTweetsV2Parameters parameters)
        {
            var tweetIds = string.Join(",", parameters.TweetIds);
            var query = new StringBuilder($"{Resources.TweetV2_Get}");
            query.AddParameterToQuery("ids", tweetIds);
            AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetChangeTweetReplyVisibilityQuery(IChangeTweetReplyVisibilityV2Parameters parameters)
        {
            var query = new StringBuilder($"https://api.twitter.com/2/tweets/{parameters.Id}/hidden");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public void AddTweetFieldsParameters(IBaseTweetsV2Parameters parameters, StringBuilder query)
        {
            query.AddParameterToQuery("expansions", parameters.Expansions);
            query.AddParameterToQuery("media.fields", parameters.MediaFields);
            query.AddParameterToQuery("place.fields", parameters.PlaceFields);
            query.AddParameterToQuery("poll.fields", parameters.PollFields);
            query.AddParameterToQuery("tweet.fields", parameters.TweetFields);
            query.AddParameterToQuery("user.fields", parameters.UserFields);
        }

        /// <summary>
        ///     Publish a tweet V2
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string GetTweetQuery(IPublishTweetV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.TweetV2_Get}");
            return query.ToString();
        }
    }
}