using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryParameterGenerator : IUserQueryParameterGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;

        public UserQueryParameterGenerator(
            IQueryParameterGenerator queryParameterGenerator,
            IUserQueryValidator userQueryValidator)
        {
            _queryParameterGenerator = queryParameterGenerator;
            _userQueryValidator = userQueryValidator;
        }

        public string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_GetCurrentUser);
            parameters = parameters ?? new GetAuthenticatedUserParameters();

            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_email", parameters.IncludeEmail);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GenerateUserIdParameter(long userId, string parameterName = "user_id")
        {
            return string.Format("{0}={1}", parameterName, userId);
        }

        public string GenerateScreenNameParameter(string screenName, string parameterName = "screen_name")
        {
            return string.Format("{0}={1}", parameterName, screenName);
        }

        public string GenerateIdOrScreenNameParameter(
            IUserIdentifier user,
            string idParameterName = "user_id",
            string screenNameParameterName = "screen_name")
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            if (_userQueryValidator.IsUserIdValid(user.Id))
            {
                return GenerateUserIdParameter(user.Id, idParameterName);
            }

            return GenerateScreenNameParameter(user.ScreenName, screenNameParameterName);
        }

        public string GenerateListOfUserIdentifiersParameter(IEnumerable<IUserIdentifier> usersIdentifiers)
        {
            if (usersIdentifiers == null)
            {
                throw new ArgumentNullException("User identifiers cannot be null.");
            }

            var usersList = usersIdentifiers.ToList();
            if (usersList.Any(user => user.Id == TweetinviSettings.DEFAULT_ID && string.IsNullOrEmpty(user.ScreenName)))
            {
                throw new ArgumentException("At least 1 valid user identifier is required.");
            }

            const string initialUserId = "user_id=";
            const string initialScreenName = "&screen_name=";

            StringBuilder idsBuilder = new StringBuilder(initialUserId);
            StringBuilder screeNameBuilder = new StringBuilder(initialScreenName);

            for (int i = 0; i < usersList.Count - 1; ++i)
            {
                var userDTO = usersList[i];

                if (userDTO.Id != TweetinviSettings.DEFAULT_ID)
                {
                    idsBuilder.Append(string.Format("{0}%2C", userDTO.Id));
                }
                else
                {
                    screeNameBuilder.Append(string.Format("{0}%2C", userDTO.ScreenName));
                }
            }

            // Last element does not have a comma
            if (usersList[usersList.Count - 1].Id != -1)
            {
                idsBuilder.Append(usersList[usersList.Count - 1].Id);
            }
            else
            {
                screeNameBuilder.Append(usersList[usersList.Count - 1].ScreenName);
            }

            // Only ids
            if (idsBuilder.ToString() == initialUserId)
            {
                return screeNameBuilder.ToString();
            }

            // Only screenames
            if (screeNameBuilder.ToString() == initialScreenName)
            {
                return idsBuilder.ToString();
            }

            // Both
            return idsBuilder.Append(screeNameBuilder).ToString();
        }

        public string GenerateListOfIdsParameter(IEnumerable<long> ids)
        {
            var idsList = ids.ToList();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < idsList.Count - 1; ++i)
            {
                builder.Append(string.Format("{0}%2C", idsList[i]));
            }

            builder.Append(idsList.ElementAt(idsList.Count - 1));

            return builder.ToString();
        }

        public string GenerateListOfScreenNameParameter(IEnumerable<string> screenNames)
        {
            var screenNamesList = screenNames.ToList();
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < screenNamesList.Count - 1; ++i)
            {
                builder.Append(string.Format("{0}%2C", screenNamesList.ElementAt(i)));
            }

            builder.Append(screenNamesList.ElementAt(screenNamesList.Count - 1));

            return builder.ToString();
        }
    }
}