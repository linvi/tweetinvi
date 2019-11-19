using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Logic.Model
{
    public class SearchResult : ISearchResult
    {
        public SearchResult(ISearchQueryResult[] searchQueryQueryResults)
        {
            SearchQueryResults = searchQueryQueryResults;
            Tweets = searchQueryQueryResults.SelectMany(x => x.FilteredTweets);
            NumberOfQueriesUsedToCompleteTheSearch = searchQueryQueryResults.Length;
        }

        public int NumberOfQueriesUsedToCompleteTheSearch { get; }
        public IEnumerable<ITweetWithSearchMetadata> Tweets { get; }
        public IEnumerable<ISearchQueryResult> SearchQueryResults { get; }
    }
}