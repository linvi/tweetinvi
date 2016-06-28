using System.Collections.Generic;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Provide methods related with search of User on Twitter
    /// </summary>
    public interface IUserSearchEngine : ISearchEngine<IUser>
    {
        /// <summary>
        /// Perform a basic search on Twitter API and returns the T type specified
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="includeEntities">Include entities to the results</param>
        /// <returns>Collection of results</returns>
        List<IUser> Search(string searchQuery, bool includeEntities);

        /// <summary>
        /// Perform a basic search on Twitter API and returns the T type specified
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="resultPerPage">Number of result per page</param>
        /// <param name="pageNumber">Page expected</param>
        /// <param name="includeEntities">Include entities to the results</param>
        /// <returns>Collection of results</returns>
        List<IUser> Search(string searchQuery, int resultPerPage, int pageNumber, bool includeEntities = false);
    }
}