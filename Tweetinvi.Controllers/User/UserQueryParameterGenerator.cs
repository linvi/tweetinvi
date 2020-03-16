using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryParameterGenerator : IUserQueryParameterGenerator
    {
        private string GenerateUserIdParameter(long userId, string parameterName = "user_id")
        {
            if (userId <= 0)
            {
                return null;
            }

            return $"{parameterName}={userId.ToString(CultureInfo.InvariantCulture)}";
        }

        private string GenerateUserIdParameter(string userId, string parameterName = "user_id")
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

            if (user.Id > 0)
            {
                return GenerateUserIdParameter(user.Id, idParameterName);
            }

            if (!user.IdStr.IsNullOrEmpty())
            {
                return GenerateUserIdParameter(user.IdStr, idParameterName);
            }

            return GenerateScreenNameParameter(user.ScreenName, screenNameParameterName);
        }

        public void AppendUser(StringBuilder query, IUserIdentifier user)
        {
            query.AddFormattedParameterToQuery(GenerateIdOrScreenNameParameter(user));
        }

        public void AppendUsers(StringBuilder query, IEnumerable<IUserIdentifier> users)
        {
            query.AddFormattedParameterToQuery(GenerateListOfUserIdentifiersParameter(users));
        }

        private string GenerateCollectionParameter(string[] screenNames)
        {
            return string.Join(",", screenNames.Where(x => x != null));
        }

        public string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers)
        {
            if (usersIdentifiers == null)
            {
                throw new ArgumentNullException(nameof(usersIdentifiers));
            }

            var usersList = usersIdentifiers.ToArray();

            if (usersList.Any(user => user.Id <= 0 && string.IsNullOrEmpty(user.IdStr) && string.IsNullOrEmpty(user.ScreenName)))
            {
                throw new ArgumentException("At least 1 valid user identifier is required.");
            }

            var userIds = new List<string>();
            var usernames = new List<string>();

            usersList.ForEach(user =>
            {
                if (user.Id > 0)
                {
                    userIds.Add(user.Id.ToString(CultureInfo.InvariantCulture));
                }
                else if (!string.IsNullOrEmpty(user.IdStr))
                {
                    userIds.Add(user.IdStr);
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

            parameterBuilder.AddParameterToQuery("user_id", GenerateCollectionParameter(userIds.ToArray()));
            parameterBuilder.AddParameterToQuery("screen_name", GenerateCollectionParameter(usernames.ToArray()));

            return parameterBuilder.ToString();
        }
    }
}