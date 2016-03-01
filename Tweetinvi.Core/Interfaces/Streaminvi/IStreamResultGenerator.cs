using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    /// <summary>
    /// Set of methods to extract objects from any kind of stream
    /// </summary>
    public interface IStreamResultGenerator
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
        event EventHandler<StreamExceptionEventArgs> StreamStopped;

        /// <summary>
        /// Get the current state of the stream analysis
        /// </summary>
        StreamState StreamState { get; }

        /// <summary>
        /// Start extracting objects from the stream
        /// </summary>
        Task StartStreamAsync(Action<string> processObject, Func<ITwitterQuery> generateTwitterQuery);

        /// <summary>
        /// Start extracting objects from the stream
        /// </summary>
        /// <param name="processTweet">Method to call foreach object</param>
        /// <param name="generateTwitterQuery">Func to generate the appropriate TwitterQuery</param>
        Task StartStreamAsync(Func<string, bool> processTweet, Func<ITwitterQuery> generateTwitterQuery);

        /// <summary>
        /// Run the stream
        /// </summary>
        void ResumeStream();

        /// <summary>
        /// Pause the stream
        /// </summary>
        void PauseStream();

        /// <summary>
        /// Stop the stream
        /// </summary>
        void StopStream();

        /// <summary>
        /// Stop a stream an define which exception made it fail
        /// </summary>
        void StopStream(Exception exception, IDisconnectMessage disconnectMessage = null);
    }
}