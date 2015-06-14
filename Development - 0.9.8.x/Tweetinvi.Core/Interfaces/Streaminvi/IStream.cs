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
        event EventHandler StreamStarted;
        event EventHandler StreamResumed;
        event EventHandler StreamPaused;
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