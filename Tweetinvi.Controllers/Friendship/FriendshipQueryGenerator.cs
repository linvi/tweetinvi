using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;

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
        public string GetMultipleRelationshipsQuery(IEnumerable<IUserIdentifier> users)
        {
            var userIdsAndScreenNameParameter = _userQueryParameterGenerator.GenerateListOfUserIdentifiersParameter(users);
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
        public string GetCreateFriendshipWithQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.Friendship_Create, userParameter);
        }

        // Destroy Friendship
        public string GetDestroyFriendshipWithQuery(IUserIdentifier user)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return string.Format(Resources.Friendship_Destroy, userParameter);
        }

        // Update Relationship
        public string GetUpdateRelationshipAuthorizationsWithQuery(IUserIdentifier user, IFriendshipAuthorizations friendshipAuthorizations)
        {
            _userQueryValidator.ThrowIfUserCannotBeIdentified(user);

            if (friendshipAuthorizations == null)
            {
                throw new ArgumentNullException("Friendship authorizations cannot be null.");
            }

            var userParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(user);
            return GetUpdateRelationshipAuthorizationQuery(userParameter, friendshipAuthorizations);
        }

        private string GetUpdateRelationshipAuthorizationQuery(string userParameter, IFriendshipAuthorizations friendshipAuthorizations)
        {
            return string.Format(Resources.Friendship_Update, 
                                 friendshipAuthorizations.RetweetsEnabled.ToString().ToLowerInvariant(),
                                 friendshipAuthorizations.DeviceNotificationEnabled.ToString().ToLowerInvariant(),
                                 userParameter);
        }
    }
}