using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Factories.SavedSearch
{
   public class SavedSearchFactory : ISavedSearchFactory
    {
        private readonly ISavedSearchQueryExecutor _savedSearchQueryExecutor;
        private readonly IFactory<ISavedSearch> _savedSearchUnityFactory;

        public SavedSearchFactory(ISavedSearchQueryExecutor savedSearchQueryExecutor, IFactory<ISavedSearch> savedSearchUnityFactory)
        {
            _savedSearchQueryExecutor = savedSearchQueryExecutor;
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
    }
}