using System;
using System.Runtime.InteropServices;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information read : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-users-search
    /// </summary>
    public interface ISearchUsersParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Query to search for people.
        /// </summary>
        string Query { get; set; }

        /// <summary>
        /// Search result page to retrieve.
        /// </summary>
        int? Page { get; set; }

        /// <summary>
        /// Number of Users to Retrieve.
        /// Cannot be more than 1000 as per the documentation.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Retrieve the user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/users/search
    /// </summary>
    public class SearchUsersParameters : CustomRequestParameters, ISearchUsersParameters
    {
        public SearchUsersParameters(string query)
        {
            PageSize = TwitterLimits.DEFAULTS.SEARCH_USERS_MAX_PAGE_SIZE;
            Query = query;
            IncludeEntities = true;
            Page = 1;
        }

        public SearchUsersParameters(ISearchUsersParameters source) : base(source)
        {
            if (source == null)
            {
                PageSize = TwitterLimits.DEFAULTS.SEARCH_USERS_MAX_PAGE_SIZE;
                return;
            }

            PageSize = source.PageSize;
            Query = source.Query;
            IncludeEntities = source.IncludeEntities;
            Page = source.Page;
        }

        public string Query { get; set; }

        private int? _page;

        public int? Page
        {
            get => _page;
            set
            {
                if (value == null)
                {
                    _page = null;
                }
                else
                {
                    if (_page < 1)
                    {
                        throw new ArgumentOutOfRangeException(nameof(Page), "Search users page number cannot be lower than 1");
                    }

                    _page = value;
                }
            }
        }

        public int PageSize { get; set; }

        public bool? IncludeEntities { get; set; }
    }
}