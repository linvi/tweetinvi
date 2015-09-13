using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ISampleStream : ITwitterStream
    {
        event EventHandler<TweetReceivedEventArgs> TweetReceived;
        void StartStream();
        Task StartStreamAsync();
    }
}