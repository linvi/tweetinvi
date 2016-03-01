using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// Set of methods to control and manage a stream
    /// </summary>
    public interface IStream<T>
    {
        /// <summary>
        /// The stream has started.
        /// </summary>
        event EventHandler StreamStarted;

        /// <summary>
        /// The stream has resumed after being paused.
        /// </summary>
        event EventHandler StreamResumed;

        /// <summary>
        /// The stream has paused.
        /// </summary>
        event EventHandler StreamPaused;

        /// <summary>
        /// The stream has stopped. This can be due by an exception.
        /// If it is the case the event args will contain the exception details.
        /// </summary>
        event EventHandler<GenericEventArgs<Exception>> StreamStopped;

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
    }
}