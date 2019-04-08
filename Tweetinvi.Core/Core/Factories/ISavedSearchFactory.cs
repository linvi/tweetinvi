using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ISavedSearchFactory
    {
        Task<ISavedSearch> CreateSavedSearch(string searchQuery);
        Task<ISavedSearch> GetSavedSearch(long searchId);
        ISavedSearch GenerateSavedSearchFromDTO(ISavedSearchDTO savedSearchDTO);
        IEnumerable<ISavedSearch> GenerateSavedSearchesFromDTOs(IEnumerable<ISavedSearchDTO> savedSearchDTO);
        ISavedSearch GenerateSavedSearchFromJson(string json);
    }
}