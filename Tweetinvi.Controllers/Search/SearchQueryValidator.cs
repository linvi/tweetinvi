using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryValidator
    {
        void ThrowIfSearchParametersIsNotValid(ITweetSearchParameters searchParameters);
        bool IsSearchQueryValid(string searchQuery);
        bool IsGeoCodeValid(IGeoCode geoCode);
        bool IsLocaleParameterValid(string locale);
        bool IsLangDefined(Language lang);
        bool IsDateTimeDefined(DateTime untilDateTime);

        bool IsSearchUserQueryValid(string searchQuery);
    }

    public class SearchQueryValidator : ISearchQueryValidator
    {
        public void ThrowIfSearchParametersIsNotValid(ITweetSearchParameters searchParameters)
        {
            if (searchParameters == null)
            {
                throw new ArgumentNullException("Search parameters cannot be null");
            }

            if (!IsAtLeasOneRequiredCriteriaSet(searchParameters))
            {
                throw new ArgumentException("At least one of the required parameters needs to be valid (query, geocode or filter).");
            }
        }

        private bool IsAtLeasOneRequiredCriteriaSet(ITweetSearchParameters searchParameters)
        {
            bool isSearchQuerySet = !string.IsNullOrEmpty(searchParameters.SearchQuery);
            bool isSearchQueryValid = IsSearchQueryValid(searchParameters.SearchQuery);
            bool isGeoCodeSet = IsGeoCodeValid(searchParameters.GeoCode);
            bool isEntitiesTypeSet = searchParameters.Filters != TweetSearchFilters.None;

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
            return !String.IsNullOrEmpty(locale);
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