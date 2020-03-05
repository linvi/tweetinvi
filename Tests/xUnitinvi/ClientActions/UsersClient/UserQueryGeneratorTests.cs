using System;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Controllers.User;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
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
        }

        private readonly FakeClassBuilder<UserQueryGenerator> _fakeBuilder;

        private UserQueryGenerator CreateUserQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("queryParameterGenerator", TweetinviContainer.Resolve<IQueryParameterGenerator>()),
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()));
        }

        [Fact]
        public void GetAuthenticatedUserQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateUserQueryGenerator();
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
            var queryGenerator = CreateUserQueryGenerator();

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
            var queryGenerator = CreateUserQueryGenerator();

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
        }

        // FOLLOWERS

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
        }

        [Fact]
        public void GetUserIdsRequestingFriendshipQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetUserIdsRequestingFriendshipParameters
            {
                PageSize = 42,
                Cursor = "start_cursor",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserIdsRequestingFriendshipQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/incoming.json?cursor=start_cursor&count=42&hello=world");
        }

        [Fact]
        public void GetUserIdsYouRequestedToFollowQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetUserIdsYouRequestedToFollowParameters
            {
                PageSize = 42,
                Cursor = "start_cursor",
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserIdsYouRequestedToFollowQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/outgoing.json?cursor=start_cursor&count=42&hello=world");
        }

        // FRIENDSHIPS
        [Fact]
        public void GetUpdateRelationshipQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new UpdateRelationshipParameters(42)
            {
                EnableRetweets = true,
                EnableDeviceNotifications = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUpdateRelationshipQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/update.json?user_id=42&device=true&retweets=true&hello=world");
        }

        [Fact]
        public void GetRelationshipsWithQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

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

        // MUTE

        [Fact]
        public void GetUserIdsWhoseRetweetsAreMutedQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetUserIdsWhoseRetweetsAreMutedParameters
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUserIdsWhoseRetweetsAreMutedQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/no_retweets/ids.json?hello=world");
        }

        [Fact]
        public void GetMutedUserIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetMutedUserIdsParameters
            {
                Cursor = "42",
                PageSize = 43,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetMutedUserIdsQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/mutes/users/ids.json?cursor=42&count=43&hello=world");
        }

        [Fact]
        public void GetMutedUsersQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new GetMutedUsersParameters
            {
                Cursor = "42",
                PageSize = 43,
                IncludeEntities = true,
                SkipStatus = false,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetMutedUsersQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/mutes/users/list.json?cursor=42&count=43&include_entities=true&skip_status=false&hello=world");
        }

        [Fact]
        public void GetMuteUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new MuteUserParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetMuteUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/mutes/users/create.json?user_id=42&hello=world");
        }

        [Fact]
        public void GetUnmuteUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateUserQueryGenerator();

            var parameters = new UnmuteUserParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetUnmuteUserQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/mutes/users/destroy.json?user_id=42&hello=world");
        }

        [Fact]
        public void GetUserQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetUserParameters(42)
            {
                SkipStatus = true,
                IncludeEntities = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.GetUserQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/users/show.json?user_id=42&skip_status=true&include_entities=true&tweet_mode=extended&hello=world");
        }

        [Fact]
        public void GetUsersQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetUsersParameters(new long[] { 42, 43 })
            {
                SkipStatus = true,
                IncludeEntities = true,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.GetUsersQuery(parameters, TweetMode.Extended);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/users/lookup.json?user_id=42%2C43&skip_status=true&include_entities=true&tweet_mode=extended&hello=world");
        }

        [Fact]
        public void GetFriendIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetFriendIdsParameters(42)
            {
                Cursor = "cursor_id",
                PageSize = 43,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.GetFriendIdsQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friends/ids.json?user_id=42&cursor=cursor_id&count=43&hello=world");
        }

        [Fact]
        public void GetFollowerIdsQuery_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetFollowerIdsParameters(42)
            {
                Cursor = "cursor_id",
                PageSize = 43,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.GetFollowerIdsQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/followers/ids.json?user_id=42&cursor=cursor_id&count=43&hello=world");
        }

        [Fact]
        public void GetRelationshipBetween_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetRelationshipBetweenParameters(42, 43)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            var queryGenerator = CreateUserQueryGenerator();

            // Act
            var result = queryGenerator.GetRelationshipBetweenQuery(parameters);

            // Assert
            Assert.Equal(result, $"https://api.twitter.com/1.1/friendships/show.json?source_id=42&target_id=43&hello=world");
        }

        [Fact]
        public void DownloadProfileImageURL_ReturnsExpectedQuery()
        {
            // Arrange
            var parameters = new GetProfileImageParameters("https://url_normal.jpg")
            {
                ImageSize = ImageSize.Bigger,
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