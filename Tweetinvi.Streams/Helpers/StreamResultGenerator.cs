using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.StreamMessages;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Streams.Model;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams.Helpers
{
    /// <summary>
    /// Extract objects from any kind of stream
    /// </summary>
    public class StreamResultGenerator : IStreamResultGenerator
    {
        public event EventHandler StreamStarted;
        public event EventHandler StreamResumed;
        public event EventHandler StreamPaused;
        public event EventHandler<StreamExceptionEventArgs> StreamStopped;

        private IStreamTask _currentStreamTask;
        private readonly IFactory<IStreamTask> _streamTaskFactory;
        private readonly object _lockStream = new object();

        public StreamResultGenerator(IFactory<IStreamTask> streamTaskFactory)
        {
            _streamTaskFactory = streamTaskFactory;
        }

        private bool IsRunning
        {
            get { return StreamState == StreamState.Running || StreamState == StreamState.Pause; }
        }

        public StreamState StreamState
        {
            get
            {
                if (_currentStreamTask != null)
                {
                    return _currentStreamTask.StreamState;
                }

                return StreamState.Stop;
            }
        }

        public async Task StartStreamAsync(Action<string> processObject, Func<ITwitterQuery> generateTwitterQuery)
        {
            Func<string, bool> processValidObject = json =>
            {
                processObject(json);
                return true;
            };

            await StartStreamAsync(processValidObject, generateTwitterQuery);
        }

        public async Task StartStreamAsync(Func<string, bool> processObject, Func<ITwitterQuery> generateTwitterQuery)
        {
            IStreamTask streamTask;

            lock (_lockStream)
            {
                if (IsRunning)
                {
                    throw new OperationCanceledException(Resources.Stream_IllegalMultipleStreams);
                }

                if (processObject == null)
                {
                    throw new NullReferenceException(Resources.Stream_ObjectDelegateIsNull);
                }

                var processObjectParameter = _streamTaskFactory.GenerateParameterOverrideWrapper("processObject", processObject);
                var generateWebRequestParameter = _streamTaskFactory.GenerateParameterOverrideWrapper("generateTwitterQuery", generateTwitterQuery);
                
                streamTask = _streamTaskFactory.Create(processObjectParameter, generateWebRequestParameter);

                _currentStreamTask = streamTask;
                _currentStreamTask.StreamStarted += StreamTaskStarted;
                _currentStreamTask.StreamStateChanged += StreamTaskStateChanged;
            }

            await TaskEx.Run(() =>
            {
                streamTask.Start();
            }).ConfigureAwait(false);
        }

        private void StreamTaskStarted(object sender, EventArgs eventArgs)
        {
            this.Raise(StreamStarted);
        }

        private void StreamTaskStateChanged(object sender, GenericEventArgs<StreamState> args)
        {
            var streamState = args.Value;
            switch (streamState)
            {
                case StreamState.Running:
                    this.Raise(StreamResumed);
                    break;
                case StreamState.Pause:
                    this.Raise(StreamPaused);
                    break;
                case StreamState.Stop:
                    StreamExceptionEventArgs streamExceptionEventArgs;

                    var streamTask = _currentStreamTask;

                    if (streamTask != null)
                    {
                        streamExceptionEventArgs = new StreamExceptionEventArgs(streamTask.LastException);
                    }
                    else
                    {
                        streamExceptionEventArgs = new StreamExceptionEventArgs(null);
                    }

                    this.Raise(StreamStopped, streamExceptionEventArgs);
                    break;
            }
        }

        public void ResumeStream()
        {
            if (_currentStreamTask != null)
            {
                _currentStreamTask.Resume();
            }
        }

        public void PauseStream()
        {
            if (_currentStreamTask != null)
            {
                _currentStreamTask.Pause();
            }
        }

        public void StopStream()
        {
            lock (_lockStream)
            {
                var stopEventArgs = StopStreamAndUnsubscribeFromEvents();
                this.Raise(StreamStopped, stopEventArgs);
            }
        }

        public void StopStream(Exception exception, IDisconnectMessage disconnectMessage = null)
        {
            lock (_lockStream)
            {
                if (StreamState != StreamState.Stop)
                {
                    StopStreamAndUnsubscribeFromEvents();

                    if (exception is ITwitterTimeoutException && disconnectMessage == null)
                    {
                        disconnectMessage = new DisconnectMessage
                        {
                            Code = 503,
                            Reason = "Timeout"
                        };
                    }

                    var streamExceptionEventArgs = new StreamExceptionEventArgs(exception, disconnectMessage);
                    this.Raise(StreamStopped, streamExceptionEventArgs);
                }
            }
        }

        private StreamExceptionEventArgs StopStreamAndUnsubscribeFromEvents()
        {
            var streamTask = _currentStreamTask;
            if (streamTask != null)
            {
                streamTask.StreamStarted -= StreamTaskStarted;
                streamTask.StreamStateChanged -= StreamTaskStateChanged;
                streamTask.Stop();

                if (_currentStreamTask == streamTask)
                {
                    _currentStreamTask = null;
                }

                return new StreamExceptionEventArgs(streamTask.LastException);
            }

            return new StreamExceptionEventArgs(null);
        }
    }
}