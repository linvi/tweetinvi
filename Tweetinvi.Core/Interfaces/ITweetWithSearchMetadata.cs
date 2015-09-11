using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITweetWithSearchMetadata : ITweet
    {
        ITweetFromSearchMetadata SearchMetadata { get; }
    }
}
