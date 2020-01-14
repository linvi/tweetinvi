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
            return CreateSampleStream(new CustomRequestParameters());
        }

        public ISampleStream CreateSampleStream(ICustomRequestParameters parameters)
        {
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("customRequestParameters", parameters ?? new CursorQueryParameters());
            var stream = _sampleStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public IFilteredStream CreateFilteredStream()
        {
            return CreateFilteredStream(new CustomRequestParameters());
        }

        public IFilteredStream CreateFilteredStream(ICustomRequestParameters parameters)
        {
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("customRequestParameters", parameters ?? new CursorQueryParameters());
            var stream = _filteredStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public ITweetStream CreateTweetStream()
        {
            return CreateTweetStream(new CustomRequestParameters());
        }

        public ITweetStream CreateTweetStream(ICustomRequestParameters parameters)
        {
            var customRequestParameters = _tweetStreamFactory.GenerateParameterOverrideWrapper("customRequestParameters", parameters ?? new CursorQueryParameters());
            var stream = _tweetStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }

        public ITrackedStream CreateTrackedStream()
        {
            return CreateTrackedStream(new CustomRequestParameters());
        }

        public ITrackedStream CreateTrackedStream(ICustomRequestParameters parameters)
        {
            var customRequestParameters = _trackedStreamFactory.GenerateParameterOverrideWrapper("customRequestParameters", parameters ?? new CursorQueryParameters());
            var stream = _trackedStreamFactory.Create(customRequestParameters);
            stream.TweetMode = _client.ClientSettings.TweetMode;
            return stream;
        }
    }
}