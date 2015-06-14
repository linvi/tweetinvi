using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.WebLogic;
using Tweetinvi.Security.System.Security.Cryptography;

namespace Tweetinvi.WebLogic
{
    public class OAuthWebRequestGenerator : IOAuthWebRequestGenerator
    {
        private readonly IWebHelper _webHelper;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public OAuthWebRequestGenerator(
            IWebHelper webHelper,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _webHelper = webHelper;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterStringFormatter = twitterStringFormatter;
        }

        #region Algorithms

        private string GenerateSignature(
            Uri uri,
            HttpMethod httpMethod,
            IEnumerable<IOAuthQueryParameter> queryParameters,
            Dictionary<string, string> urlParameters)
        {
            List<KeyValuePair<String, String>> signatureParameters = urlParameters.OrderBy(x => x.Key).ToList();

            #region Store the paramaters that will be used

            // Add all the parameters that are required to generate a signature
            var oAuthQueryParameters = queryParameters as IList<IOAuthQueryParameter> ?? queryParameters.ToList();
            foreach (var header in (from h in oAuthQueryParameters
                                    where h.RequiredForSignature
                                    orderby h.Key
                                    select h))
            {
                signatureParameters.Add(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            #endregion

            #region Generate OAuthRequest Parameters

            StringBuilder urlParametersFormatted = new StringBuilder();
            foreach (KeyValuePair<string, string> param in (from p in signatureParameters orderby p.Key select p))
            {
                if (urlParametersFormatted.Length > 0)
                {
                    urlParametersFormatted.Append("&");
                }

                urlParametersFormatted.Append(string.Format("{0}={1}", param.Key, param.Value));
            }

            #endregion

            #region Generate OAuthRequest

            string url = uri.Query == "" ? uri.AbsoluteUri : uri.AbsoluteUri.Replace(uri.Query, "");

            string oAuthRequest = string.Format("{0}&{1}&{2}",
                httpMethod,
                StringFormater.UrlEncode(url),
                StringFormater.UrlEncode(urlParametersFormatted.ToString()));

            #endregion

            #region Generate OAuthSecretKey
            // Generate OAuthSecret that is required to generate a signature
            IEnumerable<IOAuthQueryParameter> oAuthSecretKeyHeaders = from h in oAuthQueryParameters
                                                                      where h.IsPartOfOAuthSecretKey
                                                                      orderby h.Key
                                                                      select h;
            string oAuthSecretkey = "";

            for (int i = 0; i < oAuthSecretKeyHeaders.Count(); ++i)
            {
                oAuthSecretkey += String.Format("{0}{1}",
                    StringFormater.UrlEncode(oAuthSecretKeyHeaders.ElementAt(i).Value),
                    (i == oAuthSecretKeyHeaders.Count() - 1) ? "" : "&");
            }

            #endregion

            // Create and return signature

            HMACSHA1Generator hmacsha1Generator = new HMACSHA1Generator();
            return StringFormater.UrlEncode(Convert.ToBase64String(hmacsha1Generator.ComputeHash(oAuthRequest, oAuthSecretkey, Encoding.UTF8)));
        }

        private string GenerateHeader(
            Uri uri,
            HttpMethod httpMethod,
            List<IOAuthQueryParameter> queryParameters,
            Dictionary<string, string> urlParameters)
        {
            string signature = GenerateSignature(uri, httpMethod, queryParameters, urlParameters);

            IOAuthQueryParameter oAuthSignature = new OAuthQueryParameter("oauth_signature", signature, false, false, false);
            queryParameters.Add(oAuthSignature);

            StringBuilder header = new StringBuilder("OAuth ");

            foreach (var param in (from p in queryParameters
                                   where p.RequiredForHeader
                                   orderby p.Key
                                   select p))
            {
                if (header.Length > 6)
                {
                    header.Append(",");
                }

                header.Append(string.Format("{0}=\"{1}\"", param.Key, param.Value));
            }

            header.AppendFormat(",oauth_signature=\"{0}\"", signature);

            return header.ToString();
        }

        /// <summary>
        /// Method Allowing to initialize a SortedDictionnary to enable oAuth query to be generated with
        /// these parameters
        /// </summary>
        /// <returns>Call the method defined in the _generateDelegate and return a string result
        /// This result will be the header of the WebRequest.</returns>
        private List<IOAuthQueryParameter> GenerateHeaderParameters(IEnumerable<IOAuthQueryParameter> queryParameters)
        {
            List<IOAuthQueryParameter> result = queryParameters.ToList();

            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            string oauthTimestamp = Convert.ToInt64(ts.TotalSeconds).ToString(CultureInfo.InvariantCulture);
            string oauthNonce = new Random().Next(123400, 9999999).ToString(CultureInfo.InvariantCulture);

            // Required information
            result.Add(new OAuthQueryParameter("oauth_nonce", oauthNonce, true, true, false));
            result.Add(new OAuthQueryParameter("oauth_timestamp", oauthTimestamp, true, true, false));
            result.Add(new OAuthQueryParameter("oauth_version", "1.0", true, true, false));
            result.Add(new OAuthQueryParameter("oauth_signature_method", "HMAC-SHA1", true, true, false));

            return result;
        }

        #endregion

        public IEnumerable<IOAuthQueryParameter> GenerateConsumerParameters(IConsumerCredentials consumerCredentials)
        {
            var consumerHeaders = new List<IOAuthQueryParameter>();

            // Add Header for every connection to a Twitter Application
            if (consumerCredentials != null && !String.IsNullOrEmpty(consumerCredentials.ConsumerKey) && !String.IsNullOrEmpty(consumerCredentials.ConsumerSecret))
            {
                consumerHeaders.Add(new OAuthQueryParameter("oauth_consumer_key", StringFormater.UrlEncode(consumerCredentials.ConsumerKey), true, true, false));
                consumerHeaders.Add(new OAuthQueryParameter("oauth_consumer_secret", StringFormater.UrlEncode(consumerCredentials.ConsumerSecret), false, false, true));
            }

            return consumerHeaders;
        }

        public IEnumerable<IOAuthQueryParameter> GenerateApplicationParameters(ITemporaryCredentials temporaryCredentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null)
        {
            var headers = GenerateConsumerParameters(temporaryCredentials).ToList();

            // Add Header for authenticated connection to a Twitter Application
            if (temporaryCredentials != null && !String.IsNullOrEmpty(temporaryCredentials.AuthorizationKey) && !String.IsNullOrEmpty(temporaryCredentials.AuthorizationSecret))
            {
                headers.Add(new OAuthQueryParameter("oauth_token", StringFormater.UrlEncode(temporaryCredentials.AuthorizationKey), true, true, false));
                headers.Add(new OAuthQueryParameter("oauth_token_secret", StringFormater.UrlEncode(temporaryCredentials.AuthorizationSecret), false, false, true));
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

        public IEnumerable<IOAuthQueryParameter> GenerateParameters(IOAuthCredentials credentials, IEnumerable<IOAuthQueryParameter> additionalParameters = null)
        {
            var headers = GenerateConsumerParameters(credentials).ToList();

            // Add Header for authenticated connection to a Twitter Application
            if (credentials != null && !String.IsNullOrEmpty(credentials.AccessToken) && !String.IsNullOrEmpty(credentials.AccessTokenSecret))
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

        public byte[] GenerateMultipartContent(string url, HttpMethod httpMethod, IMultipartRequestConfiguration multipartRequestConfiguration, IEnumerable<IMultipartElement> multipartElements)
        {
            var urlParameters = _webHelper.GetURLParameters(url);

            var multipartContent = new StringBuilder();
            foreach (var urlParameter in urlParameters)
            {
                if (urlParameter.Value != null)
                {
                    var notEncodedParameterValue = _twitterStringFormatter.TwitterDecode(urlParameter.Value);
                    byte[] parameterBytes = Encoding.UTF8.GetBytes(notEncodedParameterValue);
                    string encodedParameter = multipartRequestConfiguration.EncodingAlgorithm.GetString(parameterBytes, 0, parameterBytes.Length);

                    multipartContent.AppendFormat("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n",
                        multipartRequestConfiguration.Boundary, urlParameter.Key, encodedParameter);
                }
            }

            foreach (var multiPartElement in multipartElements)
            {
                multipartContent.Append(multiPartElement);
            }

            multipartContent.Append(multipartRequestConfiguration.EndBoundary);
            return multipartRequestConfiguration.EncodingAlgorithm.GetBytes(multipartContent.ToString());
        }

        public HttpWebRequest GenerateWebRequest(string url, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters)
        {
            Uri uri = new Uri(url);
            var header = GenerateAuthorizationHeader(uri, httpMethod, parameters);

            // This debug is only compiled in debug mode and display the executed queries
# if DEBUG
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (_tweetinviSettingsAccessor.ShowDebug)
            // ReSharper disable once CSharpWarnings::CS0162
            {
                Debug.WriteLine("{0} : {1}", httpMethod, uri.AbsoluteUri);
                Debug.WriteLine("Header {0}", header);
            }
# endif

            var webRequest = WebRequest.CreateHttp(uri.AbsoluteUri);
            webRequest.Method = httpMethod.ToString();
            webRequest.Headers["Authorization"] = header;

            return webRequest;
        }

        public string GenerateAuthorizationHeader(Uri uri, HttpMethod httpMethod, IEnumerable<IOAuthQueryParameter> parameters)
        {
            var queryParameters = GenerateHeaderParameters(parameters);
            var urlParameters = _webHelper.GetUriParameters(uri);
            return GenerateHeader(uri, httpMethod, queryParameters, urlParameters);
        }
    }
}