using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Events.V2;
using Tweetinvi.Models.V2.Responses;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Streams.V2
{
    public class FilteredStreamV2 : TweetStreamV2<FilteredStreamTweetV2EventArgs>, IFilteredStreamV2
    {
        private readonly ITwitterClient _client;
        private readonly ITweetsV2QueryGenerator _tweetsV2QueryGenerator;

        public FilteredStreamV2(
            ITwitterClient client,
            IStreamResultGenerator streamResultGenerator,
            ITweetsV2QueryGenerator tweetsV2QueryGenerator)
        : base(client, streamResultGenerator)
        {
            _client = client;
            _tweetsV2QueryGenerator = tweetsV2QueryGenerator;
        }

        public Task StartAsync()
        {
            return StartAsync(new StartFilteredStreamV2Parameters());
        }

        public Task StartAsync(IStartFilteredStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/search/stream");
            _tweetsV2QueryGenerator.AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return base.StartAsync(query.ToString(), json =>
            {
                try
                {
                    var response = _client.Json.Deserialize<FilteredStreamTweetResponseDTO>(json);
                    return new FilteredStreamTweetV2EventArgs(response, json);
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }
    }
}