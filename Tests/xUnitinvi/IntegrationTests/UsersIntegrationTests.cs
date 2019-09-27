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

            var userToFollow = await client.Users.GetUser("artwolkt");

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
            var relationshipStateAfterRemove = await client.Account.GetRelationshipsWith(new [] { "artwolkt" });

            var blockSuccess = await userToFollow.BlockUser();

            var blockedUserIdsIterator = client.Account.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = client.Account.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            var unblockSuccess = await userToFollow.UnBlockUser();


            // assert
            Assert.Equal(tweetinviUser.Id, 1577389800);
            Assert.NotNull(authenticatedUser);
            Assert.Contains(1693649419, friendIds);
            
            Assert.Contains(friendsBeforeAdd, item => { return item.ScreenName == "tweetinvitest"; });
            Assert.DoesNotContain(friendsBeforeAdd, friend => friend.Id == userToFollow.Id);
            Assert.Contains(friendsAfterAdd, friend => friend.Id == userToFollow.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == userToFollow.Id);

            Assert.True(relationshipAfterAdd.Following);
            Assert.False(relationshipAfterRemove.Following);

            Assert.True(relationshipStateAfterAdd[userToFollow].Following);
            Assert.False(relationshipStateAfterRemove["artwolkt"].Following);

            Assert.True(blockSuccess);
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
            Assert.True(unblockSuccess);
        }
    }
}