using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.RateLimit;

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

        public IEnumerable<IEndpointRateLimit> GetTokenRateLimitsFromMethod(Expression<Action> expression, ICredentialsRateLimits rateLimits)
        {
            if (expression == null)
            {
                return null;
            }

            var body = expression.Body;
             
            var methodCallExpression = body as MethodCallExpression;
            if (methodCallExpression != null)
            {
                var method = methodCallExpression.Method;
                var attributes = _attributeHelper.GetAttributes<CustomTwitterEndpointAttribute>(method);
                var tokenAttributes = _attributeHelper.GetAllPropertiesAttributes<ICredentialsRateLimits, TwitterEndpointAttribute>();
                var validKeys = tokenAttributes.Keys.Where(x => attributes.Any(a => a.EndpointURL == x.EndpointURL));
                return validKeys.Select(key => GetRateLimitFromProperty(tokenAttributes[key], rateLimits));
            }
            
            return null;
        }

        public IEndpointRateLimit GetEndpointRateLimitFromQuery(string query, ICredentialsRateLimits rateLimits)
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

            // Other endpoint rate limits do not yet exist.
            // Therfore we create a new one and return it.
            var attribute = new TwitterEndpointAttribute(queryBaseURL);
            var endpointRateLimit = new EndpointRateLimit
            {
                Remaining = -1
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