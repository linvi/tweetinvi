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

            var blockedUserIdsIterator = _client.Users.GetBlockedUserIds();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = _client.Users.GetBlockedUsers();
            var blockedUsers = await blockedUsersIterator.MoveToNextPage();

            await userToFollow.UnblockUser();

            // assert
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
        }

        [Fact]
        public async Task TestMute()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var userToMute = await _privateUserClient.Users.GetAuthenticatedUser();

            // act
            var mutedUserIdsIterator = _client.Users.GetMutedUserIds();
            var initialMutedUserIds = await mutedUserIdsIterator.MoveToNextPage();

            await _client.Users.MuteUser(userToMute);
            var newMutedUserIdsIterator = _client.Users.GetMutedUserIds();
            var newMutedUserIds = await newMutedUserIdsIterator.MoveToNextPage();
            var newMutedUsersIterator = _client.Users.GetMutedUsers();
            var newMutedUsers = await newMutedUsersIterator.MoveToNextPage();
            await _client.Users.UnMuteUser(userToMute);

            var restoredMutedUserIdsIterator = _client.Users.GetMutedUserIds();
            var restoredMutedUserIds = await restoredMutedUserIdsIterator.MoveToNextPage();

            // assert
            Assert.True(newMutedUsers.Select(x => x.Id).OfType<long>().ContainsSameObjectsAs(newMutedUserIds));
            Assert.True(restoredMutedUserIds.Select(x => x).ContainsSameObjectsAs(initialMutedUserIds));
        }
    }
}