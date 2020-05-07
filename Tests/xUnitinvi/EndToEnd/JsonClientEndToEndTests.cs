using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.Events;
using Tweetinvi.Parameters;
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
            _logger.WriteLine($"Conversion of : {typeof(TFrom).Name} <--> {typeof(TTo).Name}");

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
        public async Task UsersAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var user = await _tweetinviClient.Users.GetUserAsync("tweetinviapi");
            var verifier = new Action<IUserIdentifier>(identifier => { Assert.Equal(user.Id, identifier.Id); });

            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUser, IUser>(user, verifier);
            TestSerializer<IUser, IUserDTO>(user, verifier);
            TestSerializer<IUserDTO, IUserDTO>(user.UserDTO, verifier);
            TestSerializer<IUserDTO, IUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task AuthenticatedUsersAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var user = await _tweetinviClient.Users.GetAuthenticatedUserAsync();
            var verifier = new Action<IUserIdentifier>(identifier => { Assert.Equal(user.Id, identifier.Id); });

            TestSerializer<IAuthenticatedUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUser>(user, verifier);
            TestSerializer<IAuthenticatedUser, IUserDTO>(user, verifier);
            TestSerializer<IUser, IAuthenticatedUser>(user, verifier);
            TestSerializer<IUserDTO, IAuthenticatedUser>(user.UserDTO, verifier);
        }

        [Fact]
        public async Task TweetsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviClient.Tweets.GetTweetAsync(979753598446948353);
            var verifier = new Action<ITweetIdentifier>(identifier => { Assert.Equal(tweet.Id, identifier.Id); });

            TestSerializer<ITweet, ITweet>(tweet, verifier);
            TestSerializer<ITweet, ITweetDTO>(tweet, verifier);
            TestSerializer<ITweetDTO, ITweet>(tweet.TweetDTO, verifier);
            TestSerializer<ITweetDTO, ITweetDTO>(tweet.TweetDTO, verifier);
        }

        [Fact]
        public async Task MessageAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var message = await _tweetinviClient.Messages.PublishMessageAsync("How are you doing?", EndToEndTestConfig.TweetinviTest.UserId);
            await message.DestroyAsync();

            TestSerializer<IMessage, IMessage>(message, deserializedMessage => { Assert.Equal(message.Text, deserializedMessage.Text); });

            TestSerializer<IMessage, IMessageEventDTO>(message, deserializedMessage => { Assert.Equal(message.Text, deserializedMessage.MessageCreate.MessageData.Text); });

            _logger.WriteLine($"Conversion of : {typeof(IMessage).Name} <--> {typeof(IMessageEventWithAppDTO).Name}");

            var json = _tweetinviClient.Json.Serialize<IMessage, IMessageEventWithAppDTO>(message);
            var deserializedObject = _tweetinviClient.Json.Deserialize<IMessageEventWithAppDTO>(json);

            Assert.Equal(message.Text, deserializedObject.MessageEvent.MessageCreate.MessageData.Text);
        }

        [Fact]
        public async Task TwitterListsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var list = await _tweetinviClient.Lists.CreateListAsync("THE_LIST");
            await _tweetinviClient.Lists.DestroyListAsync(list);

            var verifier = new Action<ITwitterListIdentifier>(identifier => { Assert.Equal(list.Id, identifier.Id); });

            TestSerializer<ITwitterList, ITwitterList>(list, verifier);
            TestSerializer<ITwitterList, ITwitterListDTO>(list, verifier);
            TestSerializer<ITwitterListDTO, ITwitterListDTO>(list.TwitterListDTO, verifier);
            TestSerializer<ITwitterListDTO, ITwitterList>(list.TwitterListDTO, verifier);
        }

        [Fact]
        public async Task AccountSettingsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var accountSettings = await _tweetinviClient.AccountSettings.GetAccountSettingsAsync();

            TestSerializer<IAccountSettings, IAccountSettings>(accountSettings, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettings, IAccountSettingsDTO>(accountSettings, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettingsDTO, IAccountSettingsDTO>(accountSettings.AccountSettingsDTO, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
            TestSerializer<IAccountSettingsDTO, IAccountSettings>(accountSettings.AccountSettingsDTO, settings =>
            {
                Assert.Equal(accountSettings.TimeZone.Name, settings.TimeZone.Name);
            });
        }

        [Fact]
        public async Task MediaAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var media = await _tweetinviClient.Upload.UploadBinaryAsync(tweetinviLogoBinary);

            TestSerializer<IMedia, IMedia>(media, deserializedMedia =>
            {
                Assert.Equal(media.UploadedMediaInfo.MediaId, deserializedMedia.UploadedMediaInfo.MediaId);
            });

            TestSerializer<IUploadedMediaInfo, IUploadedMediaInfo>(media.UploadedMediaInfo, uploadedMediaInfo =>
            {
                Assert.Equal(media.UploadedMediaInfo.MediaId, uploadedMediaInfo.MediaId);
            });
        }

        [Fact]
        public async Task SearchTweetWithMetadataAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var searchResults = await _tweetinviClient.Search.SearchTweetsWithMetadataAsync("hello");

            TestSerializer<ISearchResults, ISearchResults>(searchResults, deserializedSearchResult =>
            {
                Assert.Contains(
                searchResults.Tweets.Select(x => x.Id),
                tweetId => deserializedSearchResult.Tweets.Any(tweet => tweet.Id == tweetId));
            });
        }

        [Fact]
        public async Task SavedSearchAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var savedSearch = await _tweetinviTestClient.Search.CreateSavedSearchAsync("sandwiches");
            await _tweetinviTestClient.Search.DestroySavedSearchAsync(savedSearch);

            TestSerializer<ISavedSearch, ISavedSearch>(savedSearch, deserializedSavedSearch =>
            {
                Assert.Equal(savedSearch.Query, deserializedSavedSearch.Query);
            });
        }

        [Fact]
        public void Credentials()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var twitterCredentials = new TwitterCredentials("A", "B", "C", "D")
            {
                BearerToken = "E"
            };

            TestSerializer<ITwitterCredentials, ITwitterCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.AccessToken, creds.AccessToken);
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<ITwitterCredentials, IConsumerCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<IConsumerCredentials, IConsumerCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });

            TestSerializer<IConsumerCredentials, ITwitterCredentials>(twitterCredentials, creds =>
            {
                Assert.Equal(twitterCredentials.ConsumerKey, creds.ConsumerKey);
                Assert.Equal(twitterCredentials.BearerToken, creds.BearerToken);
            });
        }

        [Fact]
        public async Task RateLimitsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var rateLimits = await _tweetinviClient.RateLimits.GetRateLimitsAsync(new GetRateLimitsParameters());

            TestSerializer<ICredentialsRateLimits, ICredentialsRateLimits>(rateLimits, deserializedRateLimits =>
            {
                Assert.Equal(rateLimits.ApplicationRateLimitStatusLimit.Remaining, deserializedRateLimits.ApplicationRateLimitStatusLimit.Remaining);
            });
        }

        [Fact]
        public async Task TwitterConfigurationAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var config = await _tweetinviClient.Help.GetTwitterConfigurationAsync();

            TestSerializer<ITwitterConfiguration, ITwitterConfiguration>(config, deserializedConfig =>
            {
                Assert.Equal(config.ShortURLLength, deserializedConfig.ShortURLLength);
            });
        }

        [Fact]
        public async Task SupportedLanguageAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var language = (await _tweetinviClient.Help.GetSupportedLanguagesAsync())[0];

            TestSerializer<SupportedLanguage, SupportedLanguage>(language, deserializedLanguage =>
            {
                Assert.Equal(language.Status, deserializedLanguage.Status);
            });
        }

        [Fact]
        public async Task PlaceAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var place = await _tweetinviTestClient.Help.GetPlaceAsync("df51dec6f4ee2b2c");

            TestSerializer<IPlace, IPlace>(place, deserializedPlace =>
            {
                Assert.Equal(place.PlaceType, deserializedPlace.PlaceType);
            });
        }

        [Fact]
        public async Task TrendsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var trendsResult = await _tweetinviTestClient.Trends.GetPlaceTrendsAtAsync(1);
            var trendsLocations = await _tweetinviTestClient.Trends.GetTrendLocationsAsync();

            TestSerializer<IGetTrendsAtResult, IGetTrendsAtResult>(trendsResult, deserializedTrendsResult =>
            {
                Assert.Equal(trendsResult.Trends.Length, deserializedTrendsResult.Trends.Length);
            });

            TestSerializer<ITrendLocation, ITrendLocation>(trendsLocations[0], deserializedTrendLocations =>
            {
                Assert.Equal(trendsLocations[0].Country, deserializedTrendLocations.Country);
            });
        }

        [Fact]
        public async Task WebhooksAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var environments = await _tweetinviClient.AccountActivity.GetAccountActivityWebhookEnvironmentsAsync();
            var count = await _tweetinviClient.AccountActivity.CountAccountActivitySubscriptionsAsync();
            var subscriptions = await _tweetinviClient.AccountActivity.GetAccountActivitySubscriptionsAsync(environments[0].Name);

            TestSerializer<IWebhookEnvironment, IWebhookEnvironment>(environments[0], deserializedEnvironment =>
            {
                Assert.Equal(environments[0].Name, deserializedEnvironment.Name);
            });

            TestSerializer<IWebhookSubscriptionsCount, IWebhookSubscriptionsCount>(count, deserializedCount =>
            {
                Assert.Equal(count.AccountName, deserializedCount.AccountName);
            });

            TestSerializer<IWebhookEnvironmentSubscriptions, IWebhookEnvironmentSubscriptions>(subscriptions, deserializedSubscriptions =>
            {
                Assert.Equal(subscriptions.ApplicationId, deserializedSubscriptions.ApplicationId);
            });
        }
    }
}