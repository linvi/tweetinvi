using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events;
using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Streaming;
using Tweetinvi.Streaming.Events;
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
        public event EventHandler<StreamStoppedEventArgs> StreamStopped;
        public event EventHandler KeepAliveReceived;

        private IStreamTask _currentStreamTask;
        private readonly IStreamTaskFactory _streamTaskFactory;
        private readonly object _lockStream = new object();

        public StreamResultGenerator(IStreamTaskFactory streamTaskFactory)
        {
            _streamTaskFactory = streamTaskFactory;
        }

        private bool IsRunning => StreamState == StreamState.Running || StreamState == StreamState.Pause;

        public StreamState StreamState
        {
            get
            {
                lock (_lockStream)
                {
                    if (_currentStreamTask != null)
                    {
                        return _currentStreamTask.StreamState;
                    }
                }

                return StreamState.Stop;
            }
        }

        public async Task StartStream(Action<string> onJsonReceivedCallback, Func<ITwitterRequest> createTwitterRequest)
        {
            bool onJsonReceivedValidateCallback(string json)
            {
                onJsonReceivedCallback(json);
                return true;
            }

            await StartStream(onJsonReceivedValidateCallback, createTwitterRequest).ConfigureAwait(false);
        }

        public async Task StartStream(Func<string, bool> onJsonReceivedCallback, Func<ITwitterRequest> createTwitterRequest)
        {
            IStreamTask streamTask;

            lock (_lockStream)
            {
                if (IsRunning)
                {
                    throw new OperationCanceledException(Resources.Stream_IllegalMultipleStreams);
                }

                if (onJsonReceivedCallback == null)
                {
                    throw new NullReferenceException(Resources.Stream_ObjectDelegateIsNull);
                }

                streamTask = _streamTaskFactory.Create(onJsonReceivedCallback, createTwitterRequest);

                _currentStreamTask = streamTask;
                _currentStreamTask.StreamStarted += StreamTaskStarted;
                _currentStreamTask.StreamStateChanged += StreamTaskStateChanged;
                _currentStreamTask.KeepAliveReceived += KeepAliveReceived;
            }

            await streamTask.Start().ConfigureAwait(false);
        }

        private void StreamTaskStarted(object sender, EventArgs eventArgs)
        {
            this.Raise(StreamStarted);
        }

        private void StreamTaskStateChanged(object sender, StreamTaskStateChangedEventArgs args)
        {
            var streamState = args.State;
            switch (streamState)
            {
                case StreamState.Running:
                    this.Raise(StreamResumed);
                    break;
                case StreamState.Pause:
                    this.Raise(StreamPaused);
                    break;
                case StreamState.Stop:
                    var streamStoppedEventArgs = new StreamStoppedEventArgs(args.Exception);

                    this.Raise(StreamStopped, streamStoppedEventArgs);
                    break;
            }
        }

        public void ResumeStream()
        {
            lock (_lockStream)
            {
                _currentStreamTask?.Resume();
            }
        }

        public void PauseStream()
        {
            lock (_lockStream)
            {
                _currentStreamTask?.Pause();
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

        public void StopStream(Exception exception, IDisconnectMessage disconnectMessage)
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

                    var streamExceptionEventArgs = new StreamStoppedEventArgs(exception, disconnectMessage);
                    this.Raise(StreamStopped, streamExceptionEventArgs);
                }
            }
        }

        private StreamStoppedEventArgs StopStreamAndUnsubscribeFromEvents()
        {
            var streamTask = _currentStreamTask;
            if (streamTask != null)
            {
                streamTask.StreamStarted -= StreamTaskStarted;
                streamTask.StreamStateChanged -= StreamTaskStateChanged;
                streamTask.KeepAliveReceived -= KeepAliveReceived;
                streamTask.Stop();

                if (_currentStreamTask == streamTask)
                {
                    _currentStreamTask = null;
                }

                return new StreamStoppedEventArgs();
            }

            return new StreamStoppedEventArgs(null);
        }
    }
}