using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryValidator
    {
        bool IsSearchParameterValid(ITweetSearchParameters searchParameters);
        bool IsSearchTweetsQueryValid(string searchQuery);
        bool IsGeoCodeValid(IGeoCode geoCode);
        bool IsLocaleParameterValid(string locale);
        bool IsLangDefined(Language lang);
        bool IsDateTimeDefined(DateTime untilDateTime);

        bool IsSearchUserQueryValid(string searchQuery);
    }

    public class SearchQueryValidator : ISearchQueryValidator
    {
        public bool IsSearchParameterValid(ITweetSearchParameters searchParameters)
        {
            return searchParameters != null && IsAtLeasOneRequiredCriteriaSet(searchParameters);
        }

        private bool IsAtLeasOneRequiredCriteriaSet(ITweetSearchParameters searchParameters)
        {
            bool isSearchQuerySet = !String.IsNullOrEmpty(searchParameters.SearchQuery);
            bool isSearchQueryValid = IsSearchTweetsQueryValid(searchParameters.SearchQuery);
            bool isGeoCodeSet = IsGeoCodeValid(searchParameters.GeoCode);
            bool isEntitiesTypeSet = searchParameters.Filters != TweetSearchFilters.None;

            return (isSearchQuerySet && isSearchQueryValid) || isGeoCodeSet || isEntitiesTypeSet;
        }

        public bool IsSearchTweetsQueryValid(string searchQuery)
        {
            // We might want to restrict the size to 1000 characters as indicated in the documentation
            return true;
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