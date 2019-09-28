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
        private ITwitterClient Client { get; }
        private ITwitterClient PrivateUserClient { get; }
        
        public UserIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            
            var credentials = new TwitterCredentials("A", "B", "C", "D");
            var privateUserCredentials = new TwitterCredentials("A", "B", "C", "D");
            
            Client = new TwitterClient(credentials);
            PrivateUserClient = new TwitterClient(privateUserCredentials);
        }

        private readonly ITestOutputHelper _logger;

        //[Fact]
        [Fact(Skip = "IntegrationTests")]
        public async Task RunIntegrationTests()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };

            _logger.WriteLine($"Starting {nameof(TestFollow)}");
            await TestFollow();
            _logger.WriteLine($"{nameof(TestFollow)} succeeded");
            
            _logger.WriteLine($"Starting {nameof(TestRelationships)}");
            await TestRelationships();
            _logger.WriteLine($"{nameof(TestRelationships)} succeeded");
            
            
            _logger.WriteLine($"Starting {nameof(TestWithPrivateUser)}");
            await TestWithPrivateUser();
            _logger.WriteLine($"{nameof(TestWithPrivateUser)} succeeded");
            
            _logger.WriteLine($"Starting {nameof(TestBlock)}");
            await TestBlock();
            _logger.WriteLine($"{nameof(TestBlock)} succeeded");
        }

        [Fact(Skip = "IntegrationTests")]
        private async Task TestFollow()
        {
            // act
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();
            var tweetinviUser = await Client.Users.GetUser("tweetinviapi");

            var followers = new List<IUser>();
            var followersIterator = authenticatedUser.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.MoveToNextPage();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await Client.Users.GetUser("tweetinvitest");

            var friendIdsIterator = Client.Users.GetFriendIds("tweetinviapi");
            var friendIds = await friendIdsIterator.MoveToNextPage();
            var friendsBeforeAdd = await Client.Users.GetUsers(friendIds);

            await Client.Account.FollowUser(userToFollow);

            var friendsAfterAdd = await authenticatedUser.GetFriends().MoveToNextPage();
            
            await Client.Account.UnFollowUser(userToFollow);
            
            var friendsAfterRemove = await authenticatedUser.GetFriends().MoveToNextPage();

            // assert
            Assert.Equal(1577389800, tweetinviUser.Id);
            Assert.NotNull(authenticatedUser);

            Assert.NotEmpty(friendsBeforeAdd);
            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);
        }

        [Fact(Skip = "IntegrationTests")]
        private async Task TestBlock()
        {
            var userToFollow = await Client.Users.GetUser("tweetinvitest");
            
            // act
            var blockSuccess = await userToFollow.BlockUser();

            var blockedUserIdsIterator = Client.Account.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = Client.Account.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            var unblockSuccess = await userToFollow.UnBlockUser();
            
            // assert
            Assert.True(blockSuccess);
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
            Assert.True(unblockSuccess);
        }

        [Fact]
        private async Task TestRelationships()
        {
            // act
            var authenticatedUser = await Client.Account.GetAuthenticatedUser();

            var usernameToFollow = "tweetinvitest";
            var userToFollow = await Client.Users.GetUser(usernameToFollow);
            
            await Client.Account.FollowUser(userToFollow);

            var relationshipAfterAdd = await Client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterAdd = await Client.Account.GetRelationshipsWith(new IUserIdentifier[] { userToFollow });

            await Client.Account.UpdateRelationship(new UpdateRelationshipParameters(userToFollow)
            {
                EnableDeviceNotifications = true
            });
            
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
        }

        [Fact(Skip = "IntegrationTests")]
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