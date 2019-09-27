using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryGenerator : IUserQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;

        public UserQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
            _userQueryValidator = userQueryValidator;
        }

        public string GetUserQuery(IGetUserParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.User_GetUser);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode)
        {
            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.UserIdentifiers);
            var query = new StringBuilder(Resources.User_GetUsers);

            query.AddFormattedParameterToQuery(userIdsParameter);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // FOLLOWERS
        public string GetFriendIdsQuery(IGetFriendIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFriends);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFollowers);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("cursor", parameters.Cursor);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRelationshipBetweenQuery(IGetRelationshipBetweenParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.SourceUser);
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.TargetUser);

            var sourceParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.SourceUser, "source_id", "source_screen_name");
            var targetParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.TargetUser, "target_id", "target_screen_name");

            var query = new StringBuilder(Resources.Friendship_GetRelationship);

            query.AddFormattedParameterToQuery(sourceParameter);
            query.AddFormattedParameterToQuery(targetParameter);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Download Profile Image
        public string DownloadProfileImageURL(IGetProfileImageParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.ImageUrl))
            {
                throw new ArgumentException("ImageUrl cannot be null or empty", nameof(parameters));
            }

            var query = new StringBuilder(parameters.ImageUrl.Replace("_normal", $"_{parameters.ImageSize}"));

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}