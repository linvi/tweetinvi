using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Streams
{
    public class TweetStream : TwitterStream, ITweetStream
    {
        private readonly ITwitterClient _client;
        private readonly ITwitterClientFactories _factories;

        public event EventHandler<TweetReceivedEventArgs> TweetReceived;
        public override event EventHandler<StreamEventReceivedArgs> EventReceived;

        public TweetStream(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITwitterClientFactories factories,
            ICreateTweetStreamParameters createTweetStreamParameters)
            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, createTweetStreamParameters)
        {
            _client = client;
            _factories = factories;
        }

        public async Task StartStreamAsync(string url)
        {
            ITwitterRequest createTwitterRequest()
            {
                var queryBuilder = new StringBuilder(url);
                AddBaseParametersToQuery(queryBuilder);

                var request = _client.CreateRequest();
                request.Query.Url = queryBuilder.ToString();
                request.Query.HttpMethod = HttpMethod.GET;
                return request;
            }

            void onTweetReceived(string json)
            {
                this.Raise(EventReceived, new StreamEventReceivedArgs(json));

                if (IsEvent(json))
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                var tweet = _factories.CreateTweet(json);
                if (tweet != null)
                {
                    this.Raise(TweetReceived, new TweetReceivedEventArgs(tweet, json));
                }
            }

            await _streamResultGenerator.StartStreamAsync(onTweetReceived, createTwitterRequest).ConfigureAwait(false);
        }
    }
}