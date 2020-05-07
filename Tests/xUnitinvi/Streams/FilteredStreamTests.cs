using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Xunit;
using xUnitinvi.TestHelpers;
using TweetinviContainer = Tweetinvi.Injectinvi.TweetinviContainer;

namespace xUnitinvi.Streams
{
    public class FilteredStreamTests
    {
        private Tuple<IFilteredStream, Func<Action<string>>> InitForCatchingJsonEvents()
        {
            // arrange
            var fakeStreamResultGenerator = A.Fake<IStreamResultGenerator>();

            var container = new TweetinviContainer();
            container.BeforeRegistrationCompletes += (sender, args) =>
            {
                container.RegisterInstance(typeof(IStreamResultGenerator), fakeStreamResultGenerator);
            };
            container.Initialize();

            Action<string> jsonReceivedCallback = null;

            A.CallTo(() => fakeStreamResultGenerator.StartStreamAsync(It.IsAny<Action<string>>(), It.IsAny<Func<ITwitterRequest>>()))
                .ReturnsLazily(callInfo =>
                {
                    jsonReceivedCallback = callInfo.Arguments.Get<Action<string>>("onJsonReceivedCallback");
                    return Task.CompletedTask;
                });

            var client = new TwitterClient(A.Fake<ITwitterCredentials>(), new TwitterClientParameters
            {
                Container = container
            });

            var fs = client.Streams.CreateFilteredStream();
            return new Tuple<IFilteredStream, Func<Action<string>>>(fs, () => jsonReceivedCallback);
        }

        [Fact]
        public async Task Tweet_MatchOn_TextAsync()
        {
            // arrange
            var tuple = InitForCatchingJsonEvents();
            var fs = tuple.Item1;


            // act
            fs.AddTrack("raison");

            var matchingTweetReceived = false;
            fs.MatchingTweetReceived += (sender, args) =>
            {
                // Assert.Equal(args.MatchOn, MatchOn.TweetText);
                Assert.Equal(args.MatchOn, MatchOn.TweetText);
                matchingTweetReceived = true;
            };

            await fs.StartStreamMatchingAnyConditionAsync();

            var json = File.ReadAllText("./Streams/FilteredStreamEvent.json");
            var jsonReceivedCallback = tuple.Item2();
            jsonReceivedCallback(json);

            // assert
            await Task.Delay(100);
            Assert.True(matchingTweetReceived);
        }

        [Fact]
        public async Task QuotedTweet_MatchOn_UrlEntitiesAsync()
        {
            // arrange
            var tuple = InitForCatchingJsonEvents();
            var fs = tuple.Item1;


            // act
            fs.AddTrack("twitter");

            var matchingTweetReceived = false;
            fs.MatchingTweetReceived += (sender, args) =>
            {
                // Assert.Equal(args.MatchOn, MatchOn.TweetText);
                Assert.Equal(args.QuotedTweetMatchOn, MatchOn.URLEntities);
                matchingTweetReceived = true;
            };

            await fs.StartStreamMatchingAnyConditionAsync();

            var json = File.ReadAllText("./Streams/FilteredStreamEvent.json");
            var jsonReceivedCallback = tuple.Item2();
            jsonReceivedCallback(json);

            // assert
            await Task.Delay(100);
            Assert.True(matchingTweetReceived);
        }
    }
}