using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ISearchResultFactory
    {
        ISearchResult Create(ISearchResultsDTO[] searchResultsDTO);
    }
}
