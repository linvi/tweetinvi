using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
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
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITweetFactory tweetFactory,
            ICustomRequestParameters customRequestParameters,
            ITwitterQueryFactory twitterQueryFactory)
            : base(
            streamResultGenerator,
            jsonObjectConverter,
            jObjectStaticWrapper,
            tweetFactory,
            customRequestParameters,
            twitterQueryFactory)
        {
        }

        public async Task StartStream()
        {
            await StartStream(Resources.Stream_Sample).ConfigureAwait(false);
        }
    }
}