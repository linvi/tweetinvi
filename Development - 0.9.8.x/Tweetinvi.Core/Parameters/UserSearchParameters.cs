using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class UserSearchParameters : CustomRequestParameters, IUserSearchParameters
    {
        public UserSearchParameters(string query)
        {
            MaximumNumberOfResults = TweetinviConsts.SEARCH_USERS_COUNT;
            SearchQuery = query;
            IncludeEntities = true;
            Page = 0;
        }

        public string SearchQuery { get; set; }

        public int Page { get; set; }

        public int MaximumNumberOfResults { get; set; }

        public bool IncludeEntities { get; set; }
    }
}
