using System.Collections.Generic;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Models
{
    public interface ISearchQueryResult
    {
        /// <summary>
        /// All the tweets returned by the Twitter Request
        /// </summary>
        IEnumerable<ITweetWithSearchMetadata> AllTweetsFromQuery { get; }

        /// <summary>
        /// All the tweets returned by the Twitter Request and Filtered by the TweetSearchFilter
        /// </summary>
        IEnumerable<ITweetWithSearchMetadata> FilteredTweets { get; }

        /// <summary>
        /// Search Metadata Information
        /// </summary>
        ISearchMetadata SearchMetadata { get; }
    }
}