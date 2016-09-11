using System;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Parameters;
using System.Reflection;

namespace Tweetinvi.Core.Extensions
{
    public static class TweetSearchFiltersExtension
    {
        public static string GetQueryFilterName(this TweetSearchFilters tweetSearchFilters)
        {
#if NET_CORE
            var field = tweetSearchFilters.GetType().GetField(tweetSearchFilters.ToString());
            var descriptionAttribute = (TweetSearchFilterAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(TweetSearchFilterAttribute));
#else
            var field = tweetSearchFilters.GetType().GetField(tweetSearchFilters.ToString());
            var descriptionAttribute = (TweetSearchFilterAttribute)Attribute.GetCustomAttribute(field, typeof(TweetSearchFilterAttribute));
#endif

            return descriptionAttribute != null ? descriptionAttribute.FilterName : tweetSearchFilters.ToString().ToLowerInvariant();
        }
    }
}