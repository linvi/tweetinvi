using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ISavedSearchFactory
    {
        ISavedSearch CreateSavedSearch(string searchQuery);
        ISavedSearch GetSavedSearch(long searchId);
        ISavedSearch GenerateSavedSearchFromDTO(ISavedSearchDTO savedSearchDTO);
        IEnumerable<ISavedSearch> GenerateSavedSearchesFromDTOs(IEnumerable<ISavedSearchDTO> savedSearchDTO);
    }
}