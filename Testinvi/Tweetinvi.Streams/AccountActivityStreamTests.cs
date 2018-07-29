using System.Collections.Generic;
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
using Tweetinvi.Models.Webhooks;
using Tweetinvi.Streams;

namespace Testinvi.Tweetinvi.Streams
{
    [TestClass]
    public class AccountActivityStreamTests
    {
        private FakeClassBuilder<AccountActivityStream> _fakeBuilder;
        private const int ACCOUNT_ACTIVITY_USER_ID = 42;

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
                new ConstructorNamedParameter("tweetFactory", TweetinviContainer.Resolve<ITweetFactory>()),
                new ConstructorNamedParameter("userFactory", TweetinviContainer.Resolve<IUserFactory>()));

            activityStream.UserId = ACCOUNT_ACTIVITY_USER_ID;

            return activityStream;
        }

        [TestMethod]
        public void TweetEventRaised()
        {
            var activityStream = CreateAccountActivityStream();
            
            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
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
            Assert.AreEqual(eventsReceived[0].Tweet.CreatedBy.Id, 42);
        }

        [TestMethod]
        public void FavouriteTweetRaised()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetFavouritedJson = @"{
	            ""for_user_id"": ""100"",
	            ""favorite_events"": [{
                  ""favorited_status"" : " + JsonTests.TWEET_TEST_JSON + @",
                  ""user"": " + JsonTests.USER_TEST_JSON(4242) + @"
	            }]
            }";

            var eventsReceived = new List<TweetFavouritedEventArgs>();
            activityStream.TweetFavourited += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetFavouritedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Tweet.CreatedBy.Id, 42);
            Assert.AreEqual(eventsReceived[0].FavouritingUser.Id, 4242);
        }

        [TestMethod]
        public void UserFollowedRaised_WithTargetUser()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
	            ""follow_events"": [{
                  ""target"" : " + JsonTests.USER_TEST_JSON(41) + @",
                  ""source"": " + JsonTests.USER_TEST_JSON(ACCOUNT_ACTIVITY_USER_ID) + @"
	            }]
            }";

            var eventsReceived = new List<UserFollowedEventArgs>();
            activityStream.UserFollowed += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Target.Id, 41);
        }

        [TestMethod]
        public void UserFollowedRaised_WithSourceUser()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
	            ""follow_events"": [{
                  ""target"" : " + JsonTests.USER_TEST_JSON(ACCOUNT_ACTIVITY_USER_ID) + @",
                  ""source"": " + JsonTests.USER_TEST_JSON(40) + @"
	            }]
            }";

            var eventsReceived = new List<UserFollowedEventArgs>();
            activityStream.UserFollowed += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Target.Id, 40);
        }

        [TestMethod]
        public void UserBlockedRaised_WithTargetUser()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
	            ""block_events"": [{
                  ""target"" : " + JsonTests.USER_TEST_JSON(41) + @",
                  ""source"": " + JsonTests.USER_TEST_JSON(ACCOUNT_ACTIVITY_USER_ID) + @"
	            }]
            }";

            var eventsReceived = new List<UserBlockedEventArgs>();
            activityStream.UserBlocked += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Target.Id, 41);
        }

        [TestMethod]
        public void UserBlockedRaised_WithSourceUser()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
	            ""block_events"": [{
                  ""target"" : " + JsonTests.USER_TEST_JSON(ACCOUNT_ACTIVITY_USER_ID) + @",
                  ""source"": " + JsonTests.USER_TEST_JSON(40) + @"
	            }]
            }";

            var eventsReceived = new List<UserBlockedEventArgs>();
            activityStream.UserBlocked += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Target.Id, 40);
        }

        [TestMethod]
        public void UserMutedRaised_WithTargetUser()
        {
            var activityStream = CreateAccountActivityStream();

            var tweetCreatedJson = @"{
	            ""for_user_id"": ""100"",
	            ""mute_events"": [{
                  ""type"": ""mute"",
                  ""target"" : " + JsonTests.USER_TEST_JSON(41) + @",
                  ""source"": " + JsonTests.USER_TEST_JSON(ACCOUNT_ACTIVITY_USER_ID) + @"
	            }]
            }";

            var eventsReceived = new List<UserMutedEventArgs>();
            activityStream.UserMuted += (sender, args) =>
            {
                eventsReceived.Add(args);
            };

            // Act
            activityStream.WebhookMessageReceived(new WebhookMessage(tweetCreatedJson));

            // Assert
            Assert.AreEqual(eventsReceived.Count, 1);
            Assert.AreEqual(eventsReceived[0].Target.Id, 41);
        }
    }
}
