using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Controllers.Messages;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.ClientActions.MessagesClient
{
    public class MessageQueryGeneratorTests
    {
        public MessageQueryGeneratorTests()
        {
            _fakeBuilder = new FakeClassBuilder<MessageQueryGenerator>();
        }

        private readonly FakeClassBuilder<MessageQueryGenerator> _fakeBuilder;

        private MessageQueryGenerator CreateTwitterListQueryGenerator()
        {
            return _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("jsonContentFactory", TweetinviContainer.Resolve<JsonContentFactory>())
            );
        }

        [Fact]
        public void GetMessageQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new GetMessageParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // Act
            var result = queryGenerator.GetMessageQuery(parameters);

            // Assert
            Assert.Equal(result, "https://api.twitter.com/1.1/direct_messages/events/show.json?id=42&hello=world");
        }

        [Fact]
        public async Task GetPublishMessageQuery_ReturnsExpectedQueryAsync()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new PublishMessageParameters("plop", 42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetPublishMessageQuery(parameters);
            var content = await result.Content.ReadAsStringAsync();
            var body = new TwitterClient(new TwitterCredentials()).Json.Deserialize<CreateMessageDTO>(content);

            // assert
            Assert.Equal(result.Url, "https://api.twitter.com/1.1/direct_messages/events/new.json?hello=world");
            Assert.Equal(body.MessageEvent.MessageCreate.MessageData.Text, "plop");
            Assert.Equal(body.MessageEvent.MessageCreate.Target.RecipientId, 42);
        }

        [Fact]
        public async Task GetPublishMessageQuery_WithMedia_ReturnsExpectedQueryAsync()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new PublishMessageParameters("plop", 42)
            {
                MediaId = 967,
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetPublishMessageQuery(parameters);
            var content = await result.Content.ReadAsStringAsync();
            var body = new TwitterClient(new TwitterCredentials()).Json.Deserialize<CreateMessageDTO>(content);

            // assert
            Assert.Equal(result.Url, "https://api.twitter.com/1.1/direct_messages/events/new.json?hello=world");
            Assert.Equal(body.MessageEvent.MessageCreate.MessageData.Text, "plop");
            Assert.Equal(body.MessageEvent.MessageCreate.Target.RecipientId, 42);
            Assert.Equal(body.MessageEvent.MessageCreate.MessageData.Attachment.Media.Id, 967);
        }

        [Fact]
        public void GetDestroyMessageQuery_ReturnsExpectedQuery()
        {
            // arrange
            var queryGenerator = CreateTwitterListQueryGenerator();

            var parameters = new DestroyMessageParameters(42)
            {
                CustomQueryParameters = { new Tuple<string, string>("hello", "world") }
            };

            // act
            var result = queryGenerator.GetDestroyMessageQuery(parameters);

            // assert
            Assert.Equal(result, "https://api.twitter.com/1.1/direct_messages/events/destroy.json?id=42&hello=world");
        }
    }
}