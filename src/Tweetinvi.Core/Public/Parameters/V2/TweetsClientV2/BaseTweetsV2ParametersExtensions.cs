using System.Collections.Generic;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public static class BaseTweetsV2ParametersExtensions
    {
        public static T WithAllFields<T>(this T parameters) where T : BaseTweetsV2Parameters
        {
            parameters.Expansions = TweetResponseFields.Expansions.ALL;
            parameters.MediaFields = TweetResponseFields.Media.ALL;
            parameters.PlaceFields = TweetResponseFields.Place.ALL;
            parameters.PollFields = TweetResponseFields.Polls.ALL;
            parameters.TweetFields = TweetResponseFields.Tweet.ALL;
            parameters.UserFields = TweetResponseFields.User.ALL;
            return parameters;
        }

        public static T WithNoFields<T>(this T parameters) where T : BaseTweetsV2Parameters
        {
            parameters.ClearAllFields();
            return parameters;
        }
    }

    public static class BaseUsersV2ParametersExtensions
    {
        public static T WithAllFields<T>(this T parameters) where T : BaseUsersV2Parameters
        {
            parameters.Expansions = UserResponseFields.Expansions.ALL;
            parameters.TweetFields = UserResponseFields.Tweet.ALL;
            parameters.UserFields = UserResponseFields.User.ALL;
            return parameters;
        }

        public static T WithNoFields<T>(this T parameters) where T : BaseUsersV2Parameters
        {
            parameters.ClearAllFields();
            return parameters;
        }
    }
}