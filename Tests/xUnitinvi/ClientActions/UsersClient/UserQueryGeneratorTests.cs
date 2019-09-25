using System;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.UsersClient
{
    public class UserQueryGeneratorTests
    {
        public UserQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<UserQueryGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
        }

        private readonly FakeClassBuilder<UserQueryGenerator> _fakeBuilder;
        private readonly Fake<IUserQueryValidator> _fakeUserQueryValidator;

        private UserQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetBlockedUserIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetBlockedUserIdsParameters
            {
                PageSize = 42,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetBlockedUserIdsQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/blocks/ids.json?count=42&hello=world");
        }

        [Fact]
        public void GetBlockUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new BlockUserParameters(user)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetBlockUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/blocks/create.json?user_id=42&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }

        [Fact]
        public void GetFollowUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new FollowUserParameters(user)
            {
                EnableNotifications = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetFollowUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/create.json?user_id=42&follow=true&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }

        [Fact]
        public void GetUnblockUserQuery_WithValidUserDTO_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new UnblockUserParameters(user)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUnblockUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/blocks/destroy.json?user_id=42&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }

        [Fact]
        public void GetUnFollowUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new UnFollowUserParameters(user)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUnFollowUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/destroy.json?user_id=42&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }

        [Fact]
        public void ReportUserForSpamQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();
            var user = new UserIdentifier(42);

            var parameters = new ReportUserForSpamParameters(user)
            {
                PerformBlock = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetReportUserForSpamQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/users/report_spam.json?user_id=42&perform_block=false&hello=world");

            _fakeUserQueryValidator.CallsTo(x => x.ThrowIfUserCannotBeIdentified(user)).MustHaveHappened();
        }

        [Fact]
        public void DownloadProfileImageURL_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetProfileImageParameters("https://url_normal.jpg")
            {
                ImageSize = ImageSize.bigger,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.DownloadProfileImageURL(parameters);

            // Assert
            Assert.Equal(result, $"https://url_bigger.jpg?hello=world");
        }
    }
}