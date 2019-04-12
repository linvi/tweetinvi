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

        public int NumberOfQueriesUsedToCompleteTheSearch { get; private set; }
        public IEnumerable<ITweetWithSearchMetadata> Tweets { get; private set; }
        public IEnumerable<ISearchQueryResult> SearchQueryResults { get; private set; }
    }
}