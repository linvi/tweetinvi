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
        private readonly ITwitterClient _client;
        private readonly ITwitterClient _privateUserClient;

        public AccountIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;

            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            _client = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials);
            _privateUserClient = new TwitterClient(IntegrationTestConfig.ProtectedUser.Credentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllAccountTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            _logger.WriteLine($"Starting {nameof(TestBlock)}");
            await TestBlock().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestBlock)} succeeded");

            _logger.WriteLine($"Starting {nameof(TestMute)}");
            await TestMute().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(TestMute)} succeeded");
        }

        [Fact]
        public async Task TestBlock()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var userToFollow = await _client.Users.GetUser("tweetinvitest");

            // act
            var blockSuccess = await userToFollow.BlockUser();

            var blockedUserIdsIterator = _client.Account.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = _client.Account.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            var unblockSuccess = await userToFollow.UnBlockUser();

            // assert
            Assert.True(blockSuccess);
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
            Assert.True(unblockSuccess);
        }

        [Fact]
        public async Task TestMute()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var userToMute = await _privateUserClient.Account.GetAuthenticatedUser();

            // act
            var mutedUserIdsIterator = _client.Account.GetMutedUserIds();
            var initialMutedUserIds = await mutedUserIdsIterator.MoveToNextPage();

            await _client.Account.MuteUser(userToMute);
            var newMutedUserIdsIterator = _client.Account.GetMutedUserIds();
            var newMutedUserIds = await newMutedUserIdsIterator.MoveToNextPage();
            var newMutedUsersIterator = _client.Account.GetMutedUsers();
            var newMutedUsers = await newMutedUsersIterator.MoveToNextPage();
            await _client.Account.UnMuteUser(userToMute);

            var restoredMutedUserIdsIterator = _client.Account.GetMutedUserIds();
            var restoredMutedUserIds = await restoredMutedUserIdsIterator.MoveToNextPage();

            // assert
            Assert.True(newMutedUsers.Select(x => x.Id).OfType<long>().ContainsSameObjectsAs(newMutedUserIds));
            Assert.True(restoredMutedUserIds.Select(x => x).ContainsSameObjectsAs(initialMutedUserIds));
        }
    }
}