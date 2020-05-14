using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models.Authentication;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Security.System.Security.Cryptography;
using HttpMethod = Tweetinvi.Models.HttpMethod;

namespace Tweetinvi.WebLogic
{
    public interface IOAuthWebRequestGeneratorFactory
    {
        IOAuthWebRequestGenerator Create();
        IOAuthWebRequestGenerator Create(ITwitterRequest request);
    }

    public class OAuthWebRequestGeneratorFactory : IOAuthWebRequestGeneratorFactory
    {
        private readonly IWebHelper _webHelper;

        public OAuthWebRequestGeneratorFactory(IWebHelper webHelper)
        {
            _webHelper = webHelper;
        }

        public IOAuthWebRequestGenerator Create()
        {
            return new OAuthWebRequestGenerator(_webHelper, () =>
            {
                return DateTime.UtcNow;
            });
        }

        public IOAuthWebRequestGenerator Create(ITwitterRequest request)
        {
            return new OAuthWebRequestGenerator(_webHelper, request.ExecutionContext.GetUtcDateTime);
        }
    }

    public class OAuthWebRequestGenerator : IOAuthWebRequestGenerator
    {
        private readonly IWebHelper _webHelper;
        private readonly Func<DateTime> _getUtcDateTime;

        public OAuthWebRequestGenerator(IWebHelper webHelper, Func<DateTime> getUtcDateTime)
        {
            _webHelper = webHelper;
            _getUtcDateTime = getUtcDateTime;
        }

        #region Algorithms

        private string GenerateSignature(
            Uri uri,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> queryParameters,
            Dictionary<string, string> urlParameters)
        {
            var oAuthQueryParameters = queryParameters.ToArray();
            var signatureParameters = GetSignatureParameters(oAuthQueryParameters, urlParameters);
            var formattedUrlParameters = CreateFormattedUrlParameters(signatureParameters);
            var oAuthRequest = CreateOAuthRequest(uri, httpMethod, formattedUrlParameters);
            var oAuthSecretKey = CreateOAuthSecretKey(oAuthQueryParameters);

            var hmacsha1Generator = new HMACSHA1Generator();
            return StringFormater.UrlEncode(Convert.ToBase64String(hmacsha1Generator.ComputeHash(oAuthRequest, oAuthSecretKey, Encoding.UTF8)));
        }

        private static IEnumerable<KeyValuePair<string, string>> GetSignatureParameters(IEnumerable<IOAuthQueryParameter> queryParameters, Dictionary<string, string> urlParameters)
        {
            var signatureParameters = urlParameters.OrderBy(x => x.Key).ToList();
            var queryParametersRequiredForSignature = queryParameters.Where(x => x.RequiredForSignature);

            queryParametersRequiredForSignature.ForEach(x => { signatureParameters.Add(new KeyValuePair<string, string>(x.Key, x.Value)); });

            return signatureParameters;
        }

        private static string CreateFormattedUrlParameters(IEnumerable<KeyValuePair<string, string>> signatureParameters)
        {
            var orderedParameters = signatureParameters.OrderBy(x => x.Key);
            return string.Join("&", orderedParameters.Select(x => $"{x.Key}={x.Value}"));
        }

        private static string CreateOAuthSecretKey(IEnumerable<IOAuthQueryParameter> oAuthQueryParameters)
        {
            var oAuthSecretKeyHeaders = oAuthQueryParameters.Where(x => x.IsPartOfOAuthSecretKey)
                .OrderBy(x => x.Key)
                .Select(x => StringFormater.UrlEncode(x.Value));

            return string.Join("&", oAuthSecretKeyHeaders);
        }

        private static string CreateOAuthRequest(Uri uri, HttpMethod httpMethod, string urlParametersFormatted)
        {
            var url = uri.Query == "" ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, "");
            var encodedUrl = StringFormater.UrlEncode(url);
            var encodedParameters = StringFormater.UrlEncode(urlParametersFormatted);

            return $"{httpMethod}&{encodedUrl}&{encodedParameters}";
        }

        private string GenerateHeader(
            Uri uri,
            HttpMethod httpMethod,
            ICollection<IOAuthQueryParameter> queryParameters,
            Dictionary<string, string> urlParameters)
        {
            var signature = GenerateSignature(uri, httpMethod, queryParameters, urlParameters);
            var oAuthSignature = new OAuthQueryParameter("oauth_signature", signature, false, false, false);
            queryParameters.Add(oAuthSignature);

            var parametersFormattedForHeader = CreateParametersFormattedForHeader(queryParameters);
            var headerSignature = $"oauth_signature=\"{signature}\"";

            return $"OAuth {parametersFormattedForHeader},{headerSignature}";
        }

        private static string CreateParametersFormattedForHeader(ICollection<IOAuthQueryParameter> queryParameters)
        {
            var parametersForHeader = queryParameters.Where(x => x.RequiredForHeader)
                .OrderBy(x => x.Key)
                .Select(param => $"{param.Key}=\"{param.Value}\"");

            return string.Join(",", parametersForHeader);
        }

        /// <summary>
        /// Method Allowing to initialize a SortedDictionnary to enable oAuth query to be generated with
        /// these parameters
        /// </summary>
        /// <returns>Call the method defined in the _generateDelegate and return a string result
        /// This result will be the header of the WebRequest.</returns>
        private List<IOAuthQueryParameter> GenerateAdditionalHeaderParameters(IEnumerable<IOAuthQueryParameter> queryParameters)
        {
            var result = queryParameters.ToList();

            var dateTime = _getUtcDateTime();
            var ts = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
            var oauthNonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);

            // Required information
            result.Add(new OAuthQueryParameter("oauth_nonce", oauthNonce, true, true, false));
            result.Add(new OAuthQueryParameter("oauth_timestamp", oauthTimestamp, true, true, false));
            result.Add(new OAuthQueryParameter("oauth_version", "1.0", true, true, false));
            result.Add(new OAuthQueryParameter("oauth_signature_method", "HMAC-SHA1", true, true, false));

            return result;
        }

        #endregion

        private static IEnumerable<IOAuthQueryParameter> GenerateConsumerParameters(IReadOnlyConsumerCredentials consumerCredentials)
        {
            var consumerHeaders = new List<IOAuthQueryParameter>();

            // Add Header for every connection to a Twitter Application
            if (consumerCredentials != null &&
                !string.IsNullOrEmpty(consumerCredentials.ConsumerKey) &&
                !string.IsNullOrEmpty(consumerCredentials.ConsumerSecret))
            {
                consumerHeaders.Add(new OAuthQueryParameter("oauth_consumer_key", StringFormater.UrlEncode(consumerCredentials.ConsumerKey), true, true, false));
                consumerHeaders.Add(new OAuthQueryParameter("oauth_consumer_secret", StringFormater.UrlEncode(consumerCredentials.ConsumerSecret), false, false, true));
            }

            return consumerHeaders;
        }

        public IEnumerable<IOAuthQueryParameter> GenerateApplicationParameters(
            IReadOnlyConsumerCredentials temporaryCredentials,
            IAuthenticationRequest authRequest = null,
            IEnumerable<IOAuthQueryParameter> additionalParameters = null)
        {
            var headers = GenerateConsumerParameters(temporaryCredentials).ToList();

            // Add Header for authenticated connection to a Twitter Application
            if (authRequest != null &&
                !string.IsNullOrEmpty(authRequest.AuthorizationKey) &&
                !string.IsNullOrEmpty(authRequest.AuthorizationSecret))
            {
                headers.Add(new OAuthQueryParameter("oauth_token", StringFormater.UrlEncode(authRequest.AuthorizationKey), true, true, false));
                headers.Add(new OAuthQueryParameter("oauth_token_secret", StringFormater.UrlEncode(authRequest.AuthorizationSecret), false, false, true));
            }
            else
            {
                headers.Add(new OAuthQueryParameter("oauth_token", "", false, false, true));
            }

            if (additionalParameters != null)
            {
                headers.AddRange(additionalParameters);
            }

            return headers;
        }

        public IEnumerable<IOAuthQueryParameter> GenerateParameters(IReadOnlyTwitterCredentials credentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null)
        {
            var headers = GenerateConsumerParameters(credentials).ToList();

            // Add Header for authenticated connection to a Twitter Application
            if (credentials != null &&
                !string.IsNullOrEmpty(credentials.AccessToken) &&
                !string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                headers.Add(new OAuthQueryParameter("oauth_token", StringFormater.UrlEncode(credentials.AccessToken), true, true, false));
                headers.Add(new OAuthQueryParameter("oauth_token_secret", StringFormater.UrlEncode(credentials.AccessTokenSecret), false, false, true));
            }
            else
            {
                headers.Add(new OAuthQueryParameter("oauth_token", "", false, false, true));
            }

            if (additionalParameters != null)
            {
                headers.AddRange(additionalParameters);
            }

            return headers;
        }

        public IOAuthQueryParameter GenerateParameter(string key, string value, bool requiredForSignature, bool requiredForHeader, bool isPartOfOAuthSecretKey)
        {
            return new OAuthQueryParameter(key, StringFormater.UrlEncode(value), requiredForSignature, requiredForHeader, isPartOfOAuthSecretKey);
        }

        public string GenerateAuthorizationHeader(Uri uri, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters)
        {
            var queryParameters = GenerateAdditionalHeaderParameters(parameters);
            var urlParameters = _webHelper.GetUriParameters(uri);
            return GenerateHeader(uri, httpMethod, queryParameters, urlParameters);
        }

        public async Task<string> GenerateAuthorizationHeaderAsync(
            Uri uri,
            HttpContent queryContent,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> parameters)
        {
            var queryParameters = GenerateAdditionalHeaderParameters(parameters);
            var urlParameters = _webHelper.GetUriParameters(uri);

            if (queryContent != null)
            {
                var query = await queryContent.ReadAsStringAsync().ConfigureAwait(false);
                var additionalParameters = _webHelper.GetQueryParameters(query);

                additionalParameters.ForEach(x => { urlParameters.Add(x.Key, x.Value); });
            }

            return GenerateHeader(uri, httpMethod, queryParameters, urlParameters);
        }

        public async Task<string> SetTwitterQueryAuthorizationHeaderAsync(ITwitterQuery twitterQuery)
        {
            var credentials = twitterQuery.TwitterCredentials;

            if (!string.IsNullOrEmpty(credentials.AccessToken) && !string.IsNullOrEmpty(credentials.AccessTokenSecret))
            {
                var uri = new Uri(twitterQuery.Url);
                var credentialsParameters = GenerateParameters(twitterQuery.TwitterCredentials);

                if (twitterQuery.HttpContent != null && twitterQuery.IsHttpContentPartOfQueryParams)
                {
                    twitterQuery.AuthorizationHeader = await GenerateAuthorizationHeaderAsync(uri, twitterQuery.HttpContent, twitterQuery.HttpMethod, credentialsParameters).ConfigureAwait(false);
                }
                else
                {
                    twitterQuery.AuthorizationHeader = GenerateAuthorizationHeader(uri, twitterQuery.HttpMethod, credentialsParameters);
                }
            }
            else
            {
                twitterQuery.AuthorizationHeader = $"Bearer {credentials.BearerToken}";
            }

            return twitterQuery.AuthorizationHeader;
        }
    }
}