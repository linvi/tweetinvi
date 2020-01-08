using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class AccountEndToEndTests : TweetinviTest
    {
        private readonly ITwitterClient _client;
        private readonly ITwitterClient _privateUserClient;

        public AccountEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
            _client = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials);
            _privateUserClient = new TwitterClient(EndToEndTestConfig.ProtectedUser.Credentials);
        }

        [Fact]
        public async Task TestBlock()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var userToFollow = await _client.Users.GetUser(EndToEndTestConfig.ProtectedUser.AccountId);

            // act
            await userToFollow.BlockUser();

            var blockedUserIdsIterator = _client.Account.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = _client.Account.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            await userToFollow.UnBlockUser();

            // assert
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
        }

        [Fact]
        public async Task TestMute()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
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