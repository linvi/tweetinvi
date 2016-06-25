namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Information used to generate an OAuth query
    /// </summary>
    public interface IOAuthQueryParameter
    {
        /// <summary>
        /// Parameter name
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// Parameter value
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// Is this parameter required to generate the signature
        /// </summary>
        bool RequiredForSignature { get; set; }

        /// <summary>
        /// Is this parameter required to generate the headers
        /// </summary>
        bool RequiredForHeader { get; set; } 

        /// <summary>
        /// Is this parameter required to generate the secret key
        /// </summary>
        bool IsPartOfOAuthSecretKey { get; set; }
    }
}