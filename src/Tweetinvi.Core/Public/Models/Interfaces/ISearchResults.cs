using System.Collections.Generic;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Models
{
    public interface ISearchResults
    {
        /// <summary>
        /// All the tweets returned by the Twitter Request
        /// </summary>
        ITweetWithSearchMetadata[] Tweets { get; }

        /// <summary>
        /// Search Metadata Information
        /// </summary>
        ISearchMetadata SearchMetadata { get; }
    }
}