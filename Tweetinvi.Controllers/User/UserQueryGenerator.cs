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

        // Friends
        public string GetFriendIdsQuery(IUserIdentifier user, int maxFriendsToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateGetFriendIdsQuery(userParameter, maxFriendsToRetrieve);
        }

        private string GenerateGetFriendIdsQuery(string userParameter, int maxFriendsToRetrieve)
        {
            return string.Format(Resources.User_GetFriends, userParameter, maxFriendsToRetrieve);
        }

        // Followers
        public string GetFollowerIdsQuery(IUserIdentifier user, int maxFollowersToRetrieve)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GenerateGetFollowerIdsQuery(userParameter, maxFollowersToRetrieve);
        }

        private string GenerateGetFollowerIdsQuery(string userParameter, int maxFollowersToRetrieve)
        {
            return string.Format(Resources.User_GetFollowers, userParameter, maxFollowersToRetrieve);
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

        // Block User
        public string GetBlockUserQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.User_Block_Create, userParameter);
        }

        // Unblock
        public string GetUnBlockUserQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.User_Block_Destroy, userParameter);
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

            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", string.Format("_{0}", imageSize));
        }

        // Report Spam
        public string GetReportUserForSpamQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            string userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.User_Report_Spam, userParameter);
        }
    }
}