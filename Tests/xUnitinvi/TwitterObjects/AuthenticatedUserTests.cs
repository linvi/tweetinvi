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

        [Fact]
        public async Task FollowUser_ReturnsUsersClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();
            _usersClient.CallsTo(x => x.FollowUser(user)).Returns(true);

            // Act
            var result = await _authenticatedUser.FollowUser(user);

            // Assert
            Assert.True(result);
            _usersClient.CallsTo(x => x.FollowUser(user)).MustHaveHappened();
        }

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

        [Fact]
        public async Task UnFollowUser_ReturnsUsersClientTask()
        {
            // Arrange
            var user = A.Fake<IUserIdentifier>();
            _usersClient.CallsTo(x => x.UnFollowUser(user)).Returns(true);

            // Act
            var result = await _authenticatedUser.UnFollowUser(user);

            // Assert
            Assert.True(result);
            _usersClient.CallsTo(x => x.UnFollowUser(user)).MustHaveHappened();
        }
    }
}