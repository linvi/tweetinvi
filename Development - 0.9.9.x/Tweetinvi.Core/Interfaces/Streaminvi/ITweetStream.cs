using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Interfaces.Streaminvi
{
    public interface ITweetStream : ITwitterStream
    {
        event EventHandler<TweetReceivedEventArgs> TweetReceived;
        Task StartStream(string url);
    }
}