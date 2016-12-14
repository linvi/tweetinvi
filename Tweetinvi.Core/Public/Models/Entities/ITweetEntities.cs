using System.Collections.Generic;

namespace Tweetinvi.Models.Entities
{
    /// <summary>
    /// Entities are special elements that can be given to an ITweet
    /// </summary>
    public interface ITweetEntities : IObjectEntities
    {
        /// <summary>
        /// Collection of medias associated with a Tweet
        /// </summary>
        List<IMediaEntity> Medias { get; }
    }
}