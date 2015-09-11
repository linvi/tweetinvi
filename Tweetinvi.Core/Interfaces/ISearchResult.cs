using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces
{
    public interface ISearchResult
    {
        int NumberOfQueriesUsedToCompleteTheSearch { get; }

        IEnumerable<ITweetWithSearchMetadata> Tweets { get; }

        IEnumerable<ISearchQueryResult> SearchQueryResults { get; }
    }
}
