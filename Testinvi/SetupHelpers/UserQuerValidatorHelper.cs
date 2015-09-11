using System;
using FakeItEasy;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Testinvi.SetupHelpers
{
    public static class UserQuerValidatorHelper
    {
        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, bool? result = null)
        {
            userQueryValidator
                .CallsTo(x => x.IsScreenNameValid(A<string>.Ignored))
                .ReturnsLazily((string screenName) =>
                {
                    return result ?? !String.IsNullOrEmpty(screenName);
                });
        }

        public static void ArrangeIsScreenNameValid(this Fake<IUserQueryValidator> userQueryValidator, string screenName, bool result)
        {
            userQueryValidator
                .CallsTo(x => x.IsScreenNameValid(screenName))
                .Returns(result);
        }

        public static void ArrangeIsUserIdValid(this Fake<IUserQueryValidator> userQueryValidator, bool? result = null)
        {
            userQueryValidator
                .CallsTo(x => x.IsUserIdValid(A<long>.Ignored))
                .ReturnsLazily((long id) =>
                {
                    return result == null && id != TweetinviSettings.DEFAULT_ID || result != null && result.Value;
                });
        }

        public static void ArrangeCanUserBeIdentified(this Fake<IUserQueryValidator> userQueryValidator, IUserIdentifier userIdentifier, bool result)
        {
            userQueryValidator
                .CallsTo(x => x.CanUserBeIdentified(userIdentifier))
                .Returns(result);
        }
    }
}