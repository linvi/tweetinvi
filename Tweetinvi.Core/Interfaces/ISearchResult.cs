using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces
{
    /// <summary>
    /// Object containing the data returned by the Twitter search api.
    /// </summary>
    public interface ISearchResult
    {
        /// <summary>
        /// Number of searches performed to retrieve the data.
        /// </summary>
        int NumberOfQueriesUsedToCompleteTheSearch { get; }

        /// <summary>
        /// Tweets filtered after being received from the search.
        /// </summary>
        IEnumerable<ITweetWithSearchMetadata> Tweets { get; }

        /// <summary>
        /// All search result information returned from Twitter.
        /// </summary>
        IEnumerable<ISearchQueryResult> SearchQueryResults { get; }
    }
}