using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class AccountEndToEndTests : TweetinviTest
    {
        public AccountEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task TestBlockAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var userToFollow = await _tweetinviTestClient.Users.GetUserAsync(EndToEndTestConfig.ProtectedUser.AccountId);

            // act
            await userToFollow.BlockUserAsync();

            await _tweetinviTestClient.Users.GetBlockedUserIdsAsync();
            await _tweetinviTestClient.Users.GetBlockedUsersAsync();

            var blockedUserIdsIterator = _tweetinviTestClient.Users.GetBlockedUserIdsIterator();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.NextPageAsync();
            var blockedUsersIterator = _tweetinviTestClient.Users.GetBlockedUsersIterator();
            var blockedUsers = await blockedUsersIterator.NextPageAsync();

            await userToFollow.UnblockUserAsync();

            // assert
            Assert.Contains(blockedUsersFromIdsIterator, id => id == userToFollow.Id);
            Assert.Contains(blockedUsers, user => user.Id == userToFollow.Id);
        }

        [Fact]
        public async Task TestMuteAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var userToMute = await _protectedClient.Users.GetAuthenticatedUserAsync();

            // act
            var mutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var initialMutedUserIds = await mutedUserIdsIterator.NextPageAsync();

            await _tweetinviTestClient.Users.MuteUserAsync(userToMute);

            await _tweetinviTestClient.Users.GetMutedUserIdsAsync();
            await _tweetinviTestClient.Users.GetMutedUsersAsync();

            var newMutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var newMutedUserIds = await newMutedUserIdsIterator.NextPageAsync();
            var newMutedUsersIterator = _tweetinviTestClient.Users.GetMutedUsersIterator();
            var newMutedUsers = await newMutedUsersIterator.NextPageAsync();
            await _tweetinviTestClient.Users.UnmuteUserAsync(userToMute);

            var restoredMutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var restoredMutedUserIds = await restoredMutedUserIdsIterator.NextPageAsync();

            // assert
            Assert.True(newMutedUsers.Select(x => x.Id).ContainsSameObjectsAs(newMutedUserIds));
            Assert.True(restoredMutedUserIds.Select(x => x).ContainsSameObjectsAs(initialMutedUserIds));
        }
    }
}