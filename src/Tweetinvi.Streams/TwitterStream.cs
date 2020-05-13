using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Parameters;
using Tweetinvi.Streams.Model;

namespace Tweetinvi.Streams
{
    public abstract class TwitterStream : ITwitterStream
    {
        protected IStreamResultGenerator _streamResultGenerator { get; }
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IJObjectStaticWrapper _jObjectWrapper;
        private readonly ICustomRequestParameters _customRequestParameters;

        private readonly List<string> _filteredLanguages;
        private readonly Dictionary<string, Action<JToken>> _streamEventsActions;

        protected TwitterStream(
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectWrapper,
            ICustomRequestParameters customRequestParameters)
        {
            _streamResultGenerator = streamResultGenerator;
            _jsonObjectConverter = jsonObjectConverter;
            _jObjectWrapper = jObjectWrapper;
            _customRequestParameters = customRequestParameters;

            _streamEventsActions = new Dictionary<string, Action<JToken>>();
            _filteredLanguages = new List<string>();

            StallWarnings = true;

            InitializeStreamEventsActions();
        }

        public ITwitterExecutionContext ExecutionContext { get; set; }

        private TweetMode? _tweetMode;

        public TweetMode? TweetMode
        {
            get => _tweetMode;
            set
            {
                if (StreamState != StreamState.Stop)
                {
                    throw new InvalidOperationException("TweetMode cannot be changed while the stream is running.");
                }

                _tweetMode = value;
            }
        }

        private void InitializeStreamEventsActions()
        {
            _streamEventsActions.Add("delete", TryRaiseTweetDeleted);
            _streamEventsActions.Add("scrub_geo", TryRaiseTweetLocationRemoved);
            _streamEventsActions.Add("disconnect", TryRaiseDisconnectMessageReceived);
            _streamEventsActions.Add("limit", TryRaiseLimitReached);
            _streamEventsActions.Add("status_withheld", TryRaiseTweetWitheld);
            _streamEventsActions.Add("user_withheld", TryRaiseUserWitheld);
            _streamEventsActions.Add("warning", TryRaiseWarning);
        }

        public event EventHandler StreamStarted
        {
            add => _streamResultGenerator.StreamStarted += value;
            remove => _streamResultGenerator.StreamStarted -= value;
        }

        public event EventHandler StreamResumed
        {
            add => _streamResultGenerator.StreamResumed += value;
            remove => _streamResultGenerator.StreamResumed -= value;
        }

        public event EventHandler StreamPaused
        {
            add => _streamResultGenerator.StreamPaused += value;
            remove => _streamResultGenerator.StreamPaused -= value;
        }

        public event EventHandler<StreamStoppedEventArgs> StreamStopped
        {
            add => _streamResultGenerator.StreamStopped += value;
            remove => _streamResultGenerator.StreamStopped -= value;
        }

        public event EventHandler KeepAliveReceived
        {
            add => _streamResultGenerator.KeepAliveReceived += value;
            remove => _streamResultGenerator.KeepAliveReceived -= value;
        }

        public event EventHandler<TweetDeletedEvent> TweetDeleted;
        public event EventHandler<TweetLocationDeletedEventArgs> TweetLocationInfoRemoved;
        public event EventHandler<DisconnectedEventArgs> DisconnectMessageReceived;
        public event EventHandler<TweetWitheldEventArgs> TweetWitheld;
        public event EventHandler<UserWitheldEventArgs> UserWitheld;

        public event EventHandler<LimitReachedEventArgs> LimitReached;
        public event EventHandler<WarningFallingBehindEventArgs> WarningFallingBehindDetected;
        public event EventHandler<UnsupportedMessageReceivedEvent> UnmanagedEventReceived;
        public abstract event EventHandler<StreamEventReceivedArgs> EventReceived;

        // Stream State
        public StreamState StreamState => _streamResultGenerator.StreamState;

        public void Resume()
        {
            _streamResultGenerator.ResumeStream();
        }

        public void Pause()
        {
            _streamResultGenerator.PauseStream();
        }

        public void Stop()
        {
            _streamResultGenerator.StopStream();
        }

        protected void StopStream(Exception ex)
        {
            _streamResultGenerator.StopStream(ex, null);
        }

        // Parameters
        protected void AddBaseParametersToQuery(StringBuilder queryBuilder)
        {
            if (_filteredLanguages.Any())
            {
                var languages = string.Join(", ", _filteredLanguages.Select(x => x.ToLowerInvariant()));
                queryBuilder.AddParameterToQuery("language", languages);
            }

            if (StallWarnings)
            {
                queryBuilder.AddParameterToQuery("stall_warnings", "true");
            }

            if (FilterLevel != StreamFilterLevel.None)
            {
                queryBuilder.AddParameterToQuery("filter_level", FilterLevel.ToString().ToLowerInvariant());
            }

            queryBuilder.AddParameterToQuery("tweet_mode", TweetMode.ToString().ToLowerInvariant());
            queryBuilder.AddFormattedParameterToQuery(_customRequestParameters.FormattedCustomQueryParameters);
        }

        public string[] FilteredLanguages => _filteredLanguages.ToArray();

        public void AddTweetLanguageFilter(string language)
        {
            if (!_filteredLanguages.Contains(language))
            {
                _filteredLanguages.Add(language);
            }
        }

        public void AddTweetLanguageFilter(LanguageFilter language)
        {
            AddTweetLanguageFilter(language.GetLanguageCode());
        }

        public void RemoveTweetLanguageFilter(string language)
        {
            _filteredLanguages.Remove(language);
        }

        public void RemoveTweetLanguageFilter(LanguageFilter language)
        {
            RemoveTweetLanguageFilter(language.GetLanguageCode());
        }

        public void ClearTweetLanguageFilters()
        {
            _filteredLanguages.Clear();
        }

        public bool StallWarnings { get; set; }

        public StreamFilterLevel FilterLevel { get; set; }

        #region Custom Query Parameters

        public List<Tuple<string, string>> CustomQueryParameters => _customRequestParameters.CustomQueryParameters;
        public string FormattedCustomQueryParameters => _customRequestParameters.FormattedCustomQueryParameters;

        public void AddCustomQueryParameter(string name, string value)
        {
            _customRequestParameters.AddCustomQueryParameter(name, value);
        }

        public void ClearCustomQueryParameters()
        {
            _customRequestParameters.ClearCustomQueryParameters();
        }

        #endregion

        // Events
        protected void TryInvokeGlobalStreamMessages(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return;
            }

            var jsonObject = _jObjectWrapper.GetJobjectFromJson(json);
            var jsonRootToken = jsonObject.Children().First();
            var messageType = _jObjectWrapper.GetNodeRootName(jsonRootToken);

            if (_streamEventsActions.ContainsKey(messageType))
            {
                var messageInfo = jsonObject[messageType];
                _streamEventsActions[messageType].Invoke(messageInfo);
            }
            else
            {
                var unmanagedMessageEventArgs = new UnsupportedMessageReceivedEvent(json);
                this.Raise(UnmanagedEventReceived, unmanagedMessageEventArgs);
            }
        }

        protected bool IsEvent(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return false;
            }

            var jsonObject = _jObjectWrapper.GetJobjectFromJson(json);
            var jsonRootChildren = jsonObject.Children().ToArray();

            return jsonRootChildren.Length <= 1;
        }

        private void TryRaiseTweetDeleted(JToken jToken)
        {
            jToken = jToken["status"];
            if (jToken == null)
            {
                return;
            }

            var deletedTweetInfo = _jsonObjectConverter.Deserialize<TweetDeletedInfo>(jToken.ToString());
            var deletedTweetEventArgs = new TweetDeletedEvent(new AccountActivityEvent<long>(deletedTweetInfo.Id)
            {
            }, deletedTweetInfo.UserId);
            // var deletedTweetEventArgs = new TweetDeletedEvent()
            // {
            //     TweetId = deletedTweetInfo.Id,
            //     UserId = deletedTweetInfo.UserId
            // };

            this.Raise(TweetDeleted, deletedTweetEventArgs);
        }

        private void TryRaiseTweetLocationRemoved(JToken jToken)
        {
            var tweetLocationDeleted = _jsonObjectConverter.Deserialize<TweetLocationRemovedInfo>(jToken.ToString());
            var tweetLocationDeletedEventArgs = new TweetLocationDeletedEventArgs(tweetLocationDeleted);
            this.Raise(TweetLocationInfoRemoved, tweetLocationDeletedEventArgs);
        }

        private void TryRaiseDisconnectMessageReceived(JToken jToken)
        {
            var disconnectMessage = _jsonObjectConverter.Deserialize<DisconnectMessage>(jToken.ToString());
            var disconnectMessageEventArgs = new DisconnectedEventArgs(disconnectMessage);
            this.Raise(DisconnectMessageReceived, disconnectMessageEventArgs);
            _streamResultGenerator.StopStream(null, disconnectMessage);
        }

        private void TryRaiseLimitReached(JToken jToken)
        {
            var nbTweetsMissed = jToken.Value<int>("track");
            this.Raise(LimitReached, new LimitReachedEventArgs(nbTweetsMissed));
        }

        private void TryRaiseTweetWitheld(JToken jToken)
        {
            var info = _jsonObjectConverter.Deserialize<TweetWitheldInfo>(jToken.ToString());
            var eventArgs = new TweetWitheldEventArgs(info);
            this.Raise(TweetWitheld, eventArgs);
        }

        private void TryRaiseUserWitheld(JToken jToken)
        {
            var info = _jsonObjectConverter.Deserialize<UserWitheldInfo>(jToken.ToString());
            var eventArgs = new UserWitheldEventArgs(info);
            this.Raise(UserWitheld, eventArgs);
        }

        private void TryRaiseWarning(JToken jToken)
        {
            TryRaiseFallingBehindWarning(jToken);
        }

        private void TryRaiseFallingBehindWarning(JToken jsonWarning)
        {
            if (jsonWarning["percent_full"] != null)
            {
                var warningMessage = _jsonObjectConverter.Deserialize<WarningMessageFallingBehind>(jsonWarning.ToString());
                this.Raise(WarningFallingBehindDetected, new WarningFallingBehindEventArgs(warningMessage));
            }
        }
    }
}