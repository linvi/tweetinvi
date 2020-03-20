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
        public async Task TestBlock()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var userToFollow = await _tweetinviTestClient.Users.GetUser(EndToEndTestConfig.ProtectedUser.AccountId);

            // act
            await userToFollow.BlockUser();

            await _tweetinviTestClient.Users.GetBlockedUserIds();
            await _tweetinviTestClient.Users.GetBlockedUsers();

            var blockedUserIdsIterator = _tweetinviTestClient.Users.GetBlockedUserIdsIterator();
            var blockedUsersFromIdsIterator = await blockedUserIdsIterator.MoveToNextPage();
            var blockedUsersIterator = _tweetinviTestClient.Users.GetBlockedUsersIterator();
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

            var userToMute = await _protectedClient.Users.GetAuthenticatedUser();

            // act
            var mutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var initialMutedUserIds = await mutedUserIdsIterator.MoveToNextPage();

            await _tweetinviTestClient.Users.MuteUser(userToMute);

            await _tweetinviTestClient.Users.GetMutedUserIds();
            await _tweetinviTestClient.Users.GetMutedUsers();

            var newMutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var newMutedUserIds = await newMutedUserIdsIterator.MoveToNextPage();
            var newMutedUsersIterator = _tweetinviTestClient.Users.GetMutedUsersIterator();
            var newMutedUsers = await newMutedUsersIterator.MoveToNextPage();
            await _tweetinviTestClient.Users.UnmuteUser(userToMute);

            var restoredMutedUserIdsIterator = _tweetinviTestClient.Users.GetMutedUserIdsIterator();
            var restoredMutedUserIds = await restoredMutedUserIdsIterator.MoveToNextPage();

            // assert
            Assert.True(newMutedUsers.Select(x => x.Id).ContainsSameObjectsAs(newMutedUserIds));
            Assert.True(restoredMutedUserIds.Select(x => x).ContainsSameObjectsAs(initialMutedUserIds));
        }
    }
}