using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public UserEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task TestFollowAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweetinviTestAuthenticated = await _tweetinviTestClient.Users.GetAuthenticatedUserAsync();
            var tweetinviTestUser = await _tweetinviTestClient.Users.GetUserAsync("tweetinvitest");

            var followers = new List<IUser>();
            var followersIterator = tweetinviTestAuthenticated.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.NextPageAsync();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await _tweetinviTestClient.Users.GetUserAsync("tweetinviapi");

            await _tweetinviTestClient.Users.GetFriendIdsAsync("tweetinvitest");
            await _tweetinviTestClient.Users.GetFriendsAsync("tweetinvitest");

            var friendIdsIterator = _tweetinviTestClient.Users.GetFriendIdsIterator("tweetinvitest");
            var friendIds = await friendIdsIterator.NextPageAsync();
            if (friendIds.Contains(userToFollow.Id))
            {
                await _tweetinviTestClient.Users.UnfollowUserAsync(userToFollow);
            }

            var followerIdsIterator = _tweetinviTestClient.Users.GetFollowerIdsIterator(userToFollow);
            var followerIds = await followerIdsIterator.NextPageAsync();
            if (followerIds.Contains(EndToEndTestConfig.TweetinviTest.UserId))
            {
                await _tweetinviTestClient.Users.UnfollowUserAsync(userToFollow);
            }

            // act
            await _tweetinviTestClient.Users.FollowUserAsync(userToFollow);

            var friendsAfterAdd = await tweetinviTestAuthenticated.GetFriends().NextPageAsync();
            await _tweetinviTestClient.Users.GetFollowerIdsAsync(userToFollow);
            var newFollowers = await _tweetinviTestClient.Users.GetFollowersAsync(userToFollow);
            await _tweetinviTestClient.Users.UnfollowUserAsync(userToFollow);

            var friendsAfterRemove = await tweetinviTestAuthenticated.GetFriends().NextPageAsync();

            // assert
            Assert.Equal(1693649419, tweetinviTestUser.Id);
            Assert.NotNull(tweetinviTestAuthenticated);

            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);
            Assert.Contains(newFollowers.Select(x => x.Name), username => username == EndToEndTestConfig.TweetinviTest.AccountId);
        }

        [Fact]
        public async Task TestRelationshipsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var authenticatedUser = await _tweetinviTestClient.Users.GetAuthenticatedUserAsync();

            var usernameToFollow = "tweetinviapi";
            var userToFollow = await _tweetinviTestClient.Users.GetUserAsync(usernameToFollow);

            await _tweetinviTestClient.Users.FollowUserAsync(userToFollow);

            var relationshipAfterAdd = await authenticatedUser.GetRelationshipWithAsync(userToFollow);
            var relationshipStateAfterAdd = await _tweetinviTestClient.Users.GetRelationshipsWithAsync(new IUserIdentifier[] { userToFollow });

            await _tweetinviTestClient.Users.UpdateRelationshipAsync(new UpdateRelationshipParameters(userToFollow)
            {
                EnableRetweets = false,
                EnableDeviceNotifications = true
            });

            var retweetMutedUsers = await _tweetinviTestClient.Users.GetUserIdsWhoseRetweetsAreMutedAsync();
            var relationshipAfterUpdate = await _tweetinviTestClient.Users.GetRelationshipBetweenAsync(authenticatedUser, userToFollow);

            await _tweetinviTestClient.Users.UnfollowUserAsync(userToFollow);

            var relationshipAfterRemove = await _tweetinviTestClient.Users.GetRelationshipBetweenAsync(authenticatedUser, userToFollow);
            var relationshipStateAfterRemove = await _tweetinviTestClient.Users.GetRelationshipsWithAsync(new[] { usernameToFollow });

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
        public async Task TestWithPrivateUserAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests) { return; }

            var publicUser = await _tweetinviTestClient.Users.GetAuthenticatedUserAsync();
            var privateUser = await _protectedClient.Users.GetAuthenticatedUserAsync();

            // act
            await _tweetinviTestClient.Users.FollowUserAsync(privateUser);

            var sentRequestsIterator = _tweetinviTestClient.Users.GetUsersYouRequestedToFollowIterator();
            var sentRequestUsers = await sentRequestsIterator.NextPageAsync();

            var receivedRequestsIterator = _protectedClient.Users.GetUsersRequestingFriendshipIterator();
            var receivedRequestUsers = await receivedRequestsIterator.NextPageAsync();

            // delete ongoing request
            //            await publicUserClient.Account.UnfollowUserAsync(privateUser);
            //
            //            var afterUnfollowSentRequestsIterator = publicUserClient.Account.GetUsersYouRequestedToFollow();
            //            var afterUnfollowSentRequestUsers = await afterUnfollowSentRequestsIterator.NextPageAsync();
            //
            //            var afterUnfollowReceivedRequestsIterator = privateUserClient.Account.GetUsersRequestingFriendship();
            //            var afterUnfollowReceivedRequestUsers = await afterUnfollowReceivedRequestsIterator.NextPageAsync();

            // assert
            Assert.Contains(sentRequestUsers, user => user.Id == privateUser.Id);
            Assert.Contains(receivedRequestUsers, user => user.Id == publicUser.Id);

            //            Assert.DoesNotContain(afterUnfollowSentRequestUsers, user => user.Id == privateUser.Id);
            //            Assert.DoesNotContain(afterUnfollowReceivedRequestUsers, user => user.Id == publicUser.Id);
        }
    }
}