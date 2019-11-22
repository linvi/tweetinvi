using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Xunit;
using Xunit.Abstractions;

namespace xUnitinvi.IntegrationTests
{
    public class TimelineIntegrationTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly ITwitterClient _tweetinviApiClient;
        private readonly ITwitterClient _tweetinviTestClient;

        public TimelineIntegrationTests(ITestOutputHelper logger)
        {
            _logger = logger;

            _logger.WriteLine(DateTime.Now.ToLongTimeString());

            _tweetinviApiClient = new TwitterClient(IntegrationTestConfig.TweetinviApi.Credentials);
            _tweetinviTestClient = new TwitterClient(IntegrationTestConfig.TweetinviTest.Credentials);

            TweetinviEvents.QueryBeforeExecute += (sender, args) => { _logger.WriteLine(args.Url); };
        }

        [Fact]
        public async Task RunAllAccountTests()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            _logger.WriteLine($"Starting {nameof(HomeTimeLine)}");
            await HomeTimeLine().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(HomeTimeLine)} succeeded");

            _logger.WriteLine($"Starting {nameof(UserTimeline)}");
            await UserTimeline().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(UserTimeline)} succeeded");

            _logger.WriteLine($"Starting {nameof(RetweetsOfMeTimeline)}");
            await RetweetsOfMeTimeline().ConfigureAwait(false);
            _logger.WriteLine($"{nameof(RetweetsOfMeTimeline)} succeeded");
        }

        [Fact]
        public async Task HomeTimeLine()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            // arrange
            var testUser = await _tweetinviTestClient.Account.GetAuthenticatedUser();
            var tweetinviUser = await _tweetinviApiClient.Account.GetAuthenticatedUser();
            var friendsBeforeAdd = await _tweetinviApiClient.Users.GetFriendIds(tweetinviUser).MoveToNextPage();
            var alreadyFollowing = friendsBeforeAdd.Contains(testUser.Id.Value);

            if (!alreadyFollowing)
            {
                await _tweetinviApiClient.Account.FollowUser(testUser);
            }

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");

            var iterator = _tweetinviApiClient.Timeline.GetHomeTimelineIterator(new GetHomeTimelineParameters
            {
                PageSize = 5,
            });

            var page1 = await iterator.MoveToNextPage();
            var page2 = await iterator.MoveToNextPage();

            await tweet1.Destroy();

            if (!alreadyFollowing)
            {
                await _tweetinviApiClient.Account.UnFollowUser(testUser);
            }

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task UserTimeline()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweetinviTest = IntegrationTestConfig.TweetinviTest.AccountId;

            var iterator = _tweetinviApiClient.Timeline.GetUserTimelineIterator(new GetUserTimelineParameters(tweetinviTest)
            {
                PageSize = 5,
            });

            var page1 = await iterator.MoveToNextPage();
            var page2 = await iterator.MoveToNextPage();

            await tweet1.Destroy();

            // assert
            Assert.True(page1.Select(x => x.Id).Contains(tweet1.Id) || page2.Select(x => x.Id).Contains(tweet1.Id));
        }

        [Fact]
        public async Task MentionsTimeline()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            // act
            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("The new @tweetinviapi is the great!");

            var iterator = _tweetinviApiClient.Timeline.GetMentionsTimelineIterator();

            var page1 = await iterator.MoveToNextPage();

            await tweet1.Destroy();

            // assert
            Assert.Contains(tweet1.Id, page1.Select(x => x.Id));
        }


        [Fact]
        public async Task RetweetsOfMeTimeline()
        {
            if (!IntegrationTestConfig.ShouldRunIntegrationTests)
                return;

            var tweet1 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 1!");
            var tweet2 = await _tweetinviTestClient.Tweets.PublishTweet("tweet 2");

            await _tweetinviApiClient.Tweets.PublishRetweet(tweet1);
            await _tweetinviApiClient.Tweets.PublishRetweet(tweet2);

            var iterator = _tweetinviTestClient.Timeline.GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters
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