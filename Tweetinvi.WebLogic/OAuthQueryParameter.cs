using Tweetinvi.Core.Web;

namespace Tweetinvi.WebLogic
{
    /// <summary>
    /// Information used to generate an OAuth query
    /// </summary>
    public class OAuthQueryParameter : IOAuthQueryParameter
    {
        #region Public Properties

        public string Key { get; set; }
        public string Value { get; set; }
        public bool RequiredForSignature { get; set; }
        public bool RequiredForHeader { get; set; }
        public bool IsPartOfOAuthSecretKey { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an OAuthQuery parameter that will be required to create 
        /// an OAuth HttpWebRequest
        /// </summary>
        /// <param name="key">Parameter name</param>
        /// <param name="value">Parameter value</param>
        /// <param name="requiredForSignature">Is this parameter required to generate the signature</param>
        /// <param name="requiredForHeader">Is this parameter required to generate the secret key</param>
        /// <param name="isPartOfOAuthSecretKey">Is this parameter required to generate the secret key</param>
        public OAuthQueryParameter(string key,
            string value,
            bool requiredForSignature,
            bool requiredForHeader,
            bool isPartOfOAuthSecretKey)
        {
            Key = key;
            Value = value;
            RequiredForSignature = requiredForSignature;
            RequiredForHeader = requiredForHeader;
            IsPartOfOAuthSecretKey = isPartOfOAuthSecretKey;
        }

        #endregion
    }
}