using System;
using System.Collections.Generic;
using System.Net;
using Tweetinvi.Core.Authentication;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    /// <summary>
    /// Generator of HttpWebRequest using OAuth specification
    /// </summary>
    public interface IOAuthWebRequestGenerator
    {
        /// <summary>
        /// Generate an OAuth query parameter
        /// </summary>
        IOAuthQueryParameter GenerateParameter(string key, string value, bool requiredForSignature, bool requiredForHeader, bool isPartOfOAuthSecretKey);
        
        /// <summary>
        /// Generate all the query parameters for a user connection.
        /// </summary>
        IEnumerable<IOAuthQueryParameter> GenerateConsumerParameters(IConsumerCredentials consumerCredentials);

        /// <summary>
        /// Generate all the query parameters for an application connection.
        /// </summary>
        IEnumerable<IOAuthQueryParameter> GenerateApplicationParameters(
            IConsumerCredentials temporaryCredentials, 
            IAuthenticationToken authenticationToken = null,
            IEnumerable<IOAuthQueryParameter> additionalParameters = null);

        /// <summary>
        /// Generate the authentication parameters from Twitter credentials.
        /// </summary>
        IEnumerable<IOAuthQueryParameter> GenerateParameters(ITwitterCredentials credentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null);

        /// <summary>
        /// Generates an HttpWebRequest by giving minimal information
        /// </summary>
        /// <param name="url">URL we expect to send/retrieve information to/from</param>
        /// <param name="httpMethod">HTTP Method for the request</param>
        /// <param name="parameters">Parameters used to generate the query</param>
        /// <returns>The appropriate WebRequest</returns>
        HttpWebRequest GenerateWebRequest(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);

        /// <summary>
        /// Generate authorization headers for a query with the specified OAuth fields.
        /// </summary>
        string GenerateAuthorizationHeader(Uri uri, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);

        /// <summary>
        /// Generate authorization headers for a query with the specified OAuth fields.
        /// </summary>
        string GenerateAuthorizationHeader(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);
    }
}