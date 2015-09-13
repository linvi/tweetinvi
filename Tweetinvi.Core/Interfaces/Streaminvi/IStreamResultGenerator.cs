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
        event EventHandler StreamStarted;
        event EventHandler StreamResumed;
        event EventHandler StreamPaused;
        event EventHandler<StreamExceptionEventArgs> StreamStopped;

        /// <summary>
        /// Get the current state of the stream analysis
        /// </summary>
        StreamState StreamState { get; }

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