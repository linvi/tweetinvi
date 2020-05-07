using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class TweetsEndToEndTests : TweetinviTest
    {
        public TweetsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task CreateReadDeleteAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var sourceTweet = await _protectedClient.Tweets.GetTweetAsync(979753598446948353);

            var quotingTweet1 = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("tweetinvi 3.0!")
            {
                QuotedTweetUrl = "https://twitter.com/TweetinviApi/status/979753598446948353",
            });

            var quotingTweet2 = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                QuotedTweet = sourceTweet
            });

            var replyTweet = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                InReplyToTweetId = sourceTweet.Id,
                AutoPopulateReplyMetadata = true
            });

            var fullTweet = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("#tweetinvi and https://github.com/linvi/tweetinvi Full Tweet!")
            {
                Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
                DisplayExactCoordinates = true,
                TrimUser = true,
                PlaceId = "3e8542a1e9f82870",
            });

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var media = await _protectedClient.Upload.UploadTweetImageAsync(tweetinviLogoBinary);
            var tweetWithMedia = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("tweet with media")
            {
                Medias = { media },
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

            var allTweets = await _protectedClient.Tweets.GetTweetsAsync(allTweetIdentifiers);

            await _protectedClient.Tweets.DestroyTweetAsync(quotingTweet1);
            await _protectedClient.Tweets.DestroyTweetAsync(quotingTweet2);
            await _protectedClient.Tweets.DestroyTweetAsync(replyTweet);
            await _protectedClient.Tweets.DestroyTweetAsync(fullTweet);
            await _protectedClient.Tweets.DestroyTweetAsync(tweetWithMedia);

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
        }

        [Fact]
        public async Task PublishWithMediaIdAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var media = await _protectedClient.Upload.UploadBinaryAsync(tweetinviLogoBinary);
            var tweetWithMedia = await _protectedClient.Tweets.PublishTweetAsync(new PublishTweetParameters("tweet with media")
            {
                MediaIds = { media.Id.Value },
                PossiblySensitive = true,
            });

            await _protectedClient.Tweets.DestroyTweetAsync(tweetWithMedia);

            Assert.Equal(tweetWithMedia.Media[0].Id, media.Id.Value);
        }

        [Fact]
        public async Task RetweetsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            const long tweetId = 979753598446948353;

            var sourceTweet = await _protectedClient.Tweets.GetTweetAsync(tweetId);
            var retweet = await _protectedClient.Tweets.PublishRetweetAsync(sourceTweet);
            await Task.Delay(TimeSpan.FromSeconds(35)); // for Twitter to sync
            var sourceRetweets = await _protectedClient.Tweets.GetRetweetsAsync(sourceTweet);
            var tweetAfterRetweet = await _protectedClient.Tweets.GetTweetAsync(tweetId);

            var allRetweeterIdsBefore = new List<long>();

            await _protectedClient.Tweets.GetRetweeterIdsAsync(tweetId);
            var retweeterIdsBeforeIterator = _protectedClient.Tweets.GetRetweeterIdsIterator(tweetId);
            while (!retweeterIdsBeforeIterator.Completed)
            {
                allRetweeterIdsBefore.AddRange(await retweeterIdsBeforeIterator.NextPageAsync());
            }

            await _protectedClient.Tweets.DestroyRetweetAsync(retweet);
            var tweetAfterDestroy = await _protectedClient.Tweets.GetTweetAsync(tweetId);

            // assert
            Assert.Equal(tweetAfterRetweet.RetweetCount, sourceTweet.RetweetCount + 1);
            Assert.Equal(retweet.RetweetedTweet.Id, tweetId);
            Assert.Contains(retweet.Id, sourceRetweets.Select(x => x.Id));
            Assert.Contains(retweet.CreatedBy.Id, allRetweeterIdsBefore);
            Assert.Equal(tweetAfterDestroy.RetweetCount, sourceTweet.RetweetCount);
        }

        [Fact]
        public async Task FavoriteAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviTestClient.Tweets.PublishTweetAsync(Guid.NewGuid().ToString());
            var favoritedAtStart = tweet.Favorited;

            await _tweetinviTestClient.Tweets.FavoriteTweetAsync(tweet);
            var tweetAfterFavoriteCall = await _tweetinviTestClient.Tweets.GetTweetAsync(tweet.Id);
            var inMemoryTweetFavoriteStateAfterFavoriteCall = tweet.Favorited;

            await _tweetinviTestClient.Tweets.GetUserFavoriteTweetsAsync(EndToEndTestConfig.TweetinviTest);

            await _tweetinviTestClient.Tweets.UnfavoriteTweetAsync(tweet);
            var tweetAfterUnfavoriteCall = await _tweetinviTestClient.Tweets.GetTweetAsync(tweet.Id);
            var inMemoryTweetFavoriteStateAfterUnfavoriteCall = tweet.Favorited;

            await _tweetinviTestClient.Tweets.DestroyTweetAsync(tweet);

            // Assert
            Assert.False(favoritedAtStart);
            Assert.True(tweetAfterFavoriteCall.Favorited);
            Assert.True(inMemoryTweetFavoriteStateAfterFavoriteCall);
            Assert.False(tweetAfterUnfavoriteCall.Favorited);
            Assert.False(inMemoryTweetFavoriteStateAfterUnfavoriteCall);
        }

        [Fact]
        public async Task OEmbedTweetsAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviTestClient.Tweets.PublishTweetAsync(Guid.NewGuid().ToString());
            var oEmbedTweet = await _tweetinviTestClient.Tweets.GetOEmbedTweetAsync(tweet);

            await tweet.DestroyAsync();

            // Assert
            Assert.Contains(tweet.CreatedBy.ScreenName, oEmbedTweet.HTML);
            Assert.Contains(tweet.Text, oEmbedTweet.HTML);
        }
    }
}