using System;
using System.Collections.Generic;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Controllers.Friendship
{
    public interface IFriendshipJsonController
    {
        IEnumerable<string> GetUserIdsRequestingFriendship();
        IEnumerable<string> GetUserIdsYouRequestedToFollow();

        // Create Friendship with
        string CreateFriendshipWith(IUserIdentifier user);
        string CreateFriendshipWith(long userId);
        string CreateFriendshipWith(string userScreeName);

        // Destroy Friendship with
        string DestroyFriendshipWith(IUserIdentifier user);
        string DestroyFriendshipWith(long userId);
        string DestroyFriendshipWith(string userScreeName);

        // Update Friendship Authorizations
        string UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled);
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

        public string CreateFriendshipWith(IUserIdentifier user)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(user);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string CreateFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(new UserIdentifier(userId));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string CreateFriendshipWith(string userScreeName)
        {
            string query = _friendshipQueryGenerator.GetCreateFriendshipWithQuery(new UserIdentifier(userScreeName));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }


        public string DestroyFriendshipWith(IUserIdentifier user)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(user);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string DestroyFriendshipWith(long userId)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(new UserIdentifier(userId));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string DestroyFriendshipWith(string userScreeName)
        {
            string query = _friendshipQueryGenerator.GetDestroyFriendshipWithQuery(new UserIdentifier(userScreeName));
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }


        public string UpdateRelationshipAuthorizationsWith(IUserIdentifier user, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(user, friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string UpdateRelationshipAuthorizationsWith(long userId, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(new UserIdentifier(userId), friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }

        public string UpdateRelationshipAuthorizationsWith(string userScreenName, bool retweetsEnabled, bool deviceNotifictionEnabled)
        {
            var friendshipAuthorizations = _friendshipFactory.GenerateFriendshipAuthorizations(retweetsEnabled, deviceNotifictionEnabled);
            string query = _friendshipQueryGenerator.GetUpdateRelationshipAuthorizationsWithQuery(new UserIdentifier(userScreenName), friendshipAuthorizations);
            return _twitterAccessor.ExecutePOSTQueryReturningJson(query);
        }
    }
}