using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Factories;
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
        private readonly ITweetFactory _tweetFactory;
        private readonly ITwitterQueryFactory _twitterQueryFactory;

        public event EventHandler<TweetReceivedEventArgs> TweetReceived;
        public override event EventHandler<JsonObjectEventArgs> JsonObjectReceived;

        public TweetStream(
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITweetFactory tweetFactory,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory)
            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, customRequestParameters)
        {
            _tweetFactory = tweetFactory;
            _twitterQueryFactory = twitterQueryFactory;
        }

        public async Task StartStream(string url)
        {
            Func<ITwitterQuery> generateTwitterQuery = delegate
            {
                var queryBuilder = new StringBuilder(url);
                AddBaseParametersToQuery(queryBuilder);

                return _twitterQueryFactory.Create(queryBuilder.ToString(), HttpMethod.GET, Credentials);
            };

            Action<string> generateTweetDelegate = json =>
            {
                this.Raise(JsonObjectReceived, new JsonObjectEventArgs(json));

                var tweet = _tweetFactory.GenerateTweetFromJson(json);
                if (tweet == null)
                {
                    TryInvokeGlobalStreamMessages(json);
                    return;
                }

                this.Raise(TweetReceived, new TweetReceivedEventArgs(tweet, json));
            };

            await _streamResultGenerator.StartStreamAsync(generateTweetDelegate, generateTwitterQuery).ConfigureAwait(false);
        }
    }
}