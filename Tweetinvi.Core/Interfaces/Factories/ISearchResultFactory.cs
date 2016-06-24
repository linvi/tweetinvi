using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ISearchResultFactory
    {
        ISearchResult Create(ISearchResultsDTO[] searchResultsDTO);
    }
}
