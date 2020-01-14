using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Tweetinvi.Streams.Properties;

namespace Tweetinvi.Streams
{
    public class SampleStream : TweetStream, ISampleStream
    {
        public SampleStream(
            ITwitterClient twitterClient,
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITwitterClientFactories factories,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory)
            : base(
                twitterClient,
                streamResultGenerator,
                jsonObjectConverter,
                jObjectStaticWrapper,
                factories,
                customRequestParameters)
        {
        }

        public async Task StartStream()
        {
            await StartStream(Resources.Stream_Sample).ConfigureAwait(false);
        }
    }
}