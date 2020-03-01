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

        public string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.User_GetCurrentUser);
            parameters = parameters ?? new GetAuthenticatedUserParameters();

            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("include_email", parameters.IncludeEmail);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserQuery(IGetUserParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.User_GetUser);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode)
        {
            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.Users);
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
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);

            var query = new StringBuilder(Resources.User_GetFriends);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);

            var query = new StringBuilder(Resources.User_GetFollowers);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            _queryParameterGenerator.AppendCursorParameters(query, parameters);
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
            var query = new StringBuilder(parameters.ImageUrl.Replace("_normal", $"_{parameters.ImageSize.ToString().ToLowerInvariant()}"));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

         // BLOCK
        public string GetBlockUserQuery(IBlockUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_Create);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnblockUserQuery(IUnblockUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Report_Spam);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("perform_block", parameters.PerformBlock);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List_Ids);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status",  parameters.SkipStatus);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // FOLLOWERS
        public string GetFollowUserQuery(IFollowUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Create);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("follow", parameters.EnableNotifications);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUpdateRelationshipQuery(IUpdateRelationshipParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Update);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddParameterToQuery("device", parameters.EnableDeviceNotifications);
            query.AddParameterToQuery("retweets", parameters.EnableRetweets);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnFollowUserQuery(IUnFollowUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserIdsRequestingFriendshipQuery(IGetUserIdsRequestingFriendshipParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetIncomingIds);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUserIdsYouRequestedToFollowQuery(IGetUserIdsYouRequestedToFollowParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetOutgoingIds);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRelationshipsWithQuery(IGetRelationshipsWithParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_GetRelationships);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.Users));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // MUTE
        public string GetUserIdsWhoseRetweetsAreMutedQuery(IGetUserIdsWhoseRetweetsAreMutedParameters parameters)
        {
            var query = new StringBuilder(Resources.Friendship_FriendIdsWithNoRetweets);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetMutedUserIdsQuery(IGetMutedUserIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_Mute_GetUserIds);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetMutedUsersQuery(IGetMutedUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_Mute_GetUsers);

            _queryParameterGenerator.AppendCursorParameters(query, parameters);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetMuteUserQuery(IMuteUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_Mute_Create);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetUnMuteUserQuery(IUnMuteUserParameters parameters)
        {
            var query = new StringBuilder(Resources.Account_Mute_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}