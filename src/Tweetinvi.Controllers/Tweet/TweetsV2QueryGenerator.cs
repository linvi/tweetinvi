using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Tweet
{
    public interface ITweetsV2QueryGenerator
    {
        string GetTweetQuery(IGetTweetV2Parameters parameters);
    }

    public class TweetsV2QueryGenerator : ITweetsV2QueryGenerator
    {
        public string GetTweetQuery(IGetTweetV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.TweetV2_Get}/{parameters.TweetId}");
            query.AddParameterToQuery("expansions", parameters.Expansions);
            query.AddParameterToQuery("media.fields", parameters.MediaFields);
            query.AddParameterToQuery("place.fields", parameters.PlaceFields);
            query.AddParameterToQuery("poll.fields", parameters.PollFields);
            query.AddParameterToQuery("tweet.fields", parameters.TweetFields);
            query.AddParameterToQuery("user.fields", parameters.UserFields);
            return query.ToString();
        }
    }
}