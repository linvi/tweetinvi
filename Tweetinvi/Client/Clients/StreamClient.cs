using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;

namespace Tweetinvi.Client
{
    public class StreamClient : IStreamClient
    {
        private readonly ITwitterClient _client;
        private readonly IFactory<ISampleStream> _sampleStreamFactory;
        private readonly IFactory<IFilteredStream> _filteredStreamFactory;
        private readonly IFactory<ITrackedStream> _trackedStreamFactory;
        private readonly IFactory<ITweetStream> _tweetStreamFactory;

        public StreamClient(
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
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("createSampleStreamParameters", parameters ?? new CreateSampleStreamParameters());
            var stream = _sampleStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public IFilteredStream CreateFilteredStream()
        {
            return CreateFilteredStream(new CreateFilteredStreamParameters());
        }

        public IFilteredStream CreateFilteredStream(ICreateFilteredStreamParameters parameters)
        {
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("createFilteredStreamParameters", parameters ?? new CreateFilteredStreamParameters());
            var stream = _filteredStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public ITweetStream CreateTweetStream()
        {
            return CreateTweetStream(new CreateTweetStreamParameters());
        }

        public ITweetStream CreateTweetStream(ICreateTweetStreamParameters parameters)
        {
            var customRequestParameters = _tweetStreamFactory.GenerateParameterOverrideWrapper("createTweetStreamParameters", parameters ?? new CreateTweetStreamParameters());
            var stream = _tweetStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public ITrackedStream CreateTrackedStream()
        {
            return CreateTrackedStream(new CreateTrackedStreamParameters());
        }

        public ITrackedStream CreateTrackedStream(ICreateTrackedStreamParameters parameters)
        {
            var customRequestParameters = _trackedStreamFactory.GenerateParameterOverrideWrapper("createTrackedStreamParameters", parameters ?? new CreateTrackedStreamParameters());
            var stream = _trackedStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }
    }
}