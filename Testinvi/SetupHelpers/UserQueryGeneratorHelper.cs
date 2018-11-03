using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;

namespace Testinvi.SetupHelpers
{
    public static class UserQueryGeneratorHelper
    {
        public const string USER_ID_PARAMETER = "user_id={0}";
        public const string SCREEN_NAME_PARAMETER = "screen_name={0}";

        public static void ArrangeGenerateIdParameter(this IUserQueryParameterGenerator userQueryGenerator, string result = null)
        {
            A.CallTo(() => userQueryGenerator.GenerateUserIdParameter(A<long>.Ignored, A<string>.Ignored))
                .ReturnsLazily((long userId, string parameterName) =>
                    result ?? GenerateParameterExpectedResult(userId, parameterName));
        }

        public static void ArrangeGenerateScreenNameParameter(this IUserQueryParameterGenerator userQueryGenerator, string result = null)
        {
            A.CallTo(() => userQueryGenerator.GenerateScreenNameParameter(A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily((string screenName, string parameterName) =>
                    result ?? GenerateParameterExpectedResult(screenName, parameterName));
        }

        public static void ArrangeGenerateIdOrScreenNameParameter(this IUserQueryParameterGenerator userQueryGenerator, string result = null)
        {
            A.CallTo(() =>
                    userQueryGenerator.GenerateIdOrScreenNameParameter(A<IUserIdentifier>.Ignored, A<string>.Ignored,
                        A<string>.Ignored))
                .ReturnsLazily((IUserIdentifier screenName, string idParameterName, string screenNameParameterName) =>
                    result ?? GenerateParameterExpectedResult(screenName, idParameterName, screenNameParameterName));
        }
        
        public static string GenerateParameterExpectedResult(long userId, string parameterName = "user_id")
        {
            return string.Format("{0}={1}", parameterName, userId);
        }

        public static string GenerateParameterExpectedResult(string screenName, string parameterName = "screen_name")
        {
            return string.Format("{0}={1}", parameterName, screenName);
        }

        public static string GenerateParameterExpectedResult(IUserIdentifier user, string idParameterName = "user_id", string screenNameParameterName = "screen_name")
        {
            if (user.Id != TweetinviSettings.DEFAULT_ID)
            {
                return string.Format("{0}={1}", idParameterName, user.GetHashCode());
            }

            return string.Format("{0}={1}", screenNameParameterName, user.GetHashCode());
        }
    }
}