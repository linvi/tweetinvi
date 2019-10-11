using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    // VERY IMPORTANT NOTE !!!
    // THESE TESTS CANNOT BE RUN IN PARALLEL AS SOME OPERATIONS CAN AFFECT THE STATES IN TWITTER
    // RunIntegrationTests() run each of them one after another

    public class UserIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private ITwitterClient Client { get; }
        private ITwitterClient PrivateUserClient { get; }
        
        public UserIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            
            Client = new TwitterClient(IntegrationTestConfig.TweetinviTestCredentials);
            PrivateUserClient = new TwitterClient(IntegrationTestConfig.ProtectedUserCredentials);
            
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
//        [Fact(Skip = "IntegrationTests")]
        public async Task RunAllUserTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
            {
                return;
            }
            
            _logger.WriteLine($"Starting {nameof(TestFollow)}");
            await TestFollow().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestFollow)} succeeded");
            
            _logger.WriteLine($"Starting {nameof(TestRelationships)}");
            await TestRelationships().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestRelationships)} succeeded");
            
            
            _logger.WriteLine($"Starting {nameof(TestWithPrivateUser)}");
            await TestWithPrivateUser().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestWithPrivateUser)} succeeded");
        }

        private async Task TestFollow()
        {
            // act
            var tweetinviTestAuthenticated = await Client.Account.GetAuthenticatedUser();
            var tweetinviTestUser = await Client.Users.GetUser("tweetinvitest");

            var followers = new List<IUser>();
            var followersIterator = tweetinviTestAuthenticated.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.MoveToNextPage();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await Client.Users.GetUser("tweetinviapi");

            var friendIdsIterator = Client.Users.GetFriendIds("tweetinvitest");
            var friendIds = await friendIdsIterator.MoveToNextPage();
            var friendsBeforeAdd = await Client.Users.GetUsers(friendIds);

            await Client.Account.FollowUser(userToFollow);

            var friendsAfterAdd = await tweetinviTestAuthenticated.GetFriends().MoveToNextPage();
            
            await Client.Account.UnFollowUser(userToFollow);
            
            var friendsAfterRemove = await tweetinviTestAuthenticated.GetFriends().MoveToNextPage();

            // assert
            Assert.Equal(1693649419, tweetinviTestUser.Id);
            Assert.NotNull(tweetinviTestAuthenticated);

            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);
        }

        private async Task TestRelationships()
        {
            // act
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();

            var usernameToFollow = "tweetinviapi";
            var userToFollow = await Client.Users.GetUser(usernameToFollow);
            
            await Client.Account.FollowUser(userToFollow);

            var relationshipAfterAdd = await authenticatedUser.GetRelationshipWith(userToFollow);
            var relationshipStateAfterAdd = await Client.Account.GetRelationshipsWith(new IUserIdentifier[] { userToFollow });

            await Client.Account.UpdateRelationship(new UpdateRelationshipParameters(userToFollow)
            {
                EnableRetweets = false,
                EnableDeviceNotifications = true
            });

            var retweetMutedUsers = await Client.Account.GetUserIdsWhoseRetweetsAreMuted();
            var relationshipAfterUpdate = await Client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);

            await Client.Account.UnFollowUser(userToFollow);
            
            var relationshipAfterRemove = await Client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterRemove = await Client.Account.GetRelationshipsWith(new[] { usernameToFollow });
            
            // assert
            Assert.False(relationshipAfterAdd.NotificationsEnabled);
            Assert.True(relationshipAfterUpdate.NotificationsEnabled);
            
            Assert.True(relationshipAfterAdd.Following);
            Assert.False(relationshipAfterRemove.Following);

            Assert.True(relationshipStateAfterAdd[userToFollow].Following);
            Assert.False(relationshipStateAfterRemove[usernameToFollow].Following);

            Assert.Contains(retweetMutedUsers, x => x == userToFollow.Id);
        }

        private async Task TestWithPrivateUser()
        {
            var publicUser = await Client.Account.GetAuthenticatedUser();
            var privateUser = await PrivateUserClient.Account.GetAuthenticatedUser();

            // act
            await Client.Account.FollowUser(privateUser);
            
            var sentRequestsIterator = Client.Account.GetUsersYouRequestedToFollow();
            var sentRequestUsers = await sentRequestsIterator.MoveToNextPage();

            var receivedRequestsIterator = PrivateUserClient.Account.GetUsersRequestingFriendship();
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