using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class TweetsIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ITwitterClient PublicClient;
        private readonly ITwitterClient ProtectedClient;

        public TweetsIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            PublicClient = new TwitterClient(IntegrationTestConfig.TweetinviTestCredentials);
            ProtectedClient = new TwitterClient(IntegrationTestConfig.ProtectedUserCredentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllTweets()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
            {
                return;
            }

            _logger.WriteLine($"Starting {nameof(CreateReadDelete)}");
            await CreateReadDelete().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(CreateReadDelete)} succeeded");

            _logger.WriteLine($"Starting {nameof(Retweets)}");
            await Retweets().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(Retweets)} succeeded");
        }


        private async Task CreateReadDelete()
        {
            var sourceTweet = await ProtectedClient.Tweets.GetTweet(979753598446948353);

            var quotingTweet1 = await ProtectedClient.Tweets.PublishTweet(new PublishTweetParameters("tweetinvi 3.0!")
            {
                QuotedTweetUrl = "https://twitter.com/TweetinviApi/status/979753598446948353",
            });

            var quotingTweet2 = await ProtectedClient.Tweets.PublishTweet(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                QuotedTweet = sourceTweet
            });

            var replyTweet = await ProtectedClient.Tweets.PublishTweet(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                InReplyToTweetId = sourceTweet.Id,
                AutoPopulateReplyMetadata = true
            });

            var fullTweet = await ProtectedClient.Tweets.PublishTweet(new PublishTweetParameters("#tweetinvi and https://github.com/linvi/tweetinvi Full Tweet!")
            {
                Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
                DisplayExactCoordinates = true,
                TrimUser = true,
                PlaceId = "3e8542a1e9f82870",
            });

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var tweetWithMedia = await ProtectedClient.Tweets.PublishTweet(new PublishTweetParameters("tweet with media")
            {
                MediaBinaries = { tweetinviLogoBinary },
                PossiblySensitive = true,
            });

            var allTweetIdentifiers = new ITweetIdentifier[]
            {
                quotingTweet1,
                quotingTweet2,
                replyTweet,
                fullTweet,
                tweetWithMedia
            };

            var allTweets = await ProtectedClient.Tweets.GetTweets(allTweetIdentifiers);

            var quotingTweet1DestroySuccess = await ProtectedClient.Tweets.DestroyTweet(quotingTweet1);
            var quotingTweet2DestroySuccess = await ProtectedClient.Tweets.DestroyTweet(quotingTweet2);
            var replyTweetDestroy = await ProtectedClient.Tweets.DestroyTweet(replyTweet);
            var fullTweetDestroy = await ProtectedClient.Tweets.DestroyTweet(fullTweet);
            var withMediaDestroy = await ProtectedClient.Tweets.DestroyTweet(tweetWithMedia);

            // ASSERT
            Assert.Equal(979753598446948353, quotingTweet1.QuotedStatusId);
            Assert.Equal(979753598446948353, quotingTweet2.QuotedStatusId);
            Assert.Equal(sourceTweet.Id, replyTweet.InReplyToStatusId);
            Assert.Equal("TweetinviApi", replyTweet.UserMentions[0].ScreenName);

            Assert.Null(fullTweet.CreatedBy.Name);
            Assert.Equal(37, Math.Floor(fullTweet.Coordinates.Latitude));
            Assert.Equal(-122, Math.Ceiling(fullTweet.Coordinates.Longitude));
            Assert.Equal("Toronto", fullTweet.Place.Name);
            Assert.Equal("tweetinvi", fullTweet.Hashtags[0].Text);
            Assert.Single(fullTweet.Entities.Hashtags);
            Assert.Equal("https://github.com/linvi/tweetinvi", fullTweet.Urls[0].ExpandedURL);
            Assert.Single(fullTweet.Entities.Urls);

            Assert.Single(tweetWithMedia.Media);
            Assert.True(tweetWithMedia.PossiblySensitive);

            Assert.True(allTweetIdentifiers.Select(x => x.Id).All(shouldItem => allTweets.Any(isItem => isItem.Id == shouldItem)));

            Assert.True(quotingTweet1DestroySuccess);
            Assert.True(quotingTweet2DestroySuccess);
            Assert.True(replyTweetDestroy);
            Assert.True(fullTweetDestroy);
            Assert.True(withMediaDestroy);
        }

        private async Task Retweets()
        {
            var tweetId = 979753598446948353;

            var sourceTweet = await ProtectedClient.Tweets.GetTweet(tweetId);
            var retweet = await ProtectedClient.Tweets.PublishRetweet(sourceTweet);
            var sourceRetweets = await ProtectedClient.Tweets.GetRetweets(sourceTweet);
            var tweetAfterRetweet = await ProtectedClient.Tweets.GetTweet(tweetId);

            var allRetweeterIdsBefore = new List<long>();

            var retweeterIdsBeforeIterator = ProtectedClient.Tweets.GetRetweeterIdsIterator(tweetId);
            while (!retweeterIdsBeforeIterator.Completed)
            {
                allRetweeterIdsBefore.AddRange(await retweeterIdsBeforeIterator.MoveToNextPage());
            }

            await ProtectedClient.Tweets.DestroyRetweet(retweet);
            var tweetAfterDestroy = await ProtectedClient.Tweets.GetTweet(tweetId);

            // assert
            Assert.Equal(tweetAfterRetweet.RetweetCount, sourceTweet.RetweetCount + 1);
            Assert.Equal(retweet.RetweetedTweet.Id, tweetId);
            Assert.NotNull(retweet.CreatedBy.Id);
            Assert.Contains(retweet.Id, sourceRetweets.Select(x => x.Id));
            Assert.Contains(retweet.CreatedBy.Id.Value, allRetweeterIdsBefore);
            Assert.Equal(tweetAfterDestroy.RetweetCount, sourceTweet.RetweetCount);
        }
    }
}