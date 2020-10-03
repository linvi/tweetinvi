using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Core.Streaming.V2;
using Tweetinvi.Events;
using Tweetinvi.Models;

namespace Tweetinvi.Streams.V2
{
    public class TweetStreamV2<T> : ITweetStreamV2<T>
    {
        private readonly ITwitterClient _client;
        private readonly IStreamResultGenerator _streamResultGenerator;

        public event EventHandler<StreamEventReceivedArgs> EventReceived;
        public event EventHandler<T> TweetReceived;

        public TweetStreamV2(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator)
        {
            _client = client;
            _streamResultGenerator = streamResultGenerator;
        }

        protected async Task StartAsync(string url, Func<string, T> createResponse)
        {
            ITwitterRequest createTwitterRequest()
            {
                var request = _client.CreateRequest();
                request.Query.Url = url;
                request.Query.HttpMethod = HttpMethod.GET;
                return request;
            }

            void onTweetReceived(string json)
            {
                this.Raise(EventReceived, new StreamEventReceivedArgs(json));

                var response = createResponse(json);
                if (response != null)
                {
                    this.Raise(TweetReceived, response);
                }
            }

            await _streamResultGenerator.StartAsync(onTweetReceived, createTwitterRequest).ConfigureAwait(false);
        }

        public void StopStream()
        {
            _streamResultGenerator.StopStream();
        }
    }
}