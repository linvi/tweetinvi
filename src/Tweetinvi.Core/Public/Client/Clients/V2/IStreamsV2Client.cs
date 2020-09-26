using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public interface IStreamsV2Client
    {
        ISampleStreamV2 CreateSampleStream();
    }
}