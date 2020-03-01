using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class UserEndToEndTests : TweetinviTest
    {
        private readonly ITwitterClient _client;
        private readonly ITwitterClient _privateUserClient;

        public UserEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
            _client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials);
            _privateUserClient = new TwitterClient(EndToEndTestConfig.ProtectedUser.Credentials);
        }

        [Fact]
        public async Task TestFollow()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweetinviTestAuthenticated = await _client.Users.GetAuthenticatedUser();
            var tweetinviTestUser = await _client.Users.GetUser("tweetinvitest");

            var followers = new List<IUser>();
            var followersIterator = tweetinviTestAuthenticated.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.MoveToNextPage();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await _client.Users.GetUser("tweetinviapi");

            var friendIdsIterator = _client.Users.GetFriendIds("tweetinvitest");
            var friendIds = await friendIdsIterator.MoveToNextPage();

            if (userToFollow.Id != null && friendIds.Contains(userToFollow.Id.Value))
            {
                await _client.Users.UnFollowUser(userToFollow);
            }

            await _client.Users.FollowUser(userToFollow);

            var friendsAfterAdd = await tweetinviTestAuthenticated.GetFriends().MoveToNextPage();

            await _client.Users.UnFollowUser(userToFollow);

            var friendsAfterRemove = await tweetinviTestAuthenticated.GetFriends().MoveToNextPage();

            // assert
            Assert.Equal(1693649419, tweetinviTestUser.Id);
            Assert.NotNull(tweetinviTestAuthenticated);

            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);
        }

        [Fact]
        public async Task TestRelationships()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var authenticatedUser = await _client.Users.GetAuthenticatedUser();

            var usernameToFollow = "tweetinviapi";
            var userToFollow = await _client.Users.GetUser(usernameToFollow);

            await _client.Users.FollowUser(userToFollow);

            var relationshipAfterAdd = await authenticatedUser.GetRelationshipWith(userToFollow);
            var relationshipStateAfterAdd = await _client.Users.GetRelationshipsWith(new IUserIdentifier[] {userToFollow});

            await _client.Users.UpdateRelationship(new UpdateRelationshipParameters(userToFollow)
            {
                EnableRetweets = false,
                EnableDeviceNotifications = true
            });

            var retweetMutedUsers = await _client.Users.GetUserIdsWhoseRetweetsAreMuted();
            var relationshipAfterUpdate = await _client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);

            await _client.Users.UnFollowUser(userToFollow);

            var relationshipAfterRemove = await _client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterRemove = await _client.Users.GetRelationshipsWith(new[] {usernameToFollow});

            // assert
            Assert.False(relationshipAfterAdd.NotificationsEnabled);
            Assert.True(relationshipAfterUpdate.NotificationsEnabled);

            Assert.True(relationshipAfterAdd.Following);
            Assert.False(relationshipAfterRemove.Following);

            Assert.True(relationshipStateAfterAdd[userToFollow].Following);
            Assert.False(relationshipStateAfterRemove[usernameToFollow].Following);

            Assert.Contains(retweetMutedUsers, x => x == userToFollow.Id);
        }

        [Fact]
        public async Task TestWithPrivateUser()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests) { return; }

            var publicUser = await _client.Users.GetAuthenticatedUser();
            var privateUser = await _privateUserClient.Users.GetAuthenticatedUser();

            // act
            await _client.Users.FollowUser(privateUser);

            var sentRequestsIterator = _client.Users.GetUsersYouRequestedToFollow();
            var sentRequestUsers = await sentRequestsIterator.MoveToNextPage();

            var receivedRequestsIterator = _privateUserClient.Users.GetUsersRequestingFriendship();
            var receivedRequestUsers = await receivedRequestsIterator.MoveToNextPage();

            // delete ongoing request
//            await publicUserClient.Account.UnFollowUser(privateUser);
//
//            var afterUnfollowSentRequestsIterator = publicUserClient.Account.GetUsersYouRequestedToFollow();
//            var afterUnfollowSentRequestUsers = await afterUnfollowSentRequestsIterator.MoveToNextPage();
//
//            var afterUnfollowReceivedRequestsIterator = privateUserClient.Account.GetUsersRequestingFriendship();
//            var afterUnfollowReceivedRequestUsers = await afterUnfollowReceivedRequestsIterator.MoveToNextPage();

            // assert
            Assert.Contains(sentRequestUsers, user => user.Id == privateUser.Id);
            Assert.Contains(receivedRequestUsers, user => user.Id == publicUser.Id);

//            Assert.DoesNotContain(afterUnfollowSentRequestUsers, user => user.Id == privateUser.Id);
//            Assert.DoesNotContain(afterUnfollowReceivedRequestUsers, user => user.Id == publicUser.Id);
        }
    }
}