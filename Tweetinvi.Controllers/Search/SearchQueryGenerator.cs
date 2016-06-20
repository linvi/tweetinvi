﻿using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
﻿using Tweetinvi.Core;
﻿using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryGenerator
    {
        string GetSearchTweetsQuery(string query);
        string GetSearchTweetsQuery(ITweetSearchParameters tweetSearchParameters);

        string GetSearchUsersQuery(IUserSearchParameters userSearchParameters);
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

        public string GetSearchTweetsQuery(ITweetSearchParameters tweetSearchParameters)
        {
            if (tweetSearchParameters == null)
            {
                throw new ArgumentNullException("Search parameters cannot be null");
            }

            var searchQuery = GetQuery(tweetSearchParameters.SearchQuery, tweetSearchParameters.Filters);

            _searchQueryValidator.ThrowIfSearchParametersIsNotValid(tweetSearchParameters);

            var query = new StringBuilder(Resources.Search_SearchTweets);

            query.AddParameterToQuery("q", searchQuery);
            query.AddParameterToQuery("geocode", _searchQueryParameterGenerator.GenerateGeoCodeParameter(tweetSearchParameters.GeoCode));

            query.Append(_searchQueryParameterGenerator.GenerateSearchTypeParameter(tweetSearchParameters.SearchType));

            query.Append(_queryParameterGenerator.GenerateSinceIdParameter(tweetSearchParameters.SinceId));
            query.Append(_queryParameterGenerator.GenerateMaxIdParameter(tweetSearchParameters.MaxId));
            query.Append(_queryParameterGenerator.GenerateCountParameter(tweetSearchParameters.MaximumNumberOfResults));

            query.Append(_searchQueryParameterGenerator.GenerateLangParameter(tweetSearchParameters.Lang));
            query.Append(_searchQueryParameterGenerator.GenerateLocaleParameter(tweetSearchParameters.Locale));
            query.Append(_searchQueryParameterGenerator.GenerateSinceParameter(tweetSearchParameters.Since));
            query.Append(_searchQueryParameterGenerator.GenerateUntilParameter(tweetSearchParameters.Until));

            query.Append(_queryParameterGenerator.GenerateTweetModeParameter(_tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode));
            query.Append(_queryParameterGenerator.GenerateAdditionalRequestParameters(tweetSearchParameters.FormattedCustomQueryParameters));

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

        public string GetSearchUsersQuery(IUserSearchParameters userSearchParameters)
        {
            if (!_searchQueryValidator.IsSearchQueryValid(userSearchParameters.SearchQuery))
            {
                throw new ArgumentException("Search query is not valid.");
            }

            var queryBuilder = new StringBuilder(Resources.Search_SearchUsers);

            queryBuilder.AddParameterToQuery("q", _searchQueryParameterGenerator.GenerateSearchQueryParameter(userSearchParameters.SearchQuery));
            queryBuilder.AddParameterToQuery("page", userSearchParameters.Page);
            queryBuilder.Append(_queryParameterGenerator.GenerateCountParameter(userSearchParameters.MaximumNumberOfResults));
            queryBuilder.Append(_queryParameterGenerator.GenerateIncludeEntitiesParameter(userSearchParameters.IncludeEntities));

            return queryBuilder.ToString();
        }
    }
}