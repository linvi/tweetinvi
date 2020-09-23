using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Parameters.StreamsV2Client;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public class StreamsV2Client : IStreamsV2Client
    {
        private readonly IFactory<ISampleStreamV2> _sampleStreamFactory;

        public StreamsV2Client(IFactory<ISampleStreamV2> sampleStreamFactory)
        {
            _sampleStreamFactory = sampleStreamFactory;
        }

        public ISampleStreamV2 CreateSampleStream()
        {
            return CreateSampleStream(new StartSampleStreamV2Parameters());
        }

        public ISampleStreamV2 CreateSampleStream(IStartSampleStreamV2Parameters parameters)
        {
            var customRequestParameters = _sampleStreamFactory.GenerateParameterOverrideWrapper("parameters", parameters);
            return _sampleStreamFactory.Create(customRequestParameters);
        }
    }
}