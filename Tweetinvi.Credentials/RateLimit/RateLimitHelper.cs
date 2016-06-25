using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models;
using Tweetinvi.Core.RateLimit;
using Tweetinvi.Models;

namespace Tweetinvi.Credentials.RateLimit
{
    public class RateLimitHelper : IRateLimitHelper
    {
        private readonly IWebHelper _webHelper;
        private readonly IAttributeHelper _attributeHelper;

        public RateLimitHelper(IWebHelper webHelper, IAttributeHelper attributeHelper)
        {
            _webHelper = webHelper;
            _attributeHelper = attributeHelper;
        }

        public IEndpointRateLimit GetEndpointRateLimitFromQuery(string query, ICredentialsRateLimits rateLimits, bool createIfNotExist)
        {
            var queryBaseURL = _webHelper.GetBaseURL(query);
            if (rateLimits == null || queryBaseURL == null)
            {
                return null;
            }

            var tokenAttributes = _attributeHelper.GetAllPropertiesAttributes<ICredentialsRateLimits, TwitterEndpointAttribute>();
            var matchingAttribute = tokenAttributes.Keys.JustOneOrDefault(x => IsEndpointURLMatchingQueryURL(queryBaseURL, x));

            // In the default list of rate limits
            if (matchingAttribute != null)
            {
                var matchingProperty = tokenAttributes[matchingAttribute];
                return GetRateLimitFromProperty(matchingProperty, rateLimits);
            }

            // In the other endpoint rate limits
            var matchingKeyPair =  rateLimits.OtherEndpointRateLimits.FirstOrDefault(x => IsEndpointURLMatchingQueryURL(queryBaseURL, x.Key));
            if (!matchingKeyPair.Equals(default(KeyValuePair<TwitterEndpointAttribute, IEndpointRateLimit>)))
            {
                return matchingKeyPair.Value;
            }

            if (!createIfNotExist)
            {
                return null;
            }

            // Other endpoint rate limits do not yet exist.
            // Therfore we create a new one and return it.
            var attribute = new TwitterEndpointAttribute(queryBaseURL);
            var endpointRateLimit = new EndpointRateLimit
            {
                IsCustomHeaderRateLimit = true
            };

            rateLimits.OtherEndpointRateLimits.Add(attribute, endpointRateLimit);

            return endpointRateLimit;
        }

        private IEndpointRateLimit GetRateLimitFromProperty(PropertyInfo propertyInfo, ICredentialsRateLimits rateLimits)
        {
            var rateLimit = propertyInfo.GetValue(rateLimits, null) as IEndpointRateLimit;
            return rateLimit;
        }

        private bool IsEndpointURLMatchingQueryURL(string queryURL, TwitterEndpointAttribute twitterEndpoint)
        {
            if (twitterEndpoint.IsRegex)
            {
                return Regex.IsMatch(queryURL, twitterEndpoint.EndpointURL);
            }
            else
            {
                return queryURL == twitterEndpoint.EndpointURL;
            }
        }
    }
}