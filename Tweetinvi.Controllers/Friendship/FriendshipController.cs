using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;

namespace Tweetinvi.Controllers.Friendship
{
    public class FriendshipController : IFriendshipController
    {
        private readonly IFriendshipQueryExecutor _friendshipQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IFactory<IFriendshipAuthorizations> _friendshipAuthorizationsFactory;

        public FriendshipController(
            IFriendshipQueryExecutor friendshipQueryExecutor,
            IUserFactory userFactory,
            IFactory<IFriendshipAuthorizations> friendshipAuthorizationsFactory)
        {
            _friendshipQueryExecutor = friendshipQueryExecutor;
            _userFactory = userFactory;
            _friendshipAuthorizationsFactory = friendshipAuthorizationsFactory;
        }
        
        // Get Users not authorized to retweet
        public Task<long[]> GetUserIdsWhoseRetweetsAreMuted()
        {
            return _friendshipQueryExecutor.GetUserIdsWhoseRetweetsAreMuted();
        }

        public async Task<IEnumerable<IUser>> GetUsersWhoseRetweetsAreMuted()
        {
            var userIds = await GetUserIdsWhoseRetweetsAreMuted();
            return await _userFactory.GetUsersFromIds(userIds);
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