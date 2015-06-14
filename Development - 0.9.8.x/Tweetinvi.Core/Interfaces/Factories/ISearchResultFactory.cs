using Tweetinvi.Core.Interfaces.DTO;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ISearchResultFactory
    {
        ISearchResult Create(ISearchResultsDTO[] searchResultsDTO);
    }
}
