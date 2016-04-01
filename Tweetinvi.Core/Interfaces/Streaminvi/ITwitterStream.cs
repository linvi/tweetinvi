using System;
using System.Collections.Generic;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Streaminvi.Parameters;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ITwitterStream
    {
        /// <summary>
        /// The stream has been started.
        /// </summary>
        event EventHandler StreamStarted;

        /// <summary>
        /// The stream has been resumed after being paused.
        /// </summary>
        event EventHandler StreamResumed;

        /// <summary>
        /// The stream has been paused.
        /// </summary>
        event EventHandler StreamPaused;

        /// <summary>
        /// The stream has been stopped. This can be due to an exception.
        /// You can verify this with the exception infos provided in the event args.
        /// </summary>
        event EventHandler<StreamExceptionEventArgs> StreamStopped;

        /// <summary>
        /// A tweet has been deleted.
        /// </summary>
        event EventHandler<TweetDeletedEventArgs> TweetDeleted;

        /// <summary>
        /// The location information of a tweet has been deleted.
        /// </summary>
        event EventHandler<TweetLocationDeletedEventArgs> TweetLocationInfoRemoved;

        /// <summary>
        /// The stream has been disconnected. This is different from being stopped
        /// as it is the Twitter stream endpoint that let you know that they are disconnecting 
        /// you from any reason available in the event args.
        /// </summary>
        event EventHandler<DisconnectedEventArgs> DisconnectMessageReceived;

        /// <summary>
        /// A tweet matching your criteria has been identified by the stream api but it
        /// could not be received because it has been forbidden in your country.
        /// </summary>
        event EventHandler<TweetWitheldEventArgs> TweetWitheld;

        /// <summary>
        /// A user matching your criteria has been identified by the stream api but it
        /// could not be received because he was blocked in your country.
        /// </summary>
        event EventHandler<UserWitheldEventArgs> UserWitheld;

        /// <summary>
        /// Your stream has too broad parameters that result in receiving > 1% of the total tweets.
        /// You can identify the number of tweets that Twitter has not sent to you in the event args.
        /// </summary>
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
        /// Credentials that the stream will use. This can only be modified when the stream is stopped.
        /// </summary>
        ITwitterCredentials Credentials { get; set; }

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

        /// <summary>
        /// Languages that you want to receive. If empty all languages will be matched.
        /// </summary>
        string[] FilteredLanguages { get; }

        /// <summary>
        /// Add a language that you want the tweets to be filtered by.
        /// </summary>
        void AddTweetLanguageFilter(string language);

        /// <summary>
        /// Add a language that you want the tweets to be filtered by.
        /// </summary>
        void AddTweetLanguageFilter(Language language);

        /// <summary>
        /// Tweets with the specified language will no longer be received.
        /// </summary>
        void RemoveTweetLanguageFilter(string language);

        /// <summary>
        /// Tweets with the specified language will no longer be received.
        /// </summary>
        void RemoveTweetLanguageFilter(Language language);

        /// <summary>
        /// No filter on the languages will be applied.
        /// </summary>
        void ClearTweetLanguageFilters();

        /// <summary>
        /// Give you information regarding your connection. Twitter could let you know
        /// if the processing of the stream is too slow or if the connection is about to be dropped.
        /// </summary>
        bool StallWarnings { get; set; }

        /// <summary>
        /// Filter tweets containing violence, sex or any sensible subjects.
        /// </summary>
        StreamFilterLevel FilterLevel { get; set; }

        /// <summary>
        /// Custom parameters that will be appended to the stream query url.
        /// </summary>
        List<Tuple<string, string>> CustomQueryParameters { get; }

        /// <summary>
        /// A formatted version of the custom query parameters.
        /// </summary>
        string FormattedCustomQueryParameters { get; }
        
        /// <summary>
        /// Append a custom query parameter to the query url.
        /// </summary>
        void AddCustomQueryParameter(string name, string value);

        /// <summary>
        /// Remove all custom query parameters.
        /// </summary>
        void ClearCustomQueryParameters();
    }
}