using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchQueryHelper
    {
        ITweetSearchParameters CloneTweetSearchParameters(ITweetSearchParameters tweetSearchParameters);
        List<ITweetDTO> GetTweetsFromJsonResponse(string json);
        List<ITweetDTO> GetTweetsFromJsonObject(JObject jObject);

        IUserSearchParameters CloneUserSearchParameters(IUserSearchParameters userSearchParameters);
    }

    public class SearchQueryHelper : ISearchQueryHelper
    {
        private readonly IJObjectStaticWrapper _jObjectWrapper;

        public SearchQueryHelper(IJObjectStaticWrapper jObjectWrapper)
        {
            _jObjectWrapper = jObjectWrapper;
        }

        public ITweetSearchParameters CloneTweetSearchParameters(ITweetSearchParameters tweetSearchParameters)
        {
            var clone = new TweetSearchParameters(tweetSearchParameters.SearchQuery)
            {
                Filters = tweetSearchParameters.Filters,
                GeoCode = tweetSearchParameters.GeoCode,
                Lang = tweetSearchParameters.Lang,
                Locale = tweetSearchParameters.Locale,
                MaxId = tweetSearchParameters.MaxId,
                MaximumNumberOfResults = tweetSearchParameters.MaximumNumberOfResults,
                SearchType = tweetSearchParameters.SearchType,
                Since = tweetSearchParameters.Since,
                SinceId = tweetSearchParameters.SinceId,
                TweetSearchType = tweetSearchParameters.TweetSearchType,
                Until = tweetSearchParameters.Until
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

        public IUserSearchParameters CloneUserSearchParameters(IUserSearchParameters userSearchParameters)
        {
            var clone = new UserSearchParameters(userSearchParameters.SearchQuery)
            {
                IncludeEntities = userSearchParameters.IncludeEntities,
                Page = userSearchParameters.Page,
                MaximumNumberOfResults = userSearchParameters.MaximumNumberOfResults
            };

            return clone;
        }
    }
}