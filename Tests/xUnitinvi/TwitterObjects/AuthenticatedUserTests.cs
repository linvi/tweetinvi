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
        private Fake<IAccountClient> _accountClient;
        private Fake<ITweetsClient> _tweetsClient;
        private Fake<IUsersClient> _usersClient;

        private IAuthenticatedUser _authenticatedUser;

        private void InitData()
        {
            _twitterClient = new Fake<ITwitterClient>();
            _accountClient = new Fake<IAccountClient>();
            _tweetsClient = new Fake<ITweetsClient>();
            _usersClient = new Fake<IUsersClient>();

            _twitterClient.CallsTo(x => x.Account).Returns(_accountClient.FakedObject);
            _twitterClient.CallsTo(x => x.Tweets).Returns(_tweetsClient.FakedObject);
            _twitterClient.CallsTo(x => x.Users).Returns(_usersClient.FakedObject);

            _authenticatedUser = _fakeBuilder.GenerateClass();
            _authenticatedUser.Client = _twitterClient.FakedObject;
        }

        // BLOCK

        [Fact]
        public async Task BlockUser_ReturnsAccountClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.BlockUser(user);

            // Assert
            _accountClient.CallsTo(x => x.BlockUser(user)).MustHaveHappened();
        }

        [Fact]
        public async Task UnblockUser_ReturnsAccountClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.UnBlockUser(user);

            // Assert
            _accountClient.CallsTo(x => x.UnBlockUser(user)).MustHaveHappened();
        }

        [Fact]
        public async Task ReportUserForSpam_ReturnsAccountClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.ReportUserForSpam(user);

            // Assert
            _accountClient.CallsTo(x => x.ReportUserForSpam(user)).MustHaveHappened();
        }

        // FOLLOWERS

        [Fact]
        public async Task FollowUser_ReturnsAccountClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.FollowUser(user);

            // Assert
            _accountClient.CallsTo(x => x.FollowUser(user)).MustHaveHappened();
        }

        [Fact]
        public async Task UnFollowUser_ReturnsAccountClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();

            // Act
            await _authenticatedUser.UnFollowUser(user);

            // Assert
            _accountClient.CallsTo(x => x.UnFollowUser(user)).MustHaveHappened();
        }

        // TWEETS

        [Fact]
        public async Task PublishTweetText_UsesTweetsClient()
        {
            // Arrange
            var parameters = A.Fake<IPublishTweetParameters>();
            var expectedResult = A.Fake<ITweet>();

            _tweetsClient.CallsTo(x => x.PublishTweet(parameters)).Returns(expectedResult);

            // Act
            var result = await _authenticatedUser.PublishTweet(parameters);

            // Assert
            Assert.Same(result, expectedResult);
        }
    }
}