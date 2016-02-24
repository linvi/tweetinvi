using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class TrackedStream : TwitterStream, ITrackedStream
    {
        public event EventHandler<MatchedTweetReceivedEventArgs> TweetReceived;
        public event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;
        public event EventHandler<TweetEventArgs> NonMatchingTweetReceived;

        protected readonly IStreamTrackManager<ITweet> _streamTrackManager;
        protected readonly IJsonObjectConverter _jsonObjectConverter;
        protected readonly ITweetFactory _tweetFactory;
        protected readonly ISynchronousInvoker _synchronousInvoker;

        private readonly ISingleAggregateExceptionThrower _singleAggregateExceptionThrower;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        public override event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        public TrackedStream(
            IStreamTrackManager<ITweet> streamTrackManager,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            IStreamResultGenerator streamResultGenerator,
            ITweetFactory tweetFactory,
            ISynchronousInvoker synchronousInvoker,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory,
            ISingleAggregateExceptionThrower singleAggregateExceptionThrower)

            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, customRequestParameters)
        {
            _streamTrackManager = streamTrackManager;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetFactory = tweetFactory;
            _synchronousInvoker = synchronousInvoker;
            _singleAggregateExceptionThrower = singleAggregateExceptionThrower;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public void StartStream(string url)
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(() => StartStreamAsync(url));
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamAsync(string url)
        {
            Func<ITwitterQuery> generateTwitterQuery = delegate
            {
                var queryBuilder = new StringBuilder(url);
                AddBaseParametersToQuery(queryBuilder);

                return _twitterQueryFactory.Create(queryBuilder.ToString(), HttpMethod.GET, Credentials);
            };

            Action<string> generateTweetDelegate = json =>
            {
                RaiseJsonObjectReceived(json);

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var detectedTracksAndActions = _streamTrackManager.GetMatchingTracksAndActions(tweet.Text);
                var detectedTracks = detectedTracksAndActions.Select(x => x.Item1);

                var eventArgs = new MatchedTweetReceivedEventArgs(tweet)
                {
                    MatchingTracks = detectedTracks.ToArray(),
                };

                if (detectedTracksAndActions.Any())
                {
                    eventArgs.MatchOn = MatchOn.TweetText;

                    RaiseTweetReceived(eventArgs);
                    RaiseMatchingTweetReceived(eventArgs);
                }
                else
                {
                    RaiseTweetReceived(eventArgs);
                    RaiseNonMatchingTweetReceived(new TweetEventArgs(tweet));
                }
            };

            await _streamResultGenerator.StartStreamAsync(generateTweetDelegate, generateTwitterQuery);
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