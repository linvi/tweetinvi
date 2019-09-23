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
            var authenticatedUser = await client.Users.GetAuthenticatedUser();
            var tweetinviUser = await client.Users.GetUser("tweetinviapi");

            var followers = new List<IUser>();
            var followersIterator = authenticatedUser.GetFollowers();

            while (!followersIterator.Completed)
            {
                var pageFollowers = await followersIterator.MoveToNextPage();
                followers.AddRange(pageFollowers);
            }

            var artwolktUser = await client.Users.GetUser("artwolkt");

            var friendIdsIterator = client.Users.GetFriendIds("tweetinviapi");
            var friendIds = await friendIdsIterator.MoveToNextPage();
            var friendsBeforeAdd = await client.Users.GetUsers(friendIds);

            await client.Users.FollowUser(artwolktUser);
            var friendsAfterAdd = await authenticatedUser.GetFriends().MoveToNextPage();
            await client.Users.UnFollowUser(artwolktUser);
            var friendsAfterRemove = await authenticatedUser.GetFriends().MoveToNextPage();

            var blockSuccess = await artwolktUser.BlockUser();

            var blockedUserIdsIterator = client.Users.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = client.Users.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            var unblockSuccess = await artwolktUser.UnBlockUser();


            // assert
            Assert.Equal(tweetinviUser.Id, 1577389800);
            Assert.NotNull(authenticatedUser);
            Assert.Contains(1693649419, friendIds);
            Assert.Contains(friendsBeforeAdd, item => { return item.ScreenName == "tweetinvitest"; });


            Assert.DoesNotContain(friendsBeforeAdd, friend => friend.Id == artwolktUser.Id);
            Assert.Contains(friendsAfterAdd, friend => friend.Id == artwolktUser.Id);
            Assert.DoesNotContain(friendsAfterRemove, friend => friend.Id == artwolktUser.Id);

            Assert.True(blockSuccess);
            Assert.Contains(blockedUsersFromIdsIterator, id => id == artwolktUser.Id);
            Assert.Contains(blockedUsers, user => user.Id == artwolktUser.Id);
            Assert.True(unblockSuccess);
        }
    }
}