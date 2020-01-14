using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class TrackedStream : TwitterStream, ITrackedStream
    {
        public event EventHandler<MatchedTweetReceivedEventArgs> TweetReceived;
        public event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;
        public event EventHandler<TweetEventArgs> NonMatchingTweetReceived;

        private readonly ITwitterClient _client;
        private readonly IStreamTrackManager<ITweet> _streamTrackManager;
        private readonly ITwitterClientFactories _factories;

        public override event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        public TrackedStream(
            ITwitterClient client,
            IStreamTrackManager<ITweet> streamTrackManager,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            IStreamResultGenerator streamResultGenerator,
            ITwitterClientFactories factories,
            ICreateTrackedStreamParameters createTrackedStreamParameters)

            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, createTrackedStreamParameters)
        {
            _client = client;
            _streamTrackManager = streamTrackManager;
            _factories = factories;
        }

        public async Task StartStreamAsync(string url)
        {
            ITwitterRequest createTwitterRequest()
            {
                var queryBuilder = new StringBuilder(url);
                AddBaseParametersToQuery(queryBuilder);

                var request = _client.CreateRequest();
                request.Query.Url = queryBuilder.ToString();
                request.Query.HttpMethod = HttpMethod.GET;
                return request;
            }

            void onJsonReceived(string json)
            {
                RaiseJsonObjectReceived(json);

                if (IsEvent(json))
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var tweet = _factories.CreateTweet(json);

                var detectedTracksAndActions = _streamTrackManager.GetMatchingTracksAndActions(tweet.FullText);
                var detectedTracks = detectedTracksAndActions.Select(x => x.Item1);

                var eventArgs = new MatchedTweetReceivedEventArgs(tweet, json) { MatchingTracks = detectedTracks.ToArray(), };

                if (detectedTracksAndActions.Any())
                {
                    eventArgs.MatchOn = MatchOn.TweetText;

                    RaiseTweetReceived(eventArgs);
                    RaiseMatchingTweetReceived(eventArgs);
                }
                else
                {
                    RaiseTweetReceived(eventArgs);
                    RaiseNonMatchingTweetReceived(new TweetEventArgs(tweet, json));
                }
            }

            await _streamResultGenerator.StartStream(onJsonReceived, createTwitterRequest);
        }

        protected void RaiseJsonObjectReceived(string json)
        {
            this.Raise(JsonObjectReceived, new JsonObjectEventArgs(json));
        }

        public int TracksCount
        {
            get { return _streamTrackManager.TracksCount; }
        }

        public int MaxTracks
        {
            get { return _streamTrackManager.MaxTracks; }
        }

        public Dictionary<string, Action<ITweet>> Tracks
        {
            get { return _streamTrackManager.Tracks; }
        }

        public void AddTrack(string track, Action<ITweet> trackReceived = null)
        {
            if (_streamResultGenerator.StreamState != StreamState.Stop)
            {
                throw new InvalidOperationException(Resources.TrackedStream_ModifyTracks_NotStoppedException_Description);
            }

            _streamTrackManager.AddTrack(track, trackReceived);
        }

        public void RemoveTrack(string track)
        {
            if (_streamResultGenerator.StreamState != StreamState.Stop)
            {
                throw new InvalidOperationException(Resources.TrackedStream_ModifyTracks_NotStoppedException_Description);
            }

            _streamTrackManager.RemoveTrack(track);
        }

        public bool ContainsTrack(string track)
        {
            return _streamTrackManager.ContainsTrack(track);
        }

        public void ClearTracks()
        {
            if (_streamResultGenerator.StreamState != StreamState.Stop)
            {
                throw new InvalidOperationException(Resources.TrackedStream_ModifyTracks_NotStoppedException_Description);
            }

            _streamTrackManager.ClearTracks();
        }

        protected void RaiseTweetReceived(MatchedTweetReceivedEventArgs eventArgs)
        {
            this.Raise(TweetReceived, eventArgs);
        }

        protected void RaiseMatchingTweetReceived(MatchedTweetReceivedEventArgs eventArgs)
        {
            this.Raise(MatchingTweetReceived, eventArgs);
        }

        protected void RaiseNonMatchingTweetReceived(TweetEventArgs eventArgs)
        {
            this.Raise(NonMatchingTweetReceived, eventArgs);
        }
    }
}