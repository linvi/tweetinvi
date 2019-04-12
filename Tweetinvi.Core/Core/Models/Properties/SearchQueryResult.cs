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

        public IEnumerable<ITweetWithSearchMetadata> AllTweetsFromQuery { get; private set; }
        public IEnumerable<ITweetWithSearchMetadata> FilteredTweets { get; private set; }
        public ISearchMetadata SearchMetadata { get; private set; }
    }
}
