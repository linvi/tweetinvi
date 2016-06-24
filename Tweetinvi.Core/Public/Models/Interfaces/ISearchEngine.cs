using System.Collections.Generic;

namespace Tweetinvi.Models
{
    /// <summary>
    /// Provide methods related with search on Twitter
    /// </summary>
    /// <typeparam name="T">Type of object retrieve from the research</typeparam>
    public interface ISearchEngine<T> where T : class 
    {
        /// <summary>
        /// Perform a basic search on Twitter API and returns the T type specified
        /// </summary>
        /// <param name="searchQuery">Search to be sent</param>
        /// <param name="token">Token used to perform the search</param>
        /// <returns>Collection of results</returns>
        List<T> Search(string searchQuery);
    }
}