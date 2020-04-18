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
        public async Task TestFollow()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweetinviTestAuthenticated = await _tweetinviTestClient.Users.GetAuthenticatedUser();
            var tweetinviTestUser = await _tweetinviTestClient.Users.GetUser("tweetinvitest");

            var followers = new List<IUser>();
            var followersIterator = tweetinviTestAuthenticated.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.NextPage();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await _tweetinviTestClient.Users.GetUser("tweetinviapi");

            await _tweetinviTestClient.Users.GetFriendIds("tweetinvitest");
            await _tweetinviTestClient.Users.GetFriends("tweetinvitest");

            var friendIdsIterator = _tweetinviTestClient.Users.GetFriendIdsIterator("tweetinvitest");
            var friendIds = await friendIdsIterator.NextPage();
            if (friendIds.Contains(userToFollow.Id))
            {
                await _tweetinviTestClient.Users.UnfollowUser(userToFollow);
            }

            var followerIdsIterator = _tweetinviTestClient.Users.GetFollowerIdsIterator(userToFollow);
            var followerIds = await followerIdsIterator.NextPage();
            if (followerIds.Contains(EndToEndTestConfig.TweetinviTest.UserId))
            {
                await _tweetinviTestClient.Users.UnfollowUser(userToFollow);
            }

            // act
            await _tweetinviTestClient.Users.FollowUser(userToFollow);

            var friendsAfterAdd = await tweetinviTestAuthenticated.GetFriends().NextPage();
            await _tweetinviTestClient.Users.GetFollowerIds(userToFollow);
            var newFollowers = await _tweetinviTestClient.Users.GetFollowers(userToFollow);
            await _tweetinviTestClient.Users.UnfollowUser(userToFollow);

            var friendsAfterRemove = await tweetinviTestAuthenticated.GetFriends().NextPage();

            // assert
            Assert.Equal(1693649419, tweetinviTestUser.Id);
            Assert.NotNull(tweetinviTestAuthenticated);

            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);
            Assert.Contains(newFollowers.Select(x => x.Name), username => username == EndToEndTestConfig.TweetinviTest.AccountId);
        }

        [Fact]
        public async Task TestRelationships()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var authenticatedUser = await _tweetinviTestClient.Users.GetAuthenticatedUser();

            var usernameToFollow = "tweetinviapi";
            var userToFollow = await _tweetinviTestClient.Users.GetUser(usernameToFollow);

            await _tweetinviTestClient.Users.FollowUser(userToFollow);

            var relationshipAfterAdd = await authenticatedUser.GetRelationshipWith(userToFollow);
            var relationshipStateAfterAdd = await _tweetinviTestClient.Users.GetRelationshipsWith(new IUserIdentifier[] { userToFollow });

            await _tweetinviTestClient.Users.UpdateRelationship(new UpdateRelationshipParameters(userToFollow)
            {
                EnableRetweets = false,
                EnableDeviceNotifications = true
            });

            var retweetMutedUsers = await _tweetinviTestClient.Users.GetUserIdsWhoseRetweetsAreMuted();
            var relationshipAfterUpdate = await _tweetinviTestClient.Users.GetRelationshipBetween(authenticatedUser, userToFollow);

            await _tweetinviTestClient.Users.UnfollowUser(userToFollow);

            var relationshipAfterRemove = await _tweetinviTestClient.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterRemove = await _tweetinviTestClient.Users.GetRelationshipsWith(new[] { usernameToFollow });

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

            var publicUser = await _tweetinviTestClient.Users.GetAuthenticatedUser();
            var privateUser = await _protectedClient.Users.GetAuthenticatedUser();

            // act
            await _tweetinviTestClient.Users.FollowUser(privateUser);

            var sentRequestsIterator = _tweetinviTestClient.Users.GetUsersYouRequestedToFollowIterator();
            var sentRequestUsers = await sentRequestsIterator.NextPage();

            var receivedRequestsIterator = _protectedClient.Users.GetUsersRequestingFriendshipIterator();
            var receivedRequestUsers = await receivedRequestsIterator.NextPage();

            // delete ongoing request
            //            await publicUserClient.Account.UnfollowUser(privateUser);
            //
            //            var afterUnfollowSentRequestsIterator = publicUserClient.Account.GetUsersYouRequestedToFollow();
            //            var afterUnfollowSentRequestUsers = await afterUnfollowSentRequestsIterator.NextPage();
            //
            //            var afterUnfollowReceivedRequestsIterator = privateUserClient.Account.GetUsersRequestingFriendship();
            //            var afterUnfollowReceivedRequestUsers = await afterUnfollowReceivedRequestsIterator.NextPage();

            // assert
            Assert.Contains(sentRequestUsers, user => user.Id == privateUser.Id);
            Assert.Contains(receivedRequestUsers, user => user.Id == publicUser.Id);

            //            Assert.DoesNotContain(afterUnfollowSentRequestUsers, user => user.Id == privateUser.Id);
            //            Assert.DoesNotContain(afterUnfollowReceivedRequestUsers, user => user.Id == publicUser.Id);
        }
    }
}