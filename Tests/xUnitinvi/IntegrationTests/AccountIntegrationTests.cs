using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class AccountIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private ITwitterClient Client { get; }
        private ITwitterClient PrivateUserClient { get; }
        
        public AccountIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            
            _logger.WriteLine(DateTime.Now.ToLongTimeString());
            
            Client = new TwitterClient(IntegrationTestCredentials.NormalUserCredentials);
            PrivateUserClient = new TwitterClient(IntegrationTestCredentials.ProtectedUserCredentials);
            
            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }
        
//        [Fact]
        [Fact(Skip = "IntegrationTests")]
        public async Task RunAllAccountTests()
        {
            _logger.WriteLine($"Starting {nameof(TestBlock)}");
            await TestBlock().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestBlock)} succeeded");
            
            _logger.WriteLine($"Starting {nameof(TestMute)}");
            await TestMute().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestMute)} succeeded");
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
        
        [Fact(Skip = "IntegrationTests")]
        private async Task TestMute()
        {
            var userToMute = await PrivateUserClient.Account.GetAuthenticatedUser();
            
            // act
            var mutedUserIdsIterator = Client.Account.GetMutedUserIds();
            var initialMutedUserIds = await mutedUserIdsIterator.MoveToNextPage();

            await Client.Account.MuteUser(userToMute);
            var newMutedUserIdsIterator = Client.Account.GetMutedUserIds();
            var newMutedUserIds = await newMutedUserIdsIterator.MoveToNextPage();
            var newMutedUsersIterator = Client.Account.GetMutedUsers();
            var newMutedUsers = await newMutedUsersIterator.MoveToNextPage();
            await Client.Account.UnMuteUser(userToMute);
            
            var restoredMutedUserIdsIterator = Client.Account.GetMutedUserIds();
            var restoredMutedUserIds = await restoredMutedUserIdsIterator.MoveToNextPage();
            
            // assert
            Assert.True(newMutedUsers.Select(x => x.Id).OfType<long>().ContainsSameObjectsAs(newMutedUserIds));
            Assert.True(restoredMutedUserIds.Select(x => x).ContainsSameObjectsAs(initialMutedUserIds));
        }
    }
}