using System;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.User
{
    public class UserQueryGenerator : IUserQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;

        public UserQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
        }

        // Friends
        public string GetFriendIdsQuery(IUserIdentifier userDTO, int maxFriendsToRetrieve)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateGetFriendIdsQuery(userIdentifierParameter, maxFriendsToRetrieve);
        }

        public string GetFriendIdsQuery(long userId, int maxFriendsToRetrieve)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateGetFriendIdsQuery(userIdParameter, maxFriendsToRetrieve);
        }

        public string GetFriendIdsQuery(string screenName, int maxFriendsToRetrieve)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateGetFriendIdsQuery(userScreenNameParameter, maxFriendsToRetrieve);
        }

        private string GenerateGetFriendIdsQuery(string userIdentifierParameter, int maxFriendsToRetrieve)
        {
            return string.Format(Resources.User_GetFriends, userIdentifierParameter, maxFriendsToRetrieve);
        }

        // Followers
        public string GetFollowerIdsQuery(IUserIdentifier userDTO, int maxFollowersToRetrieve)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateGetFollowerIdsQuery(userIdentifierParameter, maxFollowersToRetrieve);
        }

        public string GetFollowerIdsQuery(long userId, int maxFollowersToRetrieve)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateGetFollowerIdsQuery(userIdParameter, maxFollowersToRetrieve);
        }

        public string GetFollowerIdsQuery(string screenName, int maxFollowersToRetrieve)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateGetFollowerIdsQuery(userScreenNameParameter, maxFollowersToRetrieve);
        }

        private string GenerateGetFollowerIdsQuery(string userIdentifierParameter, int maxFollowersToRetrieve)
        {
            return String.Format(Resources.User_GetFollowers, userIdentifierParameter, maxFollowersToRetrieve);
        }

        // Favourites
        public string GetFavouriteTweetsQuery(IUserIdentifier userDTO, int maxFavouritesToRetrieve)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateGetFavouriteTweetsQuery(userIdentifierParameter, maxFavouritesToRetrieve);
        }

        public string GetFavouriteTweetsQuery(long userId, int maxFavouritesToRetrieve)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateGetFavouriteTweetsQuery(userIdParameter, maxFavouritesToRetrieve);
        }

        public string GetFavouriteTweetsQuery(string screenName, int maxFavouritesToRetrieve)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateGetFavouriteTweetsQuery(userIdParameter, maxFavouritesToRetrieve);
        }

        private string GenerateGetFavouriteTweetsQuery(string userIdentifierParameter, int maxFavouritesToRetrieve)
        {
            return String.Format(Resources.User_GetFavourites, userIdentifierParameter, maxFavouritesToRetrieve);
        }

        // Block User
        public string GetBlockUserQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return String.Format(Resources.User_Block_Create, userIdentifierParameter);
        }

        public string GetBlockUserQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return String.Format(Resources.User_Block_Create, userIdParameter);
        }

        public string GetBlockUserQuery(string userScreenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return String.Format(Resources.User_Block_Create, userIdParameter);
        }

        // Unblock
        public string GetUnBlockUserQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return String.Format(Resources.User_Block_Destroy, userIdentifierParameter);
        }

        public string GetUnBlockUserQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return String.Format(Resources.User_Block_Destroy, userIdParameter);
        }

        public string GetUnBlockUserQuery(string userScreenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return String.Format(Resources.User_Block_Destroy, userIdParameter);
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
            var url = String.IsNullOrEmpty(userDTO.ProfileImageUrlHttps) ? userDTO.ProfileImageUrl : userDTO.ProfileImageUrlHttps;

            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", String.Format("_{0}", imageSize));
        }

        public string DownloadProfileImageInHttpURL(IUserDTO userDTO, ImageSize imageSize = ImageSize.normal)
        {
            var url = userDTO.ProfileImageUrl;

            if (String.IsNullOrEmpty(url))
            {
                return null;
            }

            return url.Replace("_normal", String.Format("_{0}", imageSize));
        }

        // Report Spam
        public string GetReportUserForSpamQuery(IUserIdentifier userIdentifier)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userIdentifier))
            {
                return null;
            }

            string userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return String.Format(Resources.User_Report_Spam, userIdentifierParameter);
        }

        public string GetReportUserForSpamQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return String.Format(Resources.User_Report_Spam, userIdParameter);
        }

        public string GetReportUserForSpamQuery(string userScreenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(userScreenName))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(userScreenName);
            return String.Format(Resources.User_Report_Spam, userIdParameter);
        }
    }
}