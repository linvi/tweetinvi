namespace Tweetinvi.Core.Interfaces.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/get/users/search
    /// </summary>
    public interface IUserSearchParameters : ICustomRequestParameters
    {
        /// <summary>
        /// Query to search for people
        /// </summary>
        string SearchQuery { get; set; }

        /// <summary>
        /// Page of result to retrieve
        /// </summary>
        int Page { get; set; }

        /// <summary>
        /// Number of Users to Retrieve.
        /// Cannot be more than 1000 as per the documentation.
        /// </summary>
        int MaximumNumberOfResults { get; set; }

        /// <summary>
        /// User Entities properties will be set
        /// </summary>
        bool IncludeEntities { get; set; }
    }
}
