using System;
using Tweetinvi.Controllers.Properties;
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
        public string GetUserIdsYouRequestedToFollowQuery()
        {
            return Resources.Friendship_GetOutgoingIds;
        }

        public string GetUserIdsWhoseRetweetsAreMutedQuery()
        {
            return Resources.Friendship_FriendIdsWithNoRetweets;
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