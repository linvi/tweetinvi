using System;
using System.Collections.Generic;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Web
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
        /// Generate authorization headers for a query with the specified OAuth fields.
        /// </summary>
        string GenerateAuthorizationHeader(Uri uri, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters);

        /// <summary>
        /// Generate authorization headers for a query with the specified OAuth fields.
        /// </summary>
        string SetTwitterQueryAuthorizationHeader(ITwitterQuery twitterQuery);
    }
}