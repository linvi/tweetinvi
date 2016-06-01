using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
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
            _userQueryValidator.ThrowIfUserCannotBeIdentified(sourceUserIdentifier, "Source user");
            _userQueryValidator.ThrowIfUserCannotBeIdentified(targetUserIdentifier, "Target user");

            var sourceParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(sourceUserIdentifier, "source_id", "source_screen_name");
            var targetParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(targetUserIdentifier, "target_id", "target_screen_name");
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
                throw new ArgumentNullException("Target user ids parameter cannot be null.");
            }

            if (targetUsersId.IsEmpty())
            {
                throw new ArgumentException("Target user ids parameter cannot be empty.");
            }

            string userIds = _userQueryParameterGenerator.GenerateListOfIdsParameter(targetUsersId);
            string userIdsParameter = string.Format("user_id={0}", userIds);
            return string.Format(Resources.Friendship_GetRelationships, userIdsParameter);
        }

        public string GetMultipleRelationshipsQuery(IEnumerable<string> targetUsersScreenName)
        {
            if (targetUsersScreenName == null)
            {
                throw new ArgumentNullException("Target user screen names parameter cannot be null.");
            }

            if (targetUsersScreenName.IsEmpty())
            {
                throw new ArgumentException("Target user screen names parameter cannot be empty.");
            }

            string userScreenNames = _userQueryParameterGenerator.GenerateListOfScreenNameParameter(targetUsersScreenName);
            string userScreenNamesParameter = string.Format("screen_name={0}", userScreenNames);
            return string.Format(Resources.Friendship_GetRelationships, userScreenNamesParameter);
        }

        // Create Friendship
        public string GetCreateFriendshipWithQuery(IUserIdentifier userIdentifier)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.Friendship_Create, userIdentifierParameter);
        }

        // Destroy Friendship
        public string GetDestroyFriendshipWithQuery(IUserIdentifier userIdentifier)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return string.Format(Resources.Friendship_Destroy, userIdentifierParameter);
        }

        // Update Relationship
        public string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier userIdentifier, IFriendshipAuthorizations friendshipAuthorizations)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(userIdentifier);

            if (friendshipAuthorizations == null)
            {
                throw new ArgumentNullException("Friendship authorizations cannot be null.");
            }

            var userIdentifierParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(userIdentifier);
            return GetUpdateRelationshipAuthorizationQuery(userIdentifierParameter, friendshipAuthorizations);
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