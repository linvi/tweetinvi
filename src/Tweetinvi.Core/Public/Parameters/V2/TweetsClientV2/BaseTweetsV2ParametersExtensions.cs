using System.Collections.Generic;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public static class BaseTweetsV2ParametersExtensions
    {
        public static T WithAllFields<T>(this T parameters) where T : BaseTweetsV2Parameters
        {
            parameters.Expansions = TweetV2ResponseFields.Expansions.ALL;
            parameters.MediaFields = TweetV2ResponseFields.Media.ALL;
            parameters.PlaceFields = TweetV2ResponseFields.Place.ALL;
            parameters.PollFields = TweetV2ResponseFields.Polls.ALL;
            parameters.TweetFields = TweetV2ResponseFields.Tweet.ALL;
            parameters.UserFields = TweetV2ResponseFields.User.ALL;
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