using System;
using System.Threading.Tasks;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Xunit;
using Xunit.Abstractions;
using xUnitinvi.TestHelpers;

namespace xUnitinvi.EndToEnd
{
    [Collection("EndToEndTests")]
    public class StreamEndToEndTests : TweetinviTest
    {
        public StreamEndToEndTests(ITestOutputHelper logger) : base(logger)
        {
        }

        [Fact]
        public async Task SampleStream()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var stream = _tweetinviTestClient.Streams.CreateSampleStream();
            ITweet tweet = null;
            StreamStoppedEventArgs streamStoppedEventArgs = null;

            stream.TweetReceived += (sender, args) =>
            {
                tweet = args.Tweet;
                _logger.WriteLine("Tweet received!");
                _logger.WriteLine(tweet.ToString());
                stream.StopStream();
            };

            stream.JsonObjectReceived += (sender, args) => { _logger.WriteLine(args.Json); };
            stream.StreamStopped += (sender, args) => { streamStoppedEventArgs = args; };

            var runStreamTask = Task.Run(async () =>
            {
                _logger.WriteLine("Before starting stream");
                await stream.StartStream().ConfigureAwait(false);
                _logger.WriteLine("Stream completed");
            });

            var delayTask = Task.Delay(TimeSpan.FromSeconds(15));

            var task = await Task.WhenAny(runStreamTask, delayTask).ConfigureAwait(false);

            if (task != runStreamTask)
            {
                throw new TimeoutException();
            }

            _logger.WriteLine(streamStoppedEventArgs.Exception?.ToString() ?? "No exception");
            _logger.WriteLine(streamStoppedEventArgs.DisconnectMessage?.ToString() ?? "No disconnect message");

            Assert.Null(streamStoppedEventArgs.Exception);
            Assert.Null(streamStoppedEventArgs.DisconnectMessage);
            Assert.NotNull(tweet);

            await Task.Delay(4000); // this is for preventing Enhance Your Calm message from Twitter
        }

        [Fact]
        public async Task FilteredStream()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var stream = _tweetinviTestClient.Streams.CreateFilteredStream();
            stream.AddTrack("twitter");

            ITweet tweet = null;
            StreamStoppedEventArgs streamStoppedEventArgs = null;

            stream.MatchingTweetReceived += (sender, args) =>
            {
                tweet = args.Tweet;
                _logger.WriteLine($"Tweet matched via {args.MatchOn.ToString()}");
                _logger.WriteLine(tweet.ToString());
                stream.StopStream();
            };

            stream.JsonObjectReceived += (sender, args) => { _logger.WriteLine(args.Json); };
            stream.StreamStopped += (sender, args) => { streamStoppedEventArgs = args; };

            var runStreamTask = Task.Run(async () =>
            {
                _logger.WriteLine("Before starting stream");
                await stream.StartStreamMatchingAllConditions().ConfigureAwait(false);
                _logger.WriteLine("Stream completed");
            });

            var delayTask = Task.Delay(TimeSpan.FromSeconds(15));

            var task = await Task.WhenAny(runStreamTask, delayTask).ConfigureAwait(false);

            if (task != runStreamTask)
            {
                throw new TimeoutException();
            }

            _logger.WriteLine(streamStoppedEventArgs.Exception?.ToString() ?? "No exception");
            _logger.WriteLine(streamStoppedEventArgs.DisconnectMessage?.ToString() ?? "No disconnect message");

            Assert.Null(streamStoppedEventArgs.Exception);
            Assert.Null(streamStoppedEventArgs.DisconnectMessage);
            Assert.NotNull(tweet);

            await Task.Delay(4000); // this is for preventing Enhance Your Calm message from Twitter
        }

        [Fact]
        public async Task TweetStream()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var stream = _tweetinviTestClient.Streams.CreateTweetStream();

            ITweet tweet = null;
            StreamStoppedEventArgs streamStoppedEventArgs = null;

            stream.TweetReceived += (sender, args) =>
            {
                tweet = args.Tweet;
                _logger.WriteLine(tweet.ToString());
                stream.StopStream();
            };

            stream.JsonObjectReceived += (sender, args) => { _logger.WriteLine(args.Json); };
            stream.StreamStopped += (sender, args) => { streamStoppedEventArgs = args; };

            var runStreamTask = Task.Run(async () =>
            {
                _logger.WriteLine("Before starting stream");
                await stream.StartStream("https://stream.twitter.com/1.1/statuses/sample.json").ConfigureAwait(false);
                _logger.WriteLine("Stream completed");
            });

            var delayTask = Task.Delay(TimeSpan.FromSeconds(15));

            var task = await Task.WhenAny(runStreamTask, delayTask).ConfigureAwait(false);

            if (task != runStreamTask)
            {
                throw new TimeoutException();
            }

            _logger.WriteLine(streamStoppedEventArgs.Exception?.ToString() ?? "No exception");
            _logger.WriteLine(streamStoppedEventArgs.DisconnectMessage?.ToString() ?? "No disconnect message");

            Assert.Null(streamStoppedEventArgs.Exception);
            Assert.Null(streamStoppedEventArgs.DisconnectMessage);
            Assert.NotNull(tweet);

            await Task.Delay(4000); // this is for preventing Enhance Your Calm message from Twitter
        }

        [Fact]
        public async Task TrackedStream()
        {
            if (!EndToEndTestConfig.ShouldRunEndToEndTests)
                return;

            var stream = _tweetinviTestClient.Streams.CreateTrackedStream();
            stream.AddTrack("twitter");

            ITweet tweet = null;
            StreamStoppedEventArgs streamStoppedEventArgs = null;

            stream.MatchingTweetReceived += (sender, args) =>
            {
                tweet = args.Tweet;
                _logger.WriteLine($"Tweet matched via {args.MatchOn.ToString()}");
                _logger.WriteLine(tweet.ToString());
                stream.StopStream();
            };

            stream.JsonObjectReceived += (sender, args) => { _logger.WriteLine(args.Json); };
            stream.StreamStopped += (sender, args) => { streamStoppedEventArgs = args; };

            var runStreamTask = Task.Run(async () =>
            {
                _logger.WriteLine("Before starting stream");
                await stream.StartStreamAsync("https://stream.twitter.com/1.1/statuses/filter.json?track=twitter").ConfigureAwait(false);
                _logger.WriteLine("Stream completed");
            });

            var delayTask = Task.Delay(TimeSpan.FromSeconds(15));

            var task = await Task.WhenAny(runStreamTask, delayTask).ConfigureAwait(false);

            if (task != runStreamTask)
            {
                throw new TimeoutException();
            }

            _logger.WriteLine(streamStoppedEventArgs.Exception?.ToString() ?? "No exception");
            _logger.WriteLine(streamStoppedEventArgs.DisconnectMessage?.ToString() ?? "No disconnect message");

            Assert.Null(streamStoppedEventArgs.Exception);
            Assert.Null(streamStoppedEventArgs.DisconnectMessage);
            Assert.NotNull(tweet);

            await Task.Delay(4000); // this is for preventing Enhance Your Calm message from Twitter
        }
    }
}