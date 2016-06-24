using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Wrappers;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryHelper
    {
        ISearchTweetsParameters CloneTweetSearchParameters(ISearchTweetsParameters searchTweetsParameters);
        List<ITweetDTO> GetTweetsFromJsonResponse(string json);
        List<ITweetDTO> GetTweetsFromJsonObject(JObject jObject);

        ISearchUsersParameters CloneUserSearchParameters(ISearchUsersParameters searchUsersParameters);
    }

    public class SearchQueryHelper : ISearchQueryHelper
    {
        private readonly IJObjectStaticWrapper _jObjectWrapper;

        public SearchQueryHelper(IJObjectStaticWrapper jObjectWrapper)
        {
            _jObjectWrapper = jObjectWrapper;
        }

        public ISearchTweetsParameters CloneTweetSearchParameters(ISearchTweetsParameters searchTweetsParameters)
        {
            var clone = new SearchTweetsParameters(searchTweetsParameters.SearchQuery)
            {
                Filters = searchTweetsParameters.Filters,
                GeoCode = searchTweetsParameters.GeoCode,
                Lang = searchTweetsParameters.Lang,
                Locale = searchTweetsParameters.Locale,
                MaxId = searchTweetsParameters.MaxId,
                MaximumNumberOfResults = searchTweetsParameters.MaximumNumberOfResults,
                SearchType = searchTweetsParameters.SearchType,
                Since = searchTweetsParameters.Since,
                SinceId = searchTweetsParameters.SinceId,
                TweetSearchType = searchTweetsParameters.TweetSearchType,
                Until = searchTweetsParameters.Until
            };

            return clone;
        }

        public List<ITweetDTO> GetTweetsFromJsonResponse(string json)
        {
            var jObject = _jObjectWrapper.GetJobjectFromJson(json);
            return GetTweetsFromJsonObject(jObject);
        }

        public List<ITweetDTO> GetTweetsFromJsonObject(JObject jObject)
        {
            if (jObject == null)
            {
                return null;
            }

            return _jObjectWrapper.ToObject<List<ITweetDTO>>(jObject["statuses"]);
        }

        public ISearchUsersParameters CloneUserSearchParameters(ISearchUsersParameters searchUsersParameters)
        {
            var clone = new SearchUsersParameters(searchUsersParameters.SearchQuery)
            {
                IncludeEntities = searchUsersParameters.IncludeEntities,
                Page = searchUsersParameters.Page,
                MaximumNumberOfResults = searchUsersParameters.MaximumNumberOfResults
            };

            return clone;
        }
    }
}