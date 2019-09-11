using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryGenerator : IUserQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly IUserQueryValidator _userQueryValidator;

        public UserQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
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

        public string GetUserQuery(IGetUserParameters parameters)
        {
            var query = new StringBuilder(Resources.User_GetUser);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode)
        {
            var userIdsParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(parameters.UserIdentifiers);
            var query = new StringBuilder(Resources.User_GetUsers);

            query.AddFormattedParameterToQuery(userIdsParameter);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(tweetMode));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        // Friends
        public string GetFriendIdsQuery(IGetFriendIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFriends);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("count", parameters.PageSize);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        // Followers
        public string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_GetFollowers);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("count", parameters.PageSize);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        // Favourites
        public string GetFavoriteTweetsQuery(IGetUserFavoritesQueryParameters favoriteParameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(favoriteParameters.UserIdentifier);

            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(favoriteParameters.UserIdentifier);
            var query = new StringBuilder(Resources.User_GetFavourites + userParameter);

            var parameters = favoriteParameters.Parameters;

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("since_id", parameters.SinceId);
            query.AddParameterToQuery("max_id", parameters.MaxId);
            query.AddParameterToQuery("count", parameters.MaximumNumberOfTweetsToRetrieve);

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetBlockUserQuery(IBlockUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Block_Create);
            
            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetUnblockUserQuery(IUnblockUserParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Block_Destroy);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status", parameters.SkipStatus);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.UserIdentifier);

            var query = new StringBuilder(Resources.User_Report_Spam);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.UserIdentifier));
            query.AddParameterToQuery("perform_block", parameters.PerformBlock);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List_Ids);

            query.AddParameterToQuery("count", parameters.PageSize);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        public string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.User_Block_List);

            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("skip_status",  parameters.SkipStatus);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        // Get Blocked Users
        public string GetBlockedUsersQuery()
        {
            return Resources.User_Block_List;
        }

        // Download Profile Image
        public string DownloadProfileImageURL(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = string.IsNullOrEmpty(userDTO.ProfileImageUrlHttps) ? userDTO.ProfileImageUrl : userDTO.ProfileImageUrlHttps;

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", string.Format("_{0}", imageSize));
        }

        public string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = userDTO.ProfileImageUrl;

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", string.Format("_{0}", imageSize));
        }
    }
}