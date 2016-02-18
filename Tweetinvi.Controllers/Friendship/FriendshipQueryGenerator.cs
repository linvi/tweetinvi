using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipQueryGenerator : IFriendshipQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IUserQueryValidator _userQueryValidator;

        public FriendshipQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IUserQueryValidator userQueryValidator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _userQueryValidator = userQueryValidator;
        }

        // Get Friendship
        public string GetUserIdsRequestingFriendshipQuery()
        {
            return Resources.Friendship_GetIncomingIds;
        }

        public string GetUserIdsYouRequestedToFollowQuery()
        {
            return Resources.Friendship_GetOutgoingIds;
        }

        public string GetUserIdsWhoseRetweetsAreMutedQuery()
        {
            return Resources.Friendship_FriendIdsWithNoRetweets;
        }

        // Get Existing Relationship
        public string GetRelationshipDetailsQuery(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            if (!_userQueryValidator.CanUserBeIdentified(sourceUserIdentifier) ||
                !_userQueryValidator.CanUserBeIdentified(targetUserIdentifier))
            {
                return null;
            }

            string sourceParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(sourceUserIdentifier, "source_id", "source_screen_name");
            string targetParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(targetUserIdentifier, "target_id", "target_screen_name");
            return string.Format(Resources.Friendship_GetRelationship, sourceParameter, targetParameter);
        }

        
        // Lookup Relationship State
        public string GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> userIdentifiers)
        {
            if (userIdentifiers == null)
            {
                return null;
            }

            string userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(userIdentifiers);
            return string.Format(Resources.Friendship_GetRelationships, userIdsAndScreenNameParameter);
        }

        public string GetMultipleRelationshipsQuery(IEnumerable<long> targetUsersId)
        {
            if (targetUsersId == null)
            {
                return null;
            }

            string userIds = _userQueryParameterGenerator.GenerateListOfIdsParameter(targetUsersId);
            string userIdsParameter = string.Format("user_id={0}", userIds);
            return string.Format(Resources.Friendship_GetRelationships, userIdsParameter);
        }

        public string GetMultipleRelationshipsQuery(IEnumerable<string> targetUsersScreenName)
        {
            if (targetUsersScreenName == null)
            {
                return null;
            }

            string userScreenNames = _userQueryParameterGenerator.GenerateListOfScreenNameParameter(targetUsersScreenName);
            string userScreenNamesParameter = string.Format("screen_name={0}", userScreenNames);
            return string.Format(Resources.Friendship_GetRelationships, userScreenNamesParameter);
        }

        // Create Friendship
        public string GetCreateFriendshipWithQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateCreateFriendshipQuery(userIdentifierParameter);
        }

        public string GetCreateFriendshipWithQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateCreateFriendshipQuery(userIdParameter);
        }

        public string GetCreateFriendshipWithQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateCreateFriendshipQuery(userScreenNameParameter);
        }

        private string GenerateCreateFriendshipQuery(string userIdentifierParameter)
        {
            return string.Format(Resources.Friendship_Create, userIdentifierParameter);
        }

        // Destroy Friendship
        public string GetDestroyFriendshipWithQuery(IUserIdentifier userDTO)
        {
            if (!_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GenerateDestroyFriendshipQuery(userIdentifierParameter);
        }

        public string GetDestroyFriendshipWithQuery(long userId)
        {
            if (!_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GenerateDestroyFriendshipQuery(userIdParameter);
        }

        public string GetDestroyFriendshipWithQuery(string screenName)
        {
            if (!_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GenerateDestroyFriendshipQuery(userScreenNameParameter);
        }

        private string GenerateDestroyFriendshipQuery(string userIdentifierParameter)
        {
            return string.Format(Resources.Friendship_Destroy, userIdentifierParameter);
        }

        // Update Relationship
        public string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userDTO, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.CanUserBeIdentified(userDTO))
            {
                return null;
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userDTO);
            return GetUpdateRelationshipAuthorizationQuery(userIdentifierParameter, friendshipAuthorizations);
        }

        public string GetUpdateRelationshipAuthorizationsWithQuery(long userId, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.IsUserIdValid(userId))
            {
                return null;
            }

            string userIdParameter = _userQueryParameterGenerator.GenerateUserIdParameter(userId);
            return GetUpdateRelationshipAuthorizationQuery(userIdParameter, friendshipAuthorizations);
        }

        public string GetUpdateRelationshipAuthorizationsWithQuery(string screenName, IFriendshipAuthorizations friendshipAuthorizations)
        {
            if (friendshipAuthorizations == null || !_userQueryValidator.IsScreenNameValid(screenName))
            {
                return null;
            }

            string userScreenNameParameter = _userQueryParameterGenerator.GenerateScreenNameParameter(screenName);
            return GetUpdateRelationshipAuthorizationQuery(userScreenNameParameter, friendshipAuthorizations);
        }

        private string GetUpdateRelationshipAuthorizationQuery(string userIdentifierParameter, IFriendshipAuthorizations friendshipAuthorizations)
        {
            return string.Format(Resources.Friendship_Update, 
                                 friendshipAuthorizations.RetweetsEnabled.ToString().ToLowerInvariant(),
                                 friendshipAuthorizations.DeviceNotificationEnabled.ToString().ToLowerInvariant(),
                                 userIdentifierParameter);
        }
    }
}