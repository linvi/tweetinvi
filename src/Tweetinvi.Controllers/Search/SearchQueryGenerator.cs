using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryGenerator
    {
        string GetSearchTweetsQuery(ISearchTweetsParameters parameters);

        string GetSearchUsersQuery(ISearchUsersParameters parameters);
        string GetCreateSavedSearchQuery(ICreateSavedSearchParameters parameters);
        string GetSavedSearchQuery(IGetSavedSearchParameters parameters);
        string GetListSavedSearchQuery(IListSavedSearchesParameters parameters);
        string GetDestroySavedSearchQuery(IDestroySavedSearchParameters parameters);
    }

    public class SearchQueryGenerator : ISearchQueryGenerator
    {
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ISearchQueryParameterGenerator _searchQueryParameterGenerator;

        public SearchQueryGenerator(
            IQueryParameterGenerator queryParameterGenerator,
            ISearchQueryParameterGenerator searchQueryParameterGenerator)
        {
            _queryParameterGenerator = queryParameterGenerator;
            _searchQueryParameterGenerator = searchQueryParameterGenerator;
        }

        public string GetSearchTweetsQuery(ISearchTweetsParameters parameters)
        {
            var query = new StringBuilder(Resources.Search_SearchTweets);

            query.AddParameterToQuery("q", GenerateQueryParameter(parameters.Query, parameters.Filters));
            query.AddParameterToQuery("geocode", _searchQueryParameterGenerator.GenerateGeoCodeParameter(parameters.GeoCode));

            query.AddParameterToQuery("lang", parameters.Lang?.GetLanguageCode());
            query.AddParameterToQuery("locale", parameters.Locale);
            query.AddParameterToQuery("result_type", parameters.SearchType?.ToString().ToLowerInvariant());

            _queryParameterGenerator.AddMinMaxQueryParameters(query, parameters);

            query.AddFormattedParameterToQuery(_searchQueryParameterGenerator.GenerateSinceParameter(parameters.Since));
            query.AddFormattedParameterToQuery(_searchQueryParameterGenerator.GenerateUntilParameter(parameters.Until));
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);
            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(parameters.TweetMode));
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        private string GenerateQueryParameter(string query, TweetSearchFilters tweetSearchFilters)
        {
            if (tweetSearchFilters == TweetSearchFilters.None)
            {
                return query;
            }

            foreach (var entitiesTypeFilter in GetFlags(tweetSearchFilters))
            {
                if (entitiesTypeFilter != TweetSearchFilters.None)
                {
                    var filter = entitiesTypeFilter.GetQueryFilterName().ToLowerInvariant();
                    query += string.Format(" filter:{0}", filter);
                }
            }

            return query;
        }

        private IEnumerable<TweetSearchFilters> GetFlags(TweetSearchFilters tweetSearchFilters)
        {
            foreach (TweetSearchFilters value in Enum.GetValues(tweetSearchFilters.GetType()))
            {
                if (tweetSearchFilters.HasFlag(value) && (tweetSearchFilters & value) == value)
                {
                    yield return value;
                }
            }
        }

        public string GetSearchUsersQuery(ISearchUsersParameters parameters)
        {
            var query = new StringBuilder(Resources.Search_SearchUsers);

            query.AddParameterToQuery("q", parameters.Query);
            query.AddParameterToQuery("page", parameters.Page);
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("include_entities", parameters.IncludeEntities);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetCreateSavedSearchQuery(ICreateSavedSearchParameters parameters)
        {
            var query = new StringBuilder(Resources.SavedSearch_Create);

            query.AddParameterToQuery("query", parameters.Query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetSavedSearchQuery(IGetSavedSearchParameters parameters)
        {
            var query = new StringBuilder(string.Format(Resources.SavedSearch_Get, parameters.SavedSearchId));

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetListSavedSearchQuery(IListSavedSearchesParameters parameters)
        {
            var query = new StringBuilder(Resources.SavedSearches_List);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetDestroySavedSearchQuery(IDestroySavedSearchParameters parameters)
        {
            var query = new StringBuilder(string.Format(Resources.SavedSearch_Destroy, parameters.SavedSearchId));

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}