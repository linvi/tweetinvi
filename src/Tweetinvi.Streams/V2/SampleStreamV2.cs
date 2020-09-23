using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events;
using Tweetinvi.Events.V2;
using Tweetinvi.Models;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.StreamsV2Client;
using Tweetinvi.Streaming.V2;
using Tweetinvi.Streams.V2;

namespace Tweetinvi.Streams.V2
{


    public class TweetStreamV2
    {
        private readonly ITwitterClient _client;
        private readonly IStreamResultGenerator _streamResultGenerator;

        public event EventHandler<TweetV2ReceivedEventArgs> TweetReceived;
        public event EventHandler<StreamEventReceivedArgs> EventReceived;

        public TweetStreamV2(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator)
        {
            _client = client;
            _streamResultGenerator = streamResultGenerator;
        }

        public async Task StartAsync(string url)
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

                var response = _client.Json.Deserialize<TweetResponseDTO>(json);
                if (response != null)
                {
                    this.Raise(TweetReceived, new TweetV2ReceivedEventArgs(response, json));
                }
            }

            await _streamResultGenerator.StartAsync(onTweetReceived, createTwitterRequest).ConfigureAwait(false);
        }
        }
    }

    public class SampleStreamV2 : TweetStreamV2, ISampleStreamV2
    {

        public SampleStreamV2(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator) : base(client, streamResultGenerator)
        {
        }

        public Task StartAsync()
        {
            return StartAsync(new StartSampleStreamV2Parameters());
        }

        public Task StartAsync(IStartSampleStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/sample/stream");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return base.StartAsync(query.ToString());
        }
    }