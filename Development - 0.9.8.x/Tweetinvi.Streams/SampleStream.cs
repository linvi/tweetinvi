using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Exceptions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Parameters;
using Tweetinvi.Core.Interfaces.Streaminvi;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Core.Wrappers;
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
            ITwitterRequestGenerator twitterRequestGenerator,
            ISynchronousInvoker synchronousInvoker,
            ICustomRequestParameters customRequestParameters,
            ISingleAggregateExceptionThrower singleAggregateExceptionThrower,
            ITwitterQueryFactory twitterQueryFactory)
            : base(streamResultGenerator, jsonObjectConverter, jObjectStaticWrapper, tweetFactory, twitterRequestGenerator, customRequestParameters, twitterQueryFactory)
        {
            _synchronousInvoker = synchronousInvoker;
            _singleAggregateExceptionThrower = singleAggregateExceptionThrower;
        }

        public void StartStream()
        {
            Action startStreamAction = () => _synchronousInvoker.ExecuteSynchronously(StartStream(Resources.Stream_Sample));
            _singleAggregateExceptionThrower.ExecuteActionAndThrowJustOneExceptionIfExist(startStreamAction);
        }

        public async Task StartStreamAsync()
        {
            await StartStream(Resources.Stream_Sample);
        }
    }
}