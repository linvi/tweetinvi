using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ITrackedStream : ITwitterStream, ITrackableStream<ITweet>
    {
        event EventHandler<MatchedTweetReceivedEventArgs> MatchingTweetReceived;

        void StartStream(string url);
        Task StartStreamAsync(string url);
    }
}