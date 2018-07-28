using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.json.net;
using Tweetinvi;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Public.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Factories.Tweet;
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streams;
using Tweetinvi.Streams.Helpers;

namespace Testinvi.Tweetinvi.Streams
{
    [TestClass]
    public class AccountActivityStreamTests
    {
        private FakeClassBuilder<AccountActivityStream> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<AccountActivityStream>();
        }

        public IAccountActivityStream CreateAccountActivityStream()
        {
            var activityStream = _fakeBuilder.GenerateClass(
                new ConstructorNamedParameter("jsonObjectConverter", TweetinviContainer.Resolve<IJsonObjectConverter>()),
                new ConstructorNamedParameter("jObjectWrapper", TweetinviContainer.Resolve<IJObjectStaticWrapper>()),
                new ConstructorNamedParameter("tweetFactory", TweetinviContainer.Resolve<ITweetFactory>()));

            return activityStream;
        }

        [TestMethod]
        public void TweetEventProperlyRaised()
        {
            var activityStream = CreateAccountActivityStream();
            
            var tweetCreatedJson = @"{
	            ""for_user_id"": ""2244994945"",
	            ""tweet_create_events"": [
	              " + JsonTests.TWEET_TEST_JSON + @"
	            ]
            }";

            var eventsReceived = new List<TweetReceivedEventArgs>();
            activityStream.TweetCreated += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Tweet.CreatedBy.Id, 2244994945);
        }
    }
}
