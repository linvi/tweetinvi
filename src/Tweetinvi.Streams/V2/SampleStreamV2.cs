using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events.V2;
using Tweetinvi.Models.Responses;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Streams.V2
{
    public class SampleStreamV2 : TweetStreamV2<TweetV2ReceivedEventArgs>, ISampleStreamV2
    {
        private readonly ITwitterClient _client;
        private readonly ITweetsV2QueryGenerator _tweetsV2QueryGenerator;

        public SampleStreamV2(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator,
            ITweetsV2QueryGenerator tweetsV2QueryGenerator) : base(client, streamResultGenerator)
        {
            _client = client;
            _tweetsV2QueryGenerator = tweetsV2QueryGenerator;
        }

        public Task StartAsync()
        {
            return StartAsync(new StartSampleStreamV2Parameters());
        }

        public Task StartAsync(IStartSampleStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/sample/stream");
            _tweetsV2QueryGenerator.AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return base.StartAsync(query.ToString(), json =>
            {
                try
                {
                    var response = _client.Json.Deserialize<TweetV2Response>(json);
                    return new TweetV2ReceivedEventArgs(response, json);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}