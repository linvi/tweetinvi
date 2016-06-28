using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipController : IFriendshipController
    {
        private readonly IFriendshipQueryExecutor _friendshipQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly IFactory<IRelationshipDetails> _relationshipFactory;
        private readonly IFactory<IRelationshipState> _relationshipStateFactory;
        private readonly IFactory<IFriendshipAuthorizations> _friendshipAuthorizationsFactory;

        public FriendshipController(
            IFriendshipQueryExecutor friendshipQueryExecutor,
            IUserFactory userFactory,
            IFriendshipFactory friendshipFactory,
            IFactory<IRelationshipDetails> relationshipFactory,
            IFactory<IRelationshipState> relationshipStateFactory,
            IFactory<IFriendshipAuthorizations> friendshipAuthorizationsFactory)
        {
            _friendshipQueryExecutor = friendshipQueryExecutor;
            _userFactory = userFactory;
            _friendshipFactory = friendshipFactory;
            _relationshipFactory = relationshipFactory;
            _relationshipStateFactory = relationshipStateFactory;
            _friendshipAuthorizationsFactory = friendshipAuthorizationsFactory;
        }

        // Get Users Requesting Friendship
        public IEnumerable<long> GetUserIdsRequestingFriendship(int maximumUserIdsToRetrieve = 75000)
        {
            return _friendshipQueryExecutor.GetUserIdsRequestingFriendship(maximumUserIdsToRetrieve);
        }

        public IEnumerable<IUser> GetUsersRequestingFriendship(int maximumUsersToRetrieve = 75000)
        {
            var userIds = GetUserIdsRequestingFriendship(maximumUsersToRetrieve);
            return _userFactory.GetUsersFromIds(userIds);
        }

        // Get Users You requested to follow
        public IEnumerable<long> GetUserIdsYouRequestedToFollow(int maximumUsersToRetrieve = 75000)
        {
            return _friendshipQueryExecutor.GetUserIdsYouRequestedToFollow(maximumUsersToRetrieve);
        }

        public IEnumerable<IUser> GetUsersYouRequestedToFollow(int maximumUsersToRetrieve = 75000)
        {
            var userIds = GetUserIdsYouRequestedToFollow(maximumUsersToRetrieve);
            return _userFactory.GetUsersFromIds(userIds);
        }

        // Get Users not authorized to retweet
        public IEnumerable<long> GetUserIdsWhoseRetweetsAreMuted()
        {
            return _friendshipQueryExecutor.GetUserIdsWhoseRetweetsAreMuted();
        }

        public IEnumerable<IUser> GetUsersWhoseRetweetsAreMuted()
        {
            var userIds = GetUserIdsWhoseRetweetsAreMuted();
            return _userFactory.GetUsersFromIds(userIds);
        }

        // Create Friendship with
        public bool CreateFriendshipWith(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return CreateFriendshipWith(user.UserDTO);
        }

        public bool CreateFriendshipWith(IUserIdentifier userIdentifier)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(userIdentifier);
        }

        public bool CreateFriendshipWith(long userId)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(new UserIdentifier(userId));
        }

        public bool CreateFriendshipWith(string userScreeName)
        {
            return _friendshipQueryExecutor.CreateFriendshipWith(new UserIdentifier(userScreeName));
        }

        // Destroy Friendship with

        public bool DestroyFriendshipWith(IUserIdentifier userIdentifier)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(userIdentifier);
        }

        public bool DestroyFriendshipWith(long userId)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(new UserIdentifier(userId));
        }

        public bool DestroyFriendshipWith(string userScreeName)
        {
            return _friendshipQueryExecutor.DestroyFriendshipWith(new UserIdentifier(userScreeName));
        }

        // Update Friendship Authorizations
        public bool UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return UpdateRelationshipAuthorizationsWith(user.UserDTO, retweetsEnabled, deviceNotifictionEnabled);
        }

        public bool UpdateRelationshipAuthorizationsWith(IUserIdentifier userIdentifier, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(userIdentifier, friendshipAuthorizations);
        }

        public bool UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(new UserIdentifier(userId), friendshipAuthorizations);
        }

        public bool UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            return _friendshipQueryExecutor.UpdateRelationshipAuthorizationsWith(new UserIdentifier(userScreenName), friendshipAuthorizations);
        }

        // Get Relationship (get between 2 users as there is no id for a relationship)
        public IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, IUserIdentifier targetUserIdentifier)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetRelationshipBetween(sourceUserIdentifier, targetUserIdentifier);
            return GenerateRelationshipFromRelationshipDTO(relationshipDTO);
        }

        public IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, long targetUserId)
        {
            return InternalGetRelationshipBetween(sourceUserIdentifier, targetUserId);
        }

        public IRelationshipDetails GetRelationshipBetween(IUserIdentifier sourceUserIdentifier, string targetUserScreenName)
        {
            return InternalGetRelationshipBetween(sourceUserIdentifier, targetUserScreenName);
        }

        public IRelationshipDetails GetRelationshipBetween(long sourceUserId, IUserIdentifier targetUserIdentifier)
        {
            return InternalGetRelationshipBetween(sourceUserId, targetUserIdentifier);
        }

        public IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, IUserIdentifier targetUserIdentifier)
        {
            return InternalGetRelationshipBetween(sourceUserScreenName, targetUserIdentifier);
        }

        public IRelationshipDetails GetRelationshipBetween(long sourceUserId, long targetUserId)
        {
            return InternalGetRelationshipBetween(sourceUserId, targetUserId);
        }

        public IRelationshipDetails GetRelationshipBetween(long sourceUserId, string targetUserScreenName)
        {
            return InternalGetRelationshipBetween(sourceUserId, targetUserScreenName);
        }

        public IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, long targetUserId)
        {
            return InternalGetRelationshipBetween(sourceUserScreenName, targetUserId);
        }

        public IRelationshipDetails GetRelationshipBetween(string sourceUserScreenName, string targetUserScreenName)
        {
            return InternalGetRelationshipBetween(sourceUserScreenName, targetUserScreenName);
        }

        private IRelationshipDetails InternalGetRelationshipBetween(object sourceIdentifier, object targetIdentifier)
        {
            IUserIdentifier sourceUserIdentifier = null;
            IUserIdentifier targetUserIdentifier = null;

            if (sourceIdentifier is long)
            {
                sourceUserIdentifier = _userFactory.GenerateUserIdentifierFromId((long)sourceIdentifier);
            }
            else
            {
                var screenName = sourceIdentifier as string;
                if (screenName != null)
                {
                    sourceUserIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(screenName);
                }
                else
                {
                    sourceUserIdentifier = sourceIdentifier as IUserIdentifier;
                }
            }

            if (targetIdentifier is long)
            {
                targetUserIdentifier = _userFactory.GenerateUserIdentifierFromId((long)targetIdentifier);
            }
            else
            {
                var screenName = targetIdentifier as string;
                if (screenName != null)
                {
                    targetUserIdentifier = _userFactory.GenerateUserIdentifierFromScreenName(screenName);
                }
                else
                {
                    targetUserIdentifier = targetIdentifier as IUserIdentifier;
                }
            }

            return GetRelationshipBetween(sourceUserIdentifier, targetUserIdentifier);
        }

        // Get multiple relationships
        public Dictionary<IUser, IRelationshipState> GetRelationshipStatesAssociatedWith(IEnumerable<IUser> targetUsers)
        {
            if (targetUsers == null)
            {
                throw new ArgumentNullException("Target users cannot be null.");
            }

            if (targetUsers.IsEmpty())
            {
                throw new ArgumentNullException("Target users cannot be empty.");
            }

            var relationshipStates = GetMultipleRelationships(targetUsers.Select(x => x.UserDTO).ToList());
            var userRelationshipState = new Dictionary<IUser, IRelationshipState>();

            foreach (var targetUser in targetUsers)
            {
                var userRelationship = relationshipStates.FirstOrDefault(x => x.TargetId == targetUser.Id ||
                                                                              x.TargetScreenName == targetUser.ScreenName);
                userRelationshipState.Add(targetUser, userRelationship);
            }

            return userRelationshipState;
        }

        public IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<IUserIdentifier> targetUserIdentifiers)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetMultipleRelationshipsQuery(targetUserIdentifiers);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        public IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<long> targetUsersId)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetMultipleRelationshipsQuery(targetUsersId);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        public IEnumerable<IRelationshipState> GetMultipleRelationships(IEnumerable<string> targetUsersScreenName)
        {
            var relationshipDTO = _friendshipQueryExecutor.GetMultipleRelationshipsQuery(targetUsersScreenName);
            return GenerateRelationshipStatesFromRelationshipStatesDTO(relationshipDTO);
        }

        // Generate From DTO
        private IRelationshipDetails GenerateRelationshipFromRelationshipDTO(IRelationshipDetailsDTO relationshipDetailsDTO)
        {
            if (relationshipDetailsDTO == null)
            {
                return null;
            }

            var relationshipParameter = _relationshipFactory.GenerateParameterOverrideWrapper("relationshipDetailsDTO", relationshipDetailsDTO);
            return _relationshipFactory.Create(relationshipParameter);
        }

        // Generate Relationship state from DTO
        private IRelationshipState GenerateRelationshipStateFromRelationshipStateDTO(IRelationshipStateDTO relationshipStateDTO)
        {
            if (relationshipStateDTO == null)
            {
                return null;
            }

            var relationshipStateParameter = _relationshipFactory.GenerateParameterOverrideWrapper("relationshipStateDTO", relationshipStateDTO);
            return _relationshipStateFactory.Create(relationshipStateParameter);
        }

        private List<IRelationshipState> GenerateRelationshipStatesFromRelationshipStatesDTO(IEnumerable<IRelationshipStateDTO> relationshipStateDTOs)
        {
            if (relationshipStateDTOs == null)
            {
                return null;
            }

            return relationshipStateDTOs.Select(GenerateRelationshipStateFromRelationshipStateDTO).ToList();
        }

        // Generate RelationshipAuthorizations
        public IFriendshipAuthorizations GenerateFriendshipAuthorizations(bool retweetsEnabled, bool deviceNotificationEnabled)
        {
            var friendshipAuthorization = _friendshipAuthorizationsFactory.Create();

            friendshipAuthorization.RetweetsEnabled = retweetsEnabled;
            friendshipAuthorization.DeviceNotificationEnabled = deviceNotificationEnabled;

            return friendshipAuthorization;
        }

    }
}