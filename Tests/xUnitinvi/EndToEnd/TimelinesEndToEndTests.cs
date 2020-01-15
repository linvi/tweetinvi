using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
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
        public async Task HomeTimeLine()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // arrange
            var testUser = await _tweetinviTestClient.Account.GetAuthenticatedUser();
            var tweetinviUser = await _tweetinviClient.Account.GetAuthenticatedUser();
            var friendsBeforeAdd = await _tweetinviClient.Users.GetFriendIds(tweetinviUser).MoveToNextPage();
            var alreadyFollowing = friendsBeforeAdd.Contains(testUser.Id.Value);

            if (!alreadyFollowing)
            {
                await _tweetinviClient.Account.FollowUser(testUser);
            }

            // act - pre-cleanup

            await Task.Delay(1000).ConfigureAwait(false); // time required for timeline to be generated
            var recentTweetIterators = _tweetinviClient.Timelines.GetHomeTimelineIterator();
            var recentTweets = await recentTweetIterators.MoveToNextPage();
            var tweetToDelete = recentTweets.FirstOrDefault(x => x.Text == "tweet 1!");

            if (tweetToDelete != null)
            {
                await _tweetinviTestClient.Tweets.DestroyTweet(tweetToDelete);
            }

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");

            await Task.Delay(2000).ConfigureAwait(false); // time required for timeline to be generated
            var iterator = _tweetinviClient.Timelines.GetHomeTimelineIterator(new GetHomeTimelineParameters
            {
                PageSize = 1,
            });

            var page1 = await iterator.MoveToNextPage();
            var page2 = await iterator.MoveToNextPage();

            await tweet1.Destroy();

            if (!alreadyFollowing)
            {
                await _tweetinviClient.Account.UnFollowUser(testUser);
            }

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task UserTimeline()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweetinviTest = EndToEndTestConfig.TweetinviTest.AccountId;

            var iterator = _tweetinviClient.Timelines.GetUserTimelineIterator(new GetUserTimelineParameters(tweetinviTest)
            {
                PageSize = 5,
            });

            var page1 = await iterator.MoveToNextPage();

            IEnumerable<ITweet> page2 = new ITweet[] { };
            if (!iterator.Completed)
            {
                page2 = await iterator.MoveToNextPage();
            }

            await tweet1.Destroy();

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task MentionsTimeline()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("The new @tweetinviapi is the great!");

            var iterator = _tweetinviClient.Timelines.GetMentionsTimelineIterator();

            var page1 = await iterator.MoveToNextPage();

            await tweet1.Destroy();

            // assert
            Assert.Contains(tweet1.Id, page1.Select(x => x.Id));
        }


        [Fact]
        public async Task RetweetsOfMeTimeline()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweet2 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 2");

            await _tweetinviClient.Tweets.PublishRetweet(tweet1);
            await _tweetinviClient.Tweets.PublishRetweet(tweet2);

            var iterator = _tweetinviTestClient.Timelines.GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters
            {
                PageSize = 1,
                SinceId = tweet1.Id - 1
            });

            var retweets = new List<ITweet>();
            while (!iterator.Completed)
            {
                var retweetsPage = await iterator.MoveToNextPage();
                retweets.AddRange(retweetsPage);
            }

            await tweet1.Destroy();
            await tweet2.Destroy();

            // assert
            Assert.True(retweets.Select(x => x.Id).ToArray().ContainsSameObjectsAs(new[] { tweet1.Id, tweet2.Id }));
        }
    }
}