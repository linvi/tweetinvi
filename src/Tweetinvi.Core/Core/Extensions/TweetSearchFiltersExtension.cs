using Tweetinvi.Core.Attributes;
using System.Reflection;
using Tweetinvi.Parameters.Enum;

namespace Tweetinvi.Core.Extensions
{
    public static class TweetSearchFiltersExtension
    {
        public static string GetQueryFilterName(this TweetSearchFilters tweetSearchFilters)
        {
            var field = tweetSearchFilters.GetType().GetField(tweetSearchFilters.ToString());
            var descriptionAttribute = (TweetSearchFilterAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(TweetSearchFilterAttribute));

            return descriptionAttribute != null ? descriptionAttribute.FilterName : tweetSearchFilters.ToString().ToLowerInvariant();
        }
    }
}