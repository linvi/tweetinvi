using System;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Core.Parameters.QueryParameters;

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

        // Friends
        public string GetFriendIdsQuery(IUserIdentifier userIdentifier, int maxFriendsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GenerateGetFriendIdsQuery(userIdentifierParameter, maxFriendsToRetrieve);
        }

        private string GenerateGetFriendIdsQuery(string userIdentifierParameter, int maxFriendsToRetrieve)
        {
            return string.Format(Resources.User_GetFriends, userIdentifierParameter, maxFriendsToRetrieve);
        }

        // Followers
        public string GetFollowerIdsQuery(IUserIdentifier userIdentifier, int maxFollowersToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GenerateGetFollowerIdsQuery(userIdentifierParameter, maxFollowersToRetrieve);
        }

        private string GenerateGetFollowerIdsQuery(string userIdentifierParameter, int maxFollowersToRetrieve)
        {
            return string.Format(Resources.User_GetFollowers, userIdentifierParameter, maxFollowersToRetrieve);
        }

        // Favourites

        public string GetFavoriteTweetsQuery(IGetUserFavoritesQueryParameters favoriteParameters)
        {
            if (!_userQueryValidator.CanUserBeIdentified(favoriteParameters.UserIdentifier))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(favoriteParameters.UserIdentifier);
            var query = new StringBuilder(Resources.User_GetFavourites + userIdentifierParameter);

            var parameters = favoriteParameters.Parameters;

            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddParameterToQuery("since_id", parameters.SinceId);
            query.AddParameterToQuery("max_id", parameters.MaxId);
            query.AddParameterToQuery("count", parameters.MaximumNumberOfTweetsToRetrieve);
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(parameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        // Block User
        public string GetBlockUserQuery(IUserIdentifier userIdentifier)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.User_Block_Create, userIdentifierParameter);
        }

        // Unblock
        public string GetUnBlockUserQuery(IUserIdentifier userIdentifier)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.User_Block_Destroy, userIdentifierParameter);
        }

        public string GetUnBlockUserQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return string.Format(Resources.User_Block_Destroy, userIdParameter);
        }

        public string GetUnBlockUserQuery(string userScreenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return string.Format(Resources.User_Block_Destroy, userIdParameter);
        }

        // Get Blocked Users
        public string GetBlockedUserIdsQuery()
        {
            return Resources.User_Block_List_Ids;
        }

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

            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", string.Format("_{0}", imageSize));
        }

        // Report Spam
        public string GetReportUserForSpamQuery(IUserIdentifier userIdentifier)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.User_Report_Spam, userIdentifierParameter);
        }

        public string GetReportUserForSpamQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return string.Format(Resources.User_Report_Spam, userIdParameter);
        }

        public string GetReportUserForSpamQuery(string userScreenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return string.Format(Resources.User_Report_Spam, userIdParameter);
        }
    }
}