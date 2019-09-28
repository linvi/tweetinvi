using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class UserIntegrationTests
    {
        public UserIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
        }

        private readonly ITestOutputHelper _logger;

        //[Fact]
        [Fact(Skip = "IntegrationTests")]
        public async Task TestUsers()
        {
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };

            var credentials = new TwitterCredentials("A", "B", "C", "D");
            var client = new TwitterClient(credentials);

            // act
            var authenticatedUser = await client.Account.GetAuthenticatedUser();
            var tweetinviUser = await client.Users.GetUser("tweetinviapi");

            var followers = new List<IUser>();
            var followersIterator = authenticatedUser.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.MoveToNextPage();
                followers.AddRange(pageFollowers);
            }

            var userToFollow = await client.Users.GetUser("tweetinvitest");

            var friendIdsIterator = client.Users.GetFriendIds("tweetinviapi");
            var friendIds = await friendIdsIterator.MoveToNextPage();
            var friendsBeforeAdd = await client.Users.GetUsers(friendIds);

            await client.Account.FollowUser(userToFollow);

            var relationshipAfterAdd = await client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterAdd = await client.Account.GetRelationshipsWith(new IUserIdentifier[] { userToFollow });

            var friendsAfterAdd = await authenticatedUser.GetFriends().MoveToNextPage();
            await client.Account.UnFollowUser(userToFollow);
            var friendsAfterRemove = await authenticatedUser.GetFriends().MoveToNextPage();

            var relationshipAfterRemove = await client.Users.GetRelationshipBetween(authenticatedUser, userToFollow);
            var relationshipStateAfterRemove = await client.Account.GetRelationshipsWith(new[] { "tweetinvitest" });

            var blockSuccess = await userToFollow.BlockUser();

            var blockedUserIdsIterator = client.Account.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = client.Account.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            var unblockSuccess = await userToFollow.UnBlockUser();


            // assert
            Assert.Equal(tweetinviUser.Id, 1577389800);
            Assert.NotNull(authenticatedUser);

            Assert.NotEmpty(friendsBeforeAdd);
            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);

            Assert.True(relationshipAfterAdd.Following);
            Assert.False(relationshipAfterRemove.Following);

            Assert.True(relationshipStateAfterAdd[userToFollow].Following);
            Assert.False(relationshipStateAfterRemove["tweetinvitest"].Following);

            Assert.True(blockSuccess);
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
            Assert.True(unblockSuccess);
        }

        // [Fact]
        [Fact(Skip = "IntegrationTests")]
        public async Task TestWithPrivateUser()
        {
            var publicUserCredentials = new TwitterCredentials("A", "B", "C", "D");
            var privateUserCredentials = new TwitterCredentials("A", "B", "C", "D");
            
            var publicUserClient = new TwitterClient(publicUserCredentials);
            var privateUserClient = new TwitterClient(privateUserCredentials);

            var publicUser = await publicUserClient.Account.GetAuthenticatedUser();
            var privateUser = await privateUserClient.Account.GetAuthenticatedUser();

            // act
            await publicUserClient.Account.FollowUser(privateUser);
            
            var sentRequestsIterator = publicUserClient.Account.GetUsersYouRequestedToFollow();
            var sentRequestUsers = await sentRequestsIterator.MoveToNextPage();

            var receivedRequestsIterator = privateUserClient.Account.GetUsersRequestingFriendship();
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