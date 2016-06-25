using System;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryValidator
    {
        void ThrowIfSearchParametersIsNotValid(ISearchTweetsParameters searchTweetsParameters);
        bool IsSearchQueryValid(string searchQuery);
        bool IsGeoCodeValid(IGeoCode geoCode);
        bool IsLocaleParameterValid(string locale);
        bool IsLangDefined(Language lang);
        bool IsDateTimeDefined(DateTime untilDateTime);

        bool IsSearchUserQueryValid(string searchQuery);
    }

    public class SearchQueryValidator : ISearchQueryValidator
    {
        public void ThrowIfSearchParametersIsNotValid(ISearchTweetsParameters searchTweetsParameters)
        {
            if (searchTweetsParameters == null)
            {
                throw new ArgumentNullException("Search parameters cannot be null");
            }

            if (!IsAtLeasOneRequiredCriteriaSet(searchTweetsParameters))
            {
                throw new ArgumentException("At least one of the required parameters needs to be valid (query, geocode or filter).");
            }
        }

        private bool IsAtLeasOneRequiredCriteriaSet(ISearchTweetsParameters searchTweetsParameters)
        {
            bool isSearchQuerySet = !string.IsNullOrEmpty(searchTweetsParameters.SearchQuery);
            bool isSearchQueryValid = IsSearchQueryValid(searchTweetsParameters.SearchQuery);
            bool isGeoCodeSet = IsGeoCodeValid(searchTweetsParameters.GeoCode);
            bool isEntitiesTypeSet = searchTweetsParameters.Filters != TweetSearchFilters.None;

            return (isSearchQuerySet && isSearchQueryValid) || isGeoCodeSet || isEntitiesTypeSet;
        }

        public bool IsSearchQueryValid(string searchQuery)
        {
            // We might want to restrict the size to 1000 characters as indicated in the documentation
            return !string.IsNullOrWhiteSpace(searchQuery);
        }

        public bool IsGeoCodeValid(IGeoCode geoCode)
        {
            return geoCode != null;
        }

        public bool IsLocaleParameterValid(string locale)
        {
            return !string.IsNullOrEmpty(locale);
        }

        public bool IsLangDefined(Language lang)
        {
            return lang != Language.Undefined;
        }

        public bool IsDateTimeDefined(DateTime dateTime)
        {
            return dateTime != default (DateTime);
        }

        public bool IsSearchUserQueryValid(string searchQuery)
        {
            return !string.IsNullOrEmpty(searchQuery);
        }
    }
}