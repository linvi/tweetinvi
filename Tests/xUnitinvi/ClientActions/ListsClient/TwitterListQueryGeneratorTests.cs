using System;
using Tweetinvi.Controllers.TwitterLists;
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
            return _fakeBuilder.GenerateClass();
        }

        [Fact]
        public void GetHomeTimelineIterator_ReturnsFromPageCursorIteratorFactories()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new CreateTwitterListParameters("list_name")
            {
                Description = "list_desc",
                PrivacyMode = PrivacyMode.Private,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetCreateTwitterListQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/lists/create.json?name=list_name&mode=private&description=list_desc&hello=world");
        }
    }
}