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

        public string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_GetCurrentUser);
            parameters = parameters ?? new GetAuthenticatedUserParameters();

            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_email", parameters.IncludeEmail);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserQuery(IGetUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_GetUser);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode)
        {
            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.UserIdentifiers);
            var query = new StringBuilder(Resources.User_GetUsers);

            query.AddFormattedParameterToQuery(userIdsParameter);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Friends
        public string GetFriendIdsQuery(IGetFriendIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFriends);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Followers
        public string GetFollowUserQuery(IFollowUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.Friendship_Create);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("follow", parameters.EnableNotifications);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnFollowUserQuery(IUnFollowUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.Friendship_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFollowers);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        

        public string GetBlockUserQuery(IBlockUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Block_Create);
            
            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnblockUserQuery(IUnblockUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Block_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Report_Spam);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("perform_block", parameters.PerformBlock);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List_Ids);

            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List);

            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status",  parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Get Blocked Users
        public string GetBlockedUsersQuery()
        {
            return Resources.User_Block_List;
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