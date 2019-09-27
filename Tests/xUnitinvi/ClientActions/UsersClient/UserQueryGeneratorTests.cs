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