using System;
using FakeItEasy;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Testinvi.SetupHelpers
{
    public static class UserQueryGeneratorHelper
    {
        public const string USER_ID_PARAMETER = "user_id={0}";
        public const string SCREEN_NAME_PARAMETER = "screen_name={0}";

        public static void ArrangeGenerateIdParameter(this Fake<IUserQueryParameterGenerator> userQueryGenerator, string result = null)
        {
            userQueryGenerator
                .CallsTo(x => x.GenerateUserIdParameter(A<long>.Ignored, A<string>.Ignored))
                .ReturnsLazily((long userId, string parameterName) =>
                {
                    return result ?? GenerateParameterExpectedResult(userId, parameterName);
                });
        }

        public static void ArrangeGenerateScreenNameParameter(this Fake<IUserQueryParameterGenerator> userQueryGenerator, string result = null)
        {
            userQueryGenerator
                .CallsTo(x => x.GenerateScreenNameParameter(A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily((string screenName, string parameterName) =>
                {
                    return result ?? GenerateParameterExpectedResult(screenName, parameterName);
                });
        }

        public static void ArrangeGenerateIdOrScreenNameParameter(this Fake<IUserQueryParameterGenerator> userQueryGenerator, string result = null)
        {
            userQueryGenerator
                .CallsTo(x => x.GenerateIdOrScreenNameParameter(A<IUserIdentifier>.Ignored, A<string>.Ignored, A<string>.Ignored))
                .ReturnsLazily((IUserIdentifier screenName, string idParameterName, string screenNameParameterName) =>
                {
                    return result ?? GenerateParameterExpectedResult(screenName, idParameterName, screenNameParameterName);
                });
        }
        
        public static string GenerateParameterExpectedResult(long userId, string parameterName = "user_id")
        {
            return string.Format("{0}={1}", parameterName, userId);
        }

        public static string GenerateParameterExpectedResult(string screenName, string parameterName = "screen_name")
        {
            return string.Format("{0}={1}", parameterName, screenName);
        }

        public static string GenerateParameterExpectedResult(IUserIdentifier userDTO, string idParameterName = "user_id", string screenNameParameterName = "screen_name")
        {
            if (userDTO.Id != TweetinviSettings.DEFAULT_ID)
            {
                return string.Format("{0}={1}", idParameterName, userDTO.GetHashCode());
            }

            return string.Format("{0}={1}", screenNameParameterName, userDTO.GetHashCode());
        }
    }
}