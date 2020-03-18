using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    public class JsonClientTests : TweetinviTest
    {
        public JsonClientTests(ITestOutputHelper logger) : base(logger)
        {
        }

        private void TestSerializer<TFrom, TTo>(TFrom input, Action<TTo> verify)
            where TFrom : class
            where TTo : class
        {
            var json = _tweetinviClient.Json.Serialize(input);
            var deserializedObject = _tweetinviClient.Json.Deserialize<TTo>(json);

            var inputArray = new[] {input, input};
            var arrayJson = _tweetinviClient.Json.Serialize(inputArray);
            var deserializedArray = _tweetinviClient.Json.Deserialize<TTo[]>(arrayJson);

            verify?.Invoke(deserializedObject);

            Assert.Equal(deserializedArray.Length, 2);
            verify?.Invoke(deserializedArray[1]);
        }

        [Fact]
        public async Task Users()
        {
            var user = await _tweetinviClient.Users.GetUser("tweetinviapi");
            var verifier = new Action<IUserIdentifier>(identifier =>
            {
                Assert.Equal(user.Id, identifier .Id);
            });

            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUser, IUser>(user, verifier);
            TestSerializer<IUser, IUserDTO>(user, verifier);
            TestSerializer<IUserDTO, IUserDTO>(user.UserDTO, verifier);
            TestSerializer<IUserDTO, IUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task AuthenticatedUsers()
        {
            var user = await _tweetinviClient.Users.GetAuthenticatedUser();
            var verifier = new Action<IUserIdentifier>(identifier =>
            {
                Assert.Equal(user.Id, identifier .Id);
            });

            TestSerializer<IAuthenticatedUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUserDTO>(user, verifier);
            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUserDTO, IAuthenticatedUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task Tweets()
        {
            var tweet = await _tweetinviClient.Tweets.GetTweet(979753598446948353);
            var verifier = new Action<ITweetIdentifier>(identifier =>
            {
                Assert.Equal(tweet.Id, identifier .Id);
            });

            TestSerializer<ITweet, ITweet>(tweet, verifier);
            TestSerializer<ITweet, ITweetDTO>(tweet, verifier);
            TestSerializer<ITweetDTO, ITweet>(tweet.TweetDTO, verifier);
            TestSerializer<ITweetDTO, ITweetDTO>(tweet.TweetDTO, verifier);
        }

        [Fact]
        public async Task Message()
        {
            var message = await _tweetinviClient.Messages.PublishMessage("How are you doing?", EndToEndTestConfig.TweetinviTest.UserId);
            await message.Destroy();

            TestSerializer<IMessage, IMessage>(message, deserializedMessage =>
            {
                Assert.Equal(message.Text, deserializedMessage.Text);
            });

            TestSerializer<IMessage, IMessageEventDTO>(message, deserializedMessage =>
            {
                Assert.Equal(message.Text, deserializedMessage.MessageCreate.MessageData.Text);
            });

            var json = _tweetinviClient.Json.Serialize<IMessage, IMessageEventWithAppDTO>(message);
            var deserializedObject = _tweetinviClient.Json.Deserialize<IMessageEventWithAppDTO>(json);

            Assert.Equal(message.Text, deserializedObject.MessageEvent.MessageCreate.MessageData.Text);
        }
    }
}