using System;
using System.Collections.Generic;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipJsonController
    {
        IEnumerable<string> GetUserIdsRequestingFriendship();
        IEnumerable<string> GetUserIdsYouRequestedToFollow();

        // Create Friendship with
        string CreateFriendshipWith(IUser user);
        string CreateFriendshipWith(IUserIdentifier userDTO);
        string CreateFriendshipWith(long userId);
        string CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        string DestroyFriendshipWith(IUser user);
        string DestroyFriendshipWith(IUserIdentifier userDTO);
        string DestroyFriendshipWith(long userId);
        string DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        string UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled);
        string UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, bool retweetsEnabled, bool deviceNotifictionEnabled);
        string UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled);
        string UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled);
    }

    public class FriendshipJsonController : IFriendshipJsonController
    {
        private readonly IFriendshipQueryGenerator _friendshipQueryGenerator;
        private readonly IFriendshipFactory _friendshipFactory;
        private readonly ITwitterAccessor _twitterAccessor;

        public FriendshipJsonController(
            IFriendshipQueryGenerator friendshipQueryGenerator,
            IFriendshipFactory friendshipFactory,
            ITwitterAccessor twitterAccessor)
        {
            _friendshipQueryGenerator = friendshipQueryGenerator;
            _friendshipFactory = friendshipFactory;
            _twitterAccessor = twitterAccessor;
        }

        public IEnumerable<string> GetUserIdsRequestingFriendship()
        {
            string query = _friendshipQueryGenerator.GetUserIdsRequestingFriendshipQuery();
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public IEnumerable<string> GetUserIdsYouRequestedToFollow()
        {
            string query = _friendshipQueryGenerator.GetUserIdsYouRequestedToFollowQuery();
            return _twitterAccessor.ExecuteJsonCursorGETQuery<IIdsCursorQueryResultDTO>(query);
        }

        public string CreateFriendshipWith(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return CreateFriendshipWith(user.UserDTO);
        }

        public string CreateFriendshipWith(IUserIdentifier userDTO)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string CreateFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string CreateFriendshipWith(string userScreeName)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(userScreeName);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroyFriendshipWith(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return DestroyFriendshipWith(user.UserDTO);
        }

        public string DestroyFriendshipWith(IUserIdentifier userDTO)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userDTO);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroyFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userId);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string DestroyFriendshipWith(string userScreeName)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(userScreeName);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UpdateRelationshipAuthorizationsWith(IUser user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            return UpdateRelationshipAuthorizationsWith(user.UserDTO, retweetsEnabled, deviceNotifictionEnabled);
        }

        public string UpdateRelationshipAuthorizationsWith(IUserIdentifier userDTO, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userDTO, friendshipAuthorizations);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userId, friendshipAuthorizations);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }

        public string UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(userScreenName, friendshipAuthorizations);
            return _twitterAccessor.ExecuteJsonPOSTQuery(query);
        }
    }
}