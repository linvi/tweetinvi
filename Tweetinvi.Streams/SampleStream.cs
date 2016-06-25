using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Exceptions;
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
        private readonly ISynchronousInvoker _synchronousInvoker;
        private readonly ISingleAggregateExceptionThrower _singleAggregateExceptionThrower;

        public SampleStream(
            IStreamResultGenerator streamResultGenerator,
            IJsonObjectConverter jsonObjectConverter,
            IJObjectStaticWrapper jObjectStaticWrapper,
            ITweetFactory tweetFactory,
            ISynchronousInvoker synchronousInvoker,
            ICustomRequestParameters customRequestParameters,
            ISingleAggregateExceptionThrower singleAggregateExceptionThrower,
            ITwitterQueryFactory twitterQueryFactory)
            : base(
            streamResultGenerator,
            jsonObjectConverter,
            jObjectStaticWrapper,
            tweetFactory,
            customRequestParameters,
            twitterQueryFactory)
        {
            _synchronousInvoker = synchronousInvoker;
            _singleAggregateExceptionThrower = singleAggregateExceptionThrower;
        }

        public void StartStream()
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(() => StartStream(Resources.Stream_Sample));
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamAsync()
        {
            await StartStream(Resources.Stream_Sample);
        }
    }
}