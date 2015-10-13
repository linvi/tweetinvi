using System;
using System.Collections.Generic;
using Tweetinvi.Core.Credentials;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Streaminvi.Parameters;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ITwitterStream
    {
        event EventHandler StreamStarted;
        event EventHandler StreamResumed;
        event EventHandler StreamPaused;
        event EventHandler<StreamExceptionEventArgs> StreamStopped;

        event EventHandler<TweetDeletedEventArgs> TweetDeleted;
        event EventHandler<TweetLocationDeletedEventArgs> TweetLocationInfoRemoved;
        event EventHandler<DisconnectedEventArgs> DisconnectMessageReceived;
        event EventHandler<TweetWitheldEventArgs> TweetWitheld;
        event EventHandler<UserWitheldEventArgs> UserWitheld;

        event EventHandler<LimitReachedEventArgs> LimitReached;

        /// <summary>
        /// Inform the user that the stream is not read fast enough and that if this continues,
        /// the stream will be disconnected when the buffered queue is full.
        /// 
        /// The StallWarning parameter needs to be set to true for this event to be raised.
        /// </summary>
        event EventHandler<WarningFallingBehindEventArgs> WarningFallingBehindDetected;

        /// <summary>
        /// An event that is not handled by Tweetinvi have just been received!
        /// </summary>
        event EventHandler<UnmanagedMessageReceivedEventArgs> UnmanagedEventReceived;

        /// <summary>
        /// Informs that we have received some json from the Twitter stream.
        /// </summary>
        event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        /// <summary>
        /// Get the current state of the stream
        /// </summary>
        StreamState StreamState { get; }

        /// <summary>
        /// Resume a stopped Stream
        /// </summary>
        void ResumeStream();

        /// <summary>
        /// Pause a running Stream
        /// </summary>
        void PauseStream();

        /// <summary>
        /// Stop a running or paused stream
        /// </summary>
        void StopStream();

        string[] FilteredLanguages { get; }
        void AddTweetLanguageFilter(string language);
        void AddTweetLanguageFilter(Language language);
        void RemoveTweetLanguageFilter(string language);
        void RemoveTweetLanguageFilter(Language language);
        void ClearTweetLanguageFilters();

        bool StallWarnings { get; set; }
        StreamFilterLevel FilterLevel { get; set; }

        List<Tuple<string, string>> CustomQueryParameters { get; }
        string FormattedCustomQueryParameters { get; }
        ITwitterCredentials Credentials { get; set; }

        void AddCustomQueryParameter(string name, string value);
        void ClearCustomQueryParameters();
    }
}