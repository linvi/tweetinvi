using System;
using Tweetinvi;
using Tweetinvi.Controllers.TwitterLists;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters.ListsClient;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.ListsClient
{
    public class TwitterListQueryGeneratorTests
    {
        public TwitterListQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<TwitterListQueryGenerator>();
        }

        private readonly FakeClassBuilder<TwitterListQueryGenerator> _fakeBuilder;

        private TwitterListQueryGenerator CreateTwitterListQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("userQueryParameterGenerator", TweetinviContainer.Resolve<IUserQueryParameterGenerator>()),
                new ConstructorNamedParameter("twitterListQueryParameterGenerator", TweetinviContainer.Resolve<ITwitterListQueryParameterGenerator>()));
        }

        [Fact]
        public void GetCreateListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new CreateListParameters("list_name")
            {
                Description = "list_desc",
                PrivacyMode = PrivacyMode.Private,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetCreateListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/create.json?name=list_name&mode=private&description=list_desc&hello=world");
        }

        [Fact]
        public void GetListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/show.json?list_id=42&hello=world");
        }

        [Fact]
        public void GetListQuery_WithSlug_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetListParameters(new TwitterListIdentifier("myslug", "username"))
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/show.json?slug=myslug&owner_screen_name=username&hello=world");
        }

        [Fact]
        public void GetDestroyListQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new DestroyListParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetDestroyListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/destroy.json?list_id=42&hello=world");
        }

        [Fact]
        public void GetDestroyListQuery_WithSlug_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new DestroyListParameters(new TwitterListIdentifier("myslug", "username"))
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetDestroyListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/destroy.json?slug=myslug&owner_screen_name=username&hello=world");
        }
    }
}