using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryValidator : IUserQueryValidator
    {
        public bool CanUserBeIdentified(IUserIdentifier userIdentifier)
        {
            return userIdentifier != null && (IsUserIdValid(userIdentifier.Id) || IsScreenNameValid(userIdentifier.ScreenName));
        }

        public void ThrowIfUserCannotBeIdentified(IUserIdentifier userIdentifier)
        {
            ThrowIfUserCannotBeIdentified(userIdentifier, "user");
        }

        public void ThrowIfUserCannotBeIdentified(IUserIdentifier userIdentifier, string parameterName)
        {
            if (userIdentifier == null)
            {
                throw new ArgumentNullException($"{parameterName} cannot be null");
            }

            if (!IsUserIdValid(userIdentifier.Id) && !IsScreenNameValid(userIdentifier.ScreenName))
            {
                throw new ArgumentException($"{parameterName} is not valid.");
            }
        }

        public bool IsScreenNameValid(string screenName)
        {
            return !string.IsNullOrEmpty(screenName);
        }

        public bool IsUserIdValid(long? userId)
        {
            return userId != null && userId != TweetinviSettings.DEFAULT_ID;
        }
    }
}