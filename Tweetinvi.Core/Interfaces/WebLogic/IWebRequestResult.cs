using System.Collections.Generic;
using System.IO;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface IWebRequestResult
    {
        /// <summary>
        /// Query url.
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Resulting stream to retrieve the data.
        /// </summary>
        Stream ResultStream { get; set; }

        /// <summary>
        /// Status Code of the query execution.
        /// </summary>
        int StatusCode { get; set; }

        /// <summary>
        /// Inform whether the query has succeeded.
        /// </summary>
        bool IsSuccessStatusCode { get; set; }

        /// <summary>
        /// Headers of the response.
        /// </summary>
        Dictionary<string, IEnumerable<string>> Headers { get; set; }
    }
}