using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryParameterGenerator : IUserQueryParameterGenerator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public UserQueryParameterGenerator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public string GenerateUserIdParameter(long? userId, string parameterName = "user_id")
        {
            if (userId == null)
            {
                return null;
            }

            return $"{parameterName}={userId}";
        }

        public string GenerateUserIdParameter(string userId, string parameterName = "user_id")
        {
            if (userId == null)
            {
                return null;
            }

            return $"{parameterName}={userId}";
        }

        public string GenerateScreenNameParameter(string screenName, string parameterName = "screen_name")
        {
            return $"{parameterName}={screenName}";
        }

        public string GenerateIdOrScreenNameParameter(
            IUserIdentifier user,
            string idParameterName = "user_id",
            string screenNameParameterName = "screen_name")
        {
            if (user == null)
            {
                return null;
            }

            if (_userQueryValidator.IsUserIdValid(user.Id))
            {
                return GenerateUserIdParameter(user.Id, idParameterName);
            }

            if (!user.IdStr.IsNullOrEmpty())
            {
                return GenerateUserIdParameter(user.IdStr, idParameterName);
            }

            return GenerateScreenNameParameter(user.ScreenName, screenNameParameterName);
        }

        public string GenerateUserNamesParameter(IUserIdentifier[] users)
        {
            return GenerateListOfScreenNameParameter(users.Select(x => x.ScreenName).ToArray());
        }

        public string GenerateUserIdsParameter(IUserIdentifier[] users)
        {
            return GenerateListOfUserIds(users.Select(x => x.Id).ToArray());
        }

        public void AppendUser(StringBuilder query, IUserIdentifier user)
        {
            query.AddFormattedParameterToQuery(GenerateIdOrScreenNameParameter(user));
        }

        public void AppendUsers(StringBuilder query, IEnumerable<IUserIdentifier> users)
        {
            query.AddFormattedParameterToQuery(GenerateListOfUserIdentifiersParameter(users));
        }


        public string GenerateListOfScreenNameParameter(string[] screenNames)
        {
            if (screenNames == null || screenNames.Length == 0)
            {
                return null;
            }

            return string.Join("%2C", screenNames.Where(x => x != null));
        }

        public string GenerateListOfUserIds(IEnumerable<long?> ids)
        {
            return string.Join("%2C", ids.Where(x => x != null).Select(x => x.ToString()));
        }

        public string GenerateListOfIdsParameter(long?[] ids)
        {
            return GenerateListOfIdsParameter(ids.Where(x => x != null).Cast<long>().ToArray());
        }

        public string GenerateListOfIdsParameter(long[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return null;
            }

            return string.Join("%2C", ids.Select(x => x.ToString()));
        }

        public string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers)
        {
            if (usersIdentifiers == null)
            {
                throw new ArgumentNullException(nameof(usersIdentifiers));
            }

            var usersList = usersIdentifiers.ToArray();

            if (usersList.Any(user => user.Id == TweetinviSettings.DEFAULT_ID && string.IsNullOrEmpty(user.ScreenName)))
            {
                throw new ArgumentException("At least 1 valid user identifier is required.");
            }

            var userIds = new List<long?>();
            var usernames = new List<string>();

            usersList.ForEach(user =>
            {
                if (user.Id != null && user.Id != TweetinviSettings.DEFAULT_ID)
                {
                    userIds.Add(user.Id);
                }
                else if (user.ScreenName != null)
                {
                    usernames.Add(user.ScreenName);
                }
            });

            var parameterBuilder = new StringBuilder();

            if (userIds.Count == 0 && usernames.Count == 0)
            {
                return null;
            }

            if (userIds.Count > 0)
            {
                parameterBuilder.Append($"user_id={GenerateListOfIdsParameter(userIds.ToArray())}");

                if (usernames.Count > 0)
                {
                    parameterBuilder.Append("&");
                }
            }

            if (usernames.Count > 0)
            {
                parameterBuilder.Append($"screen_name={GenerateListOfScreenNameParameter(usernames.ToArray())}");
            }

            return parameterBuilder.ToString();
        }
    }
}