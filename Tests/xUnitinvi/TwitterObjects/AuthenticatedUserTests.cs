using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Client;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.TwitterObjects
{
    public class AuthenticatedUserTests
    {
        public AuthenticatedUserTests()
        {
            _fakeBuilder = new FakeClassBuilder<AuthenticatedUser>();

            InitData();
        }

        private readonly FakeClassBuilder<AuthenticatedUser> _fakeBuilder;
        private Fake<ITwitterClient> _twitterClient;
        private Fake<ITweetsClient> _tweetsClient;
        private Fake<IUsersClient> _usersClient;

        private IAuthenticatedUser _authenticatedUser;

        private void InitData()
        {
            _twitterClient = new Fake<ITwitterClient>();
            _tweetsClient = new Fake<ITweetsClient>();
            _usersClient = new Fake<IUsersClient>();

            _twitterClient.CallsTo(x => x.Tweets).Returns(_tweetsClient.FakedObject);
            _twitterClient.CallsTo(x => x.Users).Returns(_usersClient.FakedObject);

            _authenticatedUser = _fakeBuilder.GenerateClass();
            _authenticatedUser.Client = _twitterClient.FakedObject;
        }

        // BLOCK

        [Fact]
        public async Task BlockUser_ReturnsAccountClientTaskAsync()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.BlockUserAsync(user);

            // Assert
            _usersClient.CallsTo(x => x.BlockUserAsync(user)).MustHaveHappened();
        }

        [Fact]
        public async Task UnblockUser_ReturnsAccountClientTaskAsync()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.UnblockUserAsync(user);

            // Assert
            _usersClient.CallsTo(x => x.UnblockUserAsync(user)).MustHaveHappened();
        }

        [Fact]
        public async Task ReportUserForSpam_ReturnsAccountClientTaskAsync()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.ReportUserForSpamAsync(user);

            // Assert
            _usersClient.CallsTo(x => x.ReportUserForSpamAsync(user)).MustHaveHappened();
        }

        // FOLLOWERS

        [Fact]
        public async Task FollowUser_ReturnsAccountClientTaskAsync()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.FollowUserAsync(user);

            // Assert
            _usersClient.CallsTo(x => x.FollowUserAsync(user)).MustHaveHappened();
        }

        [Fact]
        public async Task UnfollowUser_ReturnsAccountClientTaskAsync()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.UnfollowUserAsync(user);

            // Assert
            _usersClient.CallsTo(x => x.UnfollowUserAsync(user)).MustHaveHappened();
        }

        // TWEETS

        [Fact]
        public async Task PublishTweetText_UsesTweetsClientAsync()
        {
            // Arrange
            var parameters = A.Fake<IPublishTweetParameters>();
            var expectedResult = A.Fake<ITweet>();

            _tweetsClient.CallsTo(x => x.PublishTweetAsync(parameters)).Returns(expectedResult);

            // Act
            var result = await _authenticatedUser.PublishTweetAsync(parameters);

            // Assert
            Assert.Same(result, expectedResult);
        }
    }
}