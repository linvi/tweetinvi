using Tweetinvi.Models.DTO;

namespace Tweetinvi.Models
{
    public interface ITweetWithSearchMetadata : ITweet
    {
        /// <summary>
        /// Property containing search metadata.
        /// </summary>
        ITweetFromSearchMetadata SearchMetadata { get; }
    }
}