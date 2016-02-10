using System;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Events.EventArguments;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;

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

                this.Raise(TweetReceived, new TweetReceivedEventArgs(tweet));
            };

            await _streamResultGenerator.StartStreamAsync(generateTweetDelegate, generateTwitterQuery).ConfigureAwait(false);
        }
    }
}