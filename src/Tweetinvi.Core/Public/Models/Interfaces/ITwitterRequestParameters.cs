using System.Collections.Generic;
using System.Net.Http;
using Tweetinvi.Core.Wrappers;

namespace Tweetinvi.Models
{


    /// <summary>
    /// Contains the fields that are required to build an HttpRequest to run the query
    /// </summary>
    public interface ITwitterRequestParameters
    {
        /// <summary>
        /// Query that will be executed.
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// HTTP Method used to execute the query.
        /// </summary>
        HttpMethod HttpMethod { get; set; }

        /// <summary>
        /// Content of the HTTP request.
        /// </summary>
        HttpContent HttpContent { get; set; }

        /// <summary>
        /// Whether the HttpContent should be considered as part of the query url
        /// </summary>
        bool IsHttpContentPartOfQueryParams { get; set; }

        /// <summary>
        /// Content Types accepted by the HttpRequest
        /// </summary>
        List<string> AcceptHeaders { get; }

        /// <summary>
        /// Authorization header that Twitter uses to validate a twitter HttpRequest
        /// </summary>
        string AuthorizationHeader { get; set; }

        /// <summary>
        /// Additional headers to add to the HttpRequest
        /// </summary>
        CustomRequestHeaders CustomHeaders { get; set; }
    }
}