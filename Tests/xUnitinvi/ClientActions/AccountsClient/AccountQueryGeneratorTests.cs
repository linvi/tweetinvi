using System;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Controllers.Account;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.AccountsClient
{
    public class AccountQueryGeneratorTests
    {
        public AccountQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<AccountQueryGenerator>();
            _fakeUserQueryValidator = _fakeBuilder.GetFake<IUserQueryValidator>();
        }

        private readonly FakeClassBuilder<AccountQueryGenerator> _fakeBuilder;
        private readonly Fake<IUserQueryValidator> _fakeUserQueryValidator;

        private AccountQueryGenerator CreateAccountQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetAuthenticatedUserQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateAccountQueryGenerator();
            var parameters = new GetAuthenticatedUserParameters
            {
                IncludeEmail = true,
                IncludeEntities = true,
                SkipStatus = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetAuthenticatedUserQuery(parameters, TweetMode.Extended);

            // assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/account/verify_credentials.json?skip_status=true&include_entities=true&include_email=true&tweet_mode=extended&hello=world");
        }

        // BLOCK

        [Fact]
        public void GetBlockedUserIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            var parameters = new GetBlockedUserIdsParameters
            {
                PageSize = 42,
                Cursor = "cursor_id",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetBlockedUserIdsQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/blocks/ids.json?cursor=cursor_id&count=42&hello=world");
        }

        [Fact]
        public void GetBlockedUsersQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            var parameters = new GetBlockedUsersParameters
            {
                PageSize = 42,
                Cursor = "cursor_id",
                IncludeEntities = true,
                SkipStatus = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetBlockedUsersQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/blocks/list.json?cursor=cursor_id&count=42&include_entities=true&skip_status=true&hello=world");
        }

        [Fact]
        public void GetBlockUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();
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
            var queryGenerator = CreateAccountQueryGenerator();
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
            var queryGenerator = CreateAccountQueryGenerator();
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

        // FOLLOWERS

        [Fact]
        public void GetUnFollowUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();
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
            var queryGenerator = CreateAccountQueryGenerator();
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
        public void GetUserIdsRequestingFriendshipQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();

            var parameters = new GetUserIdsRequestingFriendshipParameters
            {
                PageSize = 42,
                Cursor = "start_cursor",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserIdsRequestingFriendshipQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/incoming.json?count=42&cursor=start_cursor&hello=world");
        }

        // FRIENDSHIPS
        [Fact]
        public void GetRelationshipsWithQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateAccountQueryGenerator();
            
            var users = new IUserIdentifier[]
            {
                new UserIdentifier(42),
                new UserIdentifier(43),
                new UserIdentifier("tweetinviapi"),
                new UserIdentifier("tweetinvitest"),
            };

            var parameters = new GetRelationshipsWithParameters(users)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetRelationshipsWithQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/lookup.json?user_id=42%2C43&screen_name=tweetinviapi%2Ctweetinvitest&hello=world");
        }
    }
}