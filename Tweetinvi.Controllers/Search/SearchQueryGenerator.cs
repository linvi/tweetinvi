﻿using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
﻿using Tweetinvi.Core;
﻿using Tweetinvi.Core.Extensions;
﻿using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryGenerator
    {
        string GetSearchTweetsQuery(string query);
        string GetSearchTweetsQuery(ISearchTweetsParameters searchTweetsParameters);

        string GetSearchUsersQuery(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchQueryGenerator : ISearchQueryGenerator
    {
        private readonly ISearchQueryValidator _searchQueryValidator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ISearchQueryParameterGenerator _searchQueryParameterGenerator;

        public SearchQueryGenerator(
            ISearchQueryValidator searchQueryValidator,
            IQueryParameterGenerator queryParameterGenerator,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ISearchQueryParameterGenerator searchQueryParameterGenerator)
        {
            _searchQueryValidator = searchQueryValidator;
            _queryParameterGenerator = queryParameterGenerator;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _searchQueryParameterGenerator = searchQueryParameterGenerator;
        }

        public string GetSearchTweetsQuery(string query)
        {
            var searchParameter = _searchQueryParameterGenerator.CreateSearchTweetParameter(query);
            return GetSearchTweetsQuery(searchParameter);
        }

        public string GetSearchTweetsQuery(ISearchTweetsParameters searchTweetsParameters)
        {
            if (searchTweetsParameters == null)
            {
                throw new ArgumentNullException("Search parameters cannot be null");
            }

            var searchQuery = GetQuery(searchTweetsParameters.SearchQuery, searchTweetsParameters.Filters);

            _searchQueryValidator.ThrowIfSearchParametersIsNotValid(searchTweetsParameters);

            var query = new StringBuilder(Resources.Search_SearchTweets);

            query.AddParameterToQuery("q", searchQuery);
            query.AddParameterToQuery("geocode", _searchQueryParameterGenerator.GenerateGeoCodeParameter(searchTweetsParameters.GeoCode));

            query.Append(_searchQueryParameterGenerator.GenerateSearchTypeParameter(searchTweetsParameters.SearchType));

            query.Append(_queryParameterGenerator.GenerateSinceIdParameter(searchTweetsParameters.SinceId));
            query.Append(_queryParameterGenerator.GenerateMaxIdParameter(searchTweetsParameters.MaxId));
            query.Append(_queryParameterGenerator.GenerateCountParameter(searchTweetsParameters.MaximumNumberOfResults));

            query.Append(_searchQueryParameterGenerator.GenerateLangParameter(searchTweetsParameters.Lang));
            query.Append(_searchQueryParameterGenerator.GenerateLocaleParameter(searchTweetsParameters.Locale));
            query.Append(_searchQueryParameterGenerator.GenerateSinceParameter(searchTweetsParameters.Since));
            query.Append(_searchQueryParameterGenerator.GenerateUntilParameter(searchTweetsParameters.Until));

            query.AddFormattedParameterToQuery(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(searchTweetsParameters.FormattedCustomQueryParameters));

            return query.ToString();
        }

        private string GetQuery(string query, TweetSearchFilters tweetSearchFilters)
        {
            query = _searchQueryParameterGenerator.GenerateSearchQueryParameter(query);

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

        public string GetSearchUsersQuery(ISearchUsersParameters searchUsersParameters)
        {
            if (!_searchQueryValidator.IsSearchQueryValid(searchUsersParameters.SearchQuery))
            {
                throw new ArgumentException("Search query is not valid.");
            }

            var queryBuilder = new StringBuilder(Resources.Search_SearchUsers);

            queryBuilder.AddParameterToQuery("q", _searchQueryParameterGenerator.GenerateSearchQueryParameter(searchUsersParameters.SearchQuery));
            queryBuilder.AddParameterToQuery("page", searchUsersParameters.Page);
            queryBuilder.Append(_queryParameterGenerator.GenerateCountParameter(searchUsersParameters.MaximumNumberOfResults));
            queryBuilder.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(searchUsersParameters.IncludeEntities));

            return queryBuilder.ToString();
        }
    }
}