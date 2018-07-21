using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryValidator : IUserQueryValidator
    {
        public bool CanUserBeIdentified(IUserIdentifier user)
        {
            return user != null && (IsUserIdValid(user.Id) || IsScreenNameValid(user.ScreenName));
        }

        public void ThrowIfUserCannotBeIdentified(IUserIdentifier user)
        {
            ThrowIfUserCannotBeIdentified(user, "user");
        }

        public void ThrowIfUserCannotBeIdentified(IUserIdentifier user, string parameterName)
        {
            if (user == null)
            {
                throw new ArgumentNullException($"{parameterName} cannot be null");
            }

            if (!IsUserIdValid(user.Id) && !IsScreenNameValid(user.ScreenName))
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
            return userId != null && userId != TweetinviSettings.DEFAULT_ID && userId != default(long);
        }
    }
}