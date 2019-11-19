using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Logic.Model
{
    public class SearchQueryResult : ISearchQueryResult
    {
        public SearchQueryResult(
            IEnumerable<ITweetWithSearchMetadata> allTweetsFromQuery,
            IEnumerable<ITweetWithSearchMetadata> filteredTweets, 
            ISearchMetadata searchMetadata)
        {
            AllTweetsFromQuery = allTweetsFromQuery;
            FilteredTweets = filteredTweets;
            SearchMetadata = searchMetadata;
        }

        public IEnumerable<ITweetWithSearchMetadata> AllTweetsFromQuery { get; }
        public IEnumerable<ITweetWithSearchMetadata> FilteredTweets { get; }
        public ISearchMetadata SearchMetadata { get; }
    }
}
