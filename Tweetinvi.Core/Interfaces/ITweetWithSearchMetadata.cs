using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Interfaces
{
    public interface ITweetWithSearchMetadata : ITweet
    {
        /// <summary>
        /// Property containing search metadata.
        /// </summary>
        ITweetFromSearchMetadata SearchMetadata { get; }
    }
}