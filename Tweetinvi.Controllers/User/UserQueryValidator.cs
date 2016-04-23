using System;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryValidator : IUserQueryValidator
    {
        public bool CanUserBeIdentified(IUserIdentifier userIdentifier)
        {
            return userIdentifier != null && (IsUserIdValid(userIdentifier.Id) || IsScreenNameValid(userIdentifier.ScreenName));
        }

        public void ThrowIfUserCannotBeIdentified(IUserIdentifier userIdentifier, string parameterName = "user")
        {
            if (userIdentifier == null)
            {
                throw new ArgumentException($"{parameterName} cannot be null");
            }

            if (!IsUserIdValid(userIdentifier.Id) && !IsScreenNameValid(userIdentifier.ScreenName))
            {
                throw new ArgumentException($"{parameterName} value is not valid.");
            }
        }

        public bool IsScreenNameValid(string screenName)
        {
            return !String.IsNullOrEmpty(screenName);
        }

        public bool IsUserIdValid(long userId)
        {
            return userId != TweetinviSettings.DEFAULT_ID;
        }

        public bool IsUserIdValid(long? userId)
        {
            return userId != null && userId != TweetinviSettings.DEFAULT_ID;
        }
    }
}