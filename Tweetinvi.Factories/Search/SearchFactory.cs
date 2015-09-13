using System.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Logic.Model;

namespace Tweetinvi.Factories.Search
{
    public class SearchResultFactory : ISearchResultFactory
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly IFactory<ISearchQueryResult> _searchResultFactory;

        public SearchResultFactory(
            ITweetFactory tweetFactory,
            IFactory<ISearchQueryResult> searchResultFactory)
        {
            _tweetFactory = tweetFactory;
            _searchResultFactory = searchResultFactory;
        }

        public ISearchResult Create(ISearchResultsDTO[] searchResultsDTO)
        {
            var searchResults = searchResultsDTO.Select(CreateSearchResult).ToArray();
            return new SearchResult(searchResults);
        }

        private ISearchQueryResult CreateSearchResult(ISearchResultsDTO searchResultsDTO)
        {
            var tweets = _tweetFactory.GenerateTweetsWithSearchMetadataFromDTOs(searchResultsDTO.TweetDTOs);
            var matchingTweets = _tweetFactory.GenerateTweetsWithSearchMetadataFromDTOs(searchResultsDTO.MatchingTweetDTOs);
            var tweetParameter = _searchResultFactory.GenerateParameterOverrideWrapper("allTweetsFromQuery", tweets);
            var matchingTweetParameter = _searchResultFactory.GenerateParameterOverrideWrapper("filteredTweets", matchingTweets);
            var searchMetadataParameter = _searchResultFactory.GenerateParameterOverrideWrapper("searchMetadata", searchResultsDTO.SearchMetadata);

            return _searchResultFactory.Create(tweetParameter, matchingTweetParameter, searchMetadataParameter);
        }
    }
}
