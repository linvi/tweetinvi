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

        public bool IsQueryAssociatedWithEndpointRateLimit(string query, ICredentialsRateLimits rateLimits)
        {
            return GetEndpointRateLimitFromQuery(query, rateLimits) != null;
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

            if (matchingAttribute == null)
            {
                return null;
            }

            var matchingProperty = tokenAttributes[matchingAttribute];
            return GetRateLimitFromProperty(matchingProperty, rateLimits);
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