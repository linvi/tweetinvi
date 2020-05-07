using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class TimelinesEndToEndTests : TweetinviTest
    {
        public TimelinesEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task HomeTimeLineAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // arrange
            var testUser = await _tweetinviTestClient.Users.GetAuthenticatedUserAsync();
            var tweetinviUser = await _tweetinviClient.Users.GetAuthenticatedUserAsync();
            var friendsBeforeAdd = await _tweetinviClient.Users.GetFriendIdsIterator(tweetinviUser).NextPageAsync();
            var alreadyFollowing = friendsBeforeAdd.Contains(testUser.Id);

            if (!alreadyFollowing)
            {
                await _tweetinviClient.Users.FollowUserAsync(testUser);
            }

            // act - pre-cleanup

            await Task.Delay(1000); // time required for timeline to be generated
            var recentTweetIterators = _tweetinviClient.Timelines.GetHomeTimelineIterator();
            var recentTweets = await recentTweetIterators.NextPageAsync();
            var tweetToDelete = recentTweets.FirstOrDefault(x => x.Text == "tweet 1!");

            if (tweetToDelete != null)
            {
                await _tweetinviTestClient.Tweets.DestroyTweetAsync(tweetToDelete);
            }

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweetAsync("tweet 1!");

            await Task.Delay(2000); // time required for timeline to be generated

            await _tweetinviClient.Timelines.GetHomeTimelineAsync();
            var iterator = _tweetinviClient.Timelines.GetHomeTimelineIterator(new GetHomeTimelineParameters
            {
                PageSize = 1,
            });

            var page1 = await iterator.NextPageAsync();
            var page2 = await iterator.NextPageAsync();

            await tweet1.DestroyAsync();

            if (!alreadyFollowing)
            {
                await _tweetinviClient.Users.UnfollowUserAsync(testUser);
            }

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task UserTimelineAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweetAsync("tweet 1!");
            var tweetinviTest = EndToEndTestConfig.TweetinviTest.AccountId;

            await _tweetinviClient.Timelines.GetUserTimelineAsync(tweetinviTest);
            var iterator = _tweetinviClient.Timelines.GetUserTimelineIterator(new GetUserTimelineParameters(tweetinviTest)
            {
                PageSize = 5,
            });

            var page1 = await iterator.NextPageAsync();

            IEnumerable<ITweet> page2 = new ITweet[] { };
            if (!iterator.Completed)
            {
                page2 = await iterator.NextPageAsync();
            }

            await tweet1.DestroyAsync();

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task MentionsTimelineAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweetAsync($"Hello @{EndToEndTestConfig.TweetinviApi.AccountId}!");
            await Task.Delay(TimeSpan.FromSeconds(25));

            await _tweetinviClient.Timelines.GetMentionsTimelineAsync();
            var iterator = _tweetinviClient.Timelines.GetMentionsTimelineIterator();

            var page1 = await iterator.NextPageAsync();

            await tweet1.DestroyAsync();

            // assert
            Assert.Contains(tweet1.Id, page1.Select(x => x.Id));
        }

        [Fact]
        public async Task RetweetsOfMeTimelineAsync()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweetAsync("tweet 1!");
            var tweet2 = await _tweetinviTestClient.Tweets.PublishTweetAsync("tweet 2");

            await _tweetinviClient.Tweets.PublishRetweetAsync(tweet1);
            await _tweetinviClient.Tweets.PublishRetweetAsync(tweet2);

            await _tweetinviClient.Timelines.GetRetweetsOfMeTimelineAsync();
            var iterator = _tweetinviTestClient.Timelines.GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters
            {
                PageSize = 1,
                SinceId = tweet1.Id - 1
            });

            var retweets = new List<ITweet>();
            while (!iterator.Completed)
            {
                var retweetsPage = await iterator.NextPageAsync();
                retweets.AddRange(retweetsPage);
            }

            await tweet1.DestroyAsync();
            await tweet2.DestroyAsync();

            // assert
            Assert.True(retweets.Select(x => x.Id).ToArray().ContainsSameObjectsAs(new[] { tweet1.Id, tweet2.Id }));
        }
    }
}