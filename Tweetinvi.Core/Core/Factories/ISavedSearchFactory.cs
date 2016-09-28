using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ISavedSearchFactory
    {
        ISavedSearch CreateSavedSearch(string searchQuery);
        ISavedSearch GetSavedSearch(long searchId);
        ISavedSearch GenerateSavedSearchFromDTO(ISavedSearchDTO savedSearchDTO);
        IEnumerable<ISavedSearch> GenerateSavedSearchesFromDTOs(IEnumerable<ISavedSearchDTO> savedSearchDTO);
        ISavedSearch GenerateSavedSearchFromJson(string json);
    }
}