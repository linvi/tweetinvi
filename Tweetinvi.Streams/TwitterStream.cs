using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Interfaces.Streaminvi.Parameters;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Streams.Model;

namespace Tweetinvi.Streams
{
    public abstract class TwitterStream : ITwitterStream
    {
        protected readonly IStreamResultGenerator _streamResultGenerator;
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

        private ITwitterCredentials _credentials;
        public ITwitterCredentials Credentials
        {
            get { return _credentials; }
            set
            {
                if (StreamState != StreamState.Stop)
                {
                    throw new InvalidOperationException("Credentials cannot be changed while the stream is running.");
                }

                _credentials = value;
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
            add { _streamResultGenerator.StreamStarted += value; }
            remove { _streamResultGenerator.StreamStarted -= value; }
        }
        public event EventHandler StreamResumed
        {
            add { _streamResultGenerator.StreamResumed += value; }
            remove { _streamResultGenerator.StreamResumed -= value; }
        }
        public event EventHandler StreamPaused
        {
            add { _streamResultGenerator.StreamPaused += value; }
            remove { _streamResultGenerator.StreamPaused -= value; }
        }
        public event EventHandler<StreamExceptionEventArgs> StreamStopped
        {
            add { _streamResultGenerator.StreamStopped += value; }
            remove { _streamResultGenerator.StreamStopped -= value; }
        }

        public event EventHandler<TweetDeletedEventArgs> TweetDeleted;
        public event EventHandler<TweetLocationDeletedEventArgs> TweetLocationInfoRemoved;
        public event EventHandler<DisconnectedEventArgs> DisconnectMessageReceived;
        public event EventHandler<TweetWitheldEventArgs> TweetWitheld;
        public event EventHandler<UserWitheldEventArgs> UserWitheld;

        public event EventHandler<LimitReachedEventArgs> LimitReached;
        public event EventHandler<WarningFallingBehindEventArgs> WarningFallingBehindDetected;
        public event EventHandler<UnmanagedMessageReceivedEventArgs> UnmanagedEventReceived;
        public abstract event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        // Stream State
        public StreamState StreamState
        {
            get { return _streamResultGenerator.StreamState; }
        }

        public void ResumeStream()
        {
            _streamResultGenerator.ResumeStream();
        }

        public void PauseStream()
        {
            _streamResultGenerator.PauseStream();
        }

        public void StopStream()
        {
            _streamResultGenerator.StopStream();
        }

        protected void StopStream(Exception ex)
        {
            _streamResultGenerator.StopStream(ex);
        }

        // Parameters
        protected virtual void AddBaseParametersToQuery(StringBuilder queryBuilder)
        {
            if (_filteredLanguages.Any())
            {
                queryBuilder.AddParameterToQuery("language", string.Join(Uri.EscapeDataString(", "), _filteredLanguages));
            }

            if (StallWarnings)
            {
                queryBuilder.AddParameterToQuery("stall_warnings", "true");
            }

            if (FilterLevel != StreamFilterLevel.None)
            {
                queryBuilder.AddParameterToQuery("filter_level", FilterLevel.ToString());
            }

            if (!string.IsNullOrEmpty(_customRequestParameters.FormattedCustomQueryParameters))
            {
                queryBuilder.Append(string.Format("&{0}", _customRequestParameters.FormattedCustomQueryParameters));
            }
        }

        public string[] FilteredLanguages { get { return _filteredLanguages.ToArray(); } }

        public void AddTweetLanguageFilter(string language)
        {
            if (!_filteredLanguages.Contains(language))
            {
                _filteredLanguages.Add(language);
            }
        }

        public void AddTweetLanguageFilter(Language language)
        {
            if (language != Language.Undefined)
            {
                AddTweetLanguageFilter(language.GetLanguageCode());
            }
        }

        public void RemoveTweetLanguageFilter(string language)
        {
            _filteredLanguages.Remove(language);
        }

        public void RemoveTweetLanguageFilter(Language language)
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

        public List<Tuple<string, string>> CustomQueryParameters { get { return _customRequestParameters.CustomQueryParameters; } }
        public string FormattedCustomQueryParameters { get { return _customRequestParameters.FormattedCustomQueryParameters; } }

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
                var unmanagedMessageEventArgs = new UnmanagedMessageReceivedEventArgs(json);
                this.Raise(UnmanagedEventReceived, unmanagedMessageEventArgs);
            }
        }

        private void TryRaiseTweetDeleted(JToken jToken)
        {
            jToken = jToken["status"];
            if (jToken == null)
            {
                return;
            }

            var deletedTweetInfo = _jsonObjectConverter.DeserializeObject<TweetDeletedInfo>(jToken.ToString());
            var deletedTweetEventArgs = new TweetDeletedEventArgs(deletedTweetInfo);
            this.Raise(TweetDeleted, deletedTweetEventArgs);
        }

        private void TryRaiseTweetLocationRemoved(JToken jToken)
        {
            var tweetLocationDeleted = _jsonObjectConverter.DeserializeObject<TweetLocationRemovedInfo>(jToken.ToString());
            var tweetLocationDeletedEventArgs = new TweetLocationDeletedEventArgs(tweetLocationDeleted);
            this.Raise(TweetLocationInfoRemoved, tweetLocationDeletedEventArgs);
        }

        private void TryRaiseDisconnectMessageReceived(JToken jToken)
        {
            var disconnectMessage = _jsonObjectConverter.DeserializeObject<DisconnectMessage>(jToken.ToString());
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
            var info = _jsonObjectConverter.DeserializeObject<TweetWitheldInfo>(jToken.ToString());
            var eventArgs = new TweetWitheldEventArgs(info);
            this.Raise(TweetWitheld, eventArgs);
        }

        private void TryRaiseUserWitheld(JToken jToken)
        {
            var info = _jsonObjectConverter.DeserializeObject<UserWitheldInfo>(jToken.ToString());
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
                var warningMessage = _jsonObjectConverter.DeserializeObject<WarningMessageFallingBehind>(jsonWarning.ToString());
                this.Raise(WarningFallingBehindDetected, new WarningFallingBehindEventArgs(warningMessage));
            }
        }
    }
}