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
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class TweetsEndToEndTests : TweetinviTest
    {
        private readonly ITwitterClient _protectedClient;
        private readonly ITwitterClient _tweetinviTestClient;

        public TweetsEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
            _protectedClient = new TwitterClient(EndToEndTestConfig.ProtectedUser.Credentials);
            _tweetinviTestClient = new TwitterClient(EndToEndTestConfig.TweetinviTest.Credentials);
        }

        [Fact]
        public async Task CreateReadDelete()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var sourceTweet = await _protectedClient.Tweets.GetTweet(979753598446948353);

            var quotingTweet1 = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("tweetinvi 3.0!")
            {
                QuotedTweetUrl = "https://twitter.com/TweetinviApi/status/979753598446948353",
            });

            var quotingTweet2 = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                QuotedTweet = sourceTweet
            });

            var replyTweet = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("Tweetinvi 3 v2!")
            {
                InReplyToTweetId = sourceTweet.Id,
                AutoPopulateReplyMetadata = true
            });

            var fullTweet = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("#tweetinvi and https://github.com/linvi/tweetinvi Full Tweet!")
            {
                Coordinates = new Coordinates(37.7821120598956, -122.400612831116),
                DisplayExactCoordinates = true,
                TrimUser = true,
                PlaceId = "3e8542a1e9f82870",
            });

            var tweetinviLogoBinary = File.ReadAllBytes("./tweetinvi-logo-purple.png");
            var tweetWithMedia = await _protectedClient.Tweets.PublishTweet(new PublishTweetParameters("tweet with media")
            {
                MediaBinaries = {tweetinviLogoBinary},
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

            var allTweets = await _protectedClient.Tweets.GetTweets(allTweetIdentifiers);

            await _protectedClient.Tweets.DestroyTweet(quotingTweet1);
            await _protectedClient.Tweets.DestroyTweet(quotingTweet2);
            await _protectedClient.Tweets.DestroyTweet(replyTweet);
            await _protectedClient.Tweets.DestroyTweet(fullTweet);
            await _protectedClient.Tweets.DestroyTweet(tweetWithMedia);

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
        public async Task Retweets()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            const long tweetId = 979753598446948353;

            var sourceTweet = await _protectedClient.Tweets.GetTweet(tweetId);
            var retweet = await _protectedClient.Tweets.PublishRetweet(sourceTweet);
            await Task.Delay(1000).ConfigureAwait(false); // for Twitter to sync
            var sourceRetweets = await _protectedClient.Tweets.GetRetweets(sourceTweet);
            var tweetAfterRetweet = await _protectedClient.Tweets.GetTweet(tweetId);

            var allRetweeterIdsBefore = new List<long>();

            var retweeterIdsBeforeIterator = _protectedClient.Tweets.GetRetweeterIdsIterator(tweetId);
            while (!retweeterIdsBeforeIterator.Completed)
            {
                allRetweeterIdsBefore.AddRange(await retweeterIdsBeforeIterator.MoveToNextPage());
            }

            await _protectedClient.Tweets.DestroyRetweet(retweet).ConfigureAwait(false);
            var tweetAfterDestroy = await _protectedClient.Tweets.GetTweet(tweetId);

            // assert
            Assert.Equal(tweetAfterRetweet.RetweetCount, sourceTweet.RetweetCount + 1);
            Assert.Equal(retweet.RetweetedTweet.Id, tweetId);
            Assert.NotNull(retweet.CreatedBy.Id);
            Assert.Contains(retweet.Id, sourceRetweets.Select(x => x.Id));
            Assert.Contains(retweet.CreatedBy.Id.Value, allRetweeterIdsBefore);
            Assert.Equal(tweetAfterDestroy.RetweetCount, sourceTweet.RetweetCount);
        }

        [Fact]
        public async Task Favorite()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviTestClient.Tweets.PublishTweet(Guid.NewGuid().ToString()).ConfigureAwait(false);
            var favoritedAtStart = tweet.Favorited;

            await _tweetinviTestClient.Tweets.FavoriteTweet(tweet).ConfigureAwait(false);
            var tweetAfterFavoriteCall = await _tweetinviTestClient.Tweets.GetTweet(tweet.Id).ConfigureAwait(false);
            var inMemoryTweetFavoriteStateAfterFavoriteCall = tweet.Favorited;

            await _tweetinviTestClient.Tweets.UnFavoriteTweet(tweet);
            var tweetAfterUnFavoriteCall = await _tweetinviTestClient.Tweets.GetTweet(tweet.Id).ConfigureAwait(false);
            var inMemoryTweetFavoriteStateAfterUnFavoriteCall = tweet.Favorited;

            await _tweetinviTestClient.Tweets.DestroyTweet(tweet).ConfigureAwait(false);

            // Assert
            Assert.False(favoritedAtStart);
            Assert.True(tweetAfterFavoriteCall.Favorited);
            Assert.True(inMemoryTweetFavoriteStateAfterFavoriteCall);
            Assert.False(tweetAfterUnFavoriteCall.Favorited);
            Assert.False(inMemoryTweetFavoriteStateAfterUnFavoriteCall);
        }

        [Fact]
        public async Task OEmbedTweets()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet = await _tweetinviTestClient.Tweets.PublishTweet(Guid.NewGuid().ToString()).ConfigureAwait(false);
            var oEmbedTweet = await _tweetinviTestClient.Tweets.GetOEmbedTweet(tweet).ConfigureAwait(false);

            await tweet.Destroy();

            // Assert
            Assert.Contains(tweet.CreatedBy.ScreenName, oEmbedTweet.HTML);
            Assert.Contains(tweet.Text, oEmbedTweet.HTML);
        }
    }
}