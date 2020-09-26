using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public static class BaseTweetsV2ParametersExtensions
    {
        public static T WithAllFields<T>(this T parameters) where T : BaseTweetsV2Parameters
        {
            parameters.Expansions = TweetFields.Expansions.ALL;
            parameters.MediaFields = TweetFields.Media.ALL;
            parameters.PlaceFields = TweetFields.Place.ALL;
            parameters.PollFields = TweetFields.Polls.ALL;
            parameters.TweetFields = TweetFields.Tweet.ALL;
            parameters.UserFields = TweetFields.User.ALL;
            return parameters;
        }
    }
}