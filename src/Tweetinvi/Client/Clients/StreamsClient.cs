using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;

namespace Tweetinvi.Client
{
    public class StreamsClient : IStreamsClient
    {
        private readonly ITwitterClient _client;
        private readonly IFactory<ISampleStream> _sampleStreamFactory;
        private readonly IFactory<IFilteredStream> _filteredStreamFactory;
        private readonly IFactory<ITrackedStream> _trackedStreamFactory;
        private readonly IFactory<ITweetStream> _tweetStreamFactory;

        public StreamsClient(
            ITwitterClient client,
            IFactory<ISampleStream> sampleStreamFactory,
            IFactory<IFilteredStream> filteredStreamFactory,
            IFactory<ITrackedStream> trackedStreamFactory,
            IFactory<ITweetStream> tweetStreamFactory)
        {
            _client = client;
            _sampleStreamFactory = sampleStreamFactory;
            _filteredStreamFactory = filteredStreamFactory;
            _trackedStreamFactory = trackedStreamFactory;
            _tweetStreamFactory = tweetStreamFactory;
        }

        public ISampleStream CreateSampleStream()
        {
            return CreateSampleStream(new CreateSampleStreamParameters());
        }

        public ISampleStream CreateSampleStream(ICreateSampleStreamParameters parameters)
        {
            parameters ??= new CreateSampleStreamParameters();
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("createSampleStreamParameters", parameters);
            var stream = _sampleStreamFactory.Create(customRequestParameters);
            stream.TweetMode = parameters.TweetMode ?? _client.Config.TweetMode;
            return stream;
        }

        public IFilteredStream CreateFilteredStream()
        {
            return CreateFilteredStream(new CreateFilteredTweetStreamParameters());
        }

        public IFilteredStream CreateFilteredStream(ICreateFilteredTweetStreamParameters parameters)
        {
            parameters ??= new CreateFilteredTweetStreamParameters();
            var customRequestParameters = _filteredStreamFactory.GenerateParameterOverrideWrapper("createFilteredTweetStreamParameters", parameters);
            var stream = _filteredStreamFactory.Create(customRequestParameters);
            stream.TweetMode = parameters.TweetMode ?? _client.Config.TweetMode;
            return stream;
        }

        public ITweetStream CreateTweetStream()
        {
            return CreateTweetStream(new CreateTweetStreamParameters());
        }

        public ITweetStream CreateTweetStream(ICreateTweetStreamParameters parameters)
        {
            parameters ??= new CreateTweetStreamParameters();
            var customRequestParameters = _tweetStreamFactory.GenerateParameterOverrideWrapper("createTweetStreamParameters", parameters);
            var stream = _tweetStreamFactory.Create(customRequestParameters);
            stream.TweetMode = parameters.TweetMode ?? _client.Config.TweetMode;
            return stream;
        }

        public ITrackedStream CreateTrackedTweetStream()
        {
            return CreateTrackedTweetStream(new CreateTrackedTweetStreamParameters());
        }

        public ITrackedStream CreateTrackedTweetStream(ICreateTrackedTweetStreamParameters parameters)
        {
            parameters ??= new CreateTrackedTweetStreamParameters();
            var customRequestParameters = _trackedStreamFactory.GenerateParameterOverrideWrapper("createTrackedTweetStreamParameters", parameters);
            var stream = _trackedStreamFactory.Create(customRequestParameters);
            stream.TweetMode = parameters.TweetMode ?? _client.Config.TweetMode;
            return stream;
        }
    }
}