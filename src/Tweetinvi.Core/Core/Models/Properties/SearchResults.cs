using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Models.Properties
{
    public class SearchResults : ISearchResults
    {
        public SearchResults(IEnumerable<ITweetWithSearchMetadata> tweets, ISearchMetadata searchMetadata)
        {
            Tweets = tweets?.ToArray();
            SearchMetadata = searchMetadata;
        }

        public ITweetWithSearchMetadata[] Tweets { get; }
        public ISearchMetadata SearchMetadata { get; }
    }
}
