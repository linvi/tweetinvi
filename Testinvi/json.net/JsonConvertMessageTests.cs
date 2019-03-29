using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Logic.Wrapper;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.json.net
{
    [TestClass]
    public class JsonConvertMessageTests
    {
        private FakeClassBuilder<JsonObjectConverter> _fakeBuilder;
        private JsonObjectConverter _jsonObjectConverter;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<JsonObjectConverter>();
            _jsonObjectConverter = _fakeBuilder.GenerateClass(new ConstructorNamedParameter("jsonConvertWrapper", new JsonConvertWrapper()));
        }

        [TestMethod]
        public void TestMessageJsonSerialization()
        {
            var json = @"{
              ""type"": ""message_create"",
              ""id"": ""1234858592"",
              ""created_timestamp"": ""1392078023603"",
              ""initiated_via"": {
                ""tweet_id"": ""123456"",
                ""welcome_message_id"": ""456789""
              },
              ""message_create"": {
                ""target"": {
                  ""recipient_id"": ""1234858592""
                },
                ""sender_id"": ""3805104374"",
                ""source_app_id"": ""268278"",
                ""message_data"": {
                  ""text"": ""Blue Bird"",
                  ""entities"": {
                    ""hashtags"": [{
                        ""text"": ""hashTagText""
                    }],
                    ""symbols"": [],
                    ""urls"": [],
                    ""user_mentions"": [],
                  },
                  ""quick_reply_response"": {
                    ""type"": ""options"",
                    ""metadata"": ""metadata_42""
                  },
                  ""attachment"": null
                }
              }
            }";

            var messageEventDTO = _jsonObjectConverter.DeserializeObject<IMessageEventDTO>(json);

            Assert.AreEqual(messageEventDTO.InitiatedVia.TweetId, 123456);
            Assert.AreEqual(messageEventDTO.InitiatedVia.WelcomeMessageId, 456789);

            var messageData = messageEventDTO.MessageCreate.MessageData;

            Assert.AreEqual(messageData.Text, "Blue Bird");
            Assert.AreEqual(messageData.QuickReplyResponse.Type, QuickReplyType.Options);
            Assert.AreEqual(messageData.QuickReplyResponse.Metadata, "metadata_42");

            var hashtags = messageData.Entities.Hashtags;

            Assert.AreEqual(hashtags.Count, 1);
            Assert.AreEqual(hashtags[0].Text, "hashTagText");
        }
    }
}
