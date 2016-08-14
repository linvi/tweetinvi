using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.SavedSearch
{
    public class SavedSearchFactory : ISavedSearchFactory
    {
        private readonly ISavedSearchQueryExecutor _savedSearchQueryExecutor;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly IFactory<ISavedSearch> _savedSearchUnityFactory;

        public SavedSearchFactory(
            ISavedSearchQueryExecutor savedSearchQueryExecutor,
            IJsonObjectConverter jsonObjectConverter,
            IFactory<ISavedSearch> savedSearchUnityFactory)
        {
            _savedSearchQueryExecutor = savedSearchQueryExecutor;
            _jsonObjectConverter = jsonObjectConverter;
            _savedSearchUnityFactory = savedSearchUnityFactory;
        }

        public ISavedSearch CreateSavedSearch(string searchQuery)
        {
            return _savedSearchQueryExecutor.CreateSavedSearch(searchQuery);
        }

        public ISavedSearch GetSavedSearch(long searchId)
        {
            return _savedSearchQueryExecutor.GetSavedSearch(searchId);
        }

        public ISavedSearch GenerateSavedSearchFromDTO(ISavedSearchDTO savedSearchDTO)
        {
            if (savedSearchDTO == null)
            {
                return null;
            }

            var savedSearchDTOParameter = _savedSearchUnityFactory.GenerateParameterOverrideWrapper("savedSearchDTO", savedSearchDTO);
            return _savedSearchUnityFactory.Create(savedSearchDTOParameter);
        }

        public IEnumerable<ISavedSearch> GenerateSavedSearchesFromDTOs(IEnumerable<ISavedSearchDTO> savedSearchDTOs)
        {
            if (savedSearchDTOs == null)
            {
                return null;
            }

            return savedSearchDTOs.Select(GenerateSavedSearchFromDTO);
        }

        // Generate SavedSearch from Json
        public ISavedSearch GenerateSavedSearchFromJson(string json)
        {
            var savedSearchDTO = _jsonObjectConverter.DeserializeObject<ISavedSearchDTO>(json);
            if (savedSearchDTO == null || savedSearchDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateSavedSearchFromDTO(savedSearchDTO);
        }
    }
}