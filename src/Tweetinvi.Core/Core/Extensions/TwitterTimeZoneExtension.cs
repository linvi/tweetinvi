using Tweetinvi.Core.Attributes;
using Tweetinvi.Models;
using System.Reflection;

namespace Tweetinvi.Core.Extensions
{
    public static class TwitterTimeZoneExtension
    {
        public static string GetDisplayableValue(this TimeZoneFromTwitter timeZoneFromTwitter)
        {
            var twitterTimeZoneAttribute = GetAttribute(timeZoneFromTwitter);
            return twitterTimeZoneAttribute != null ? twitterTimeZoneAttribute.DisplayValue : timeZoneFromTwitter.ToString();
        }

        public static string GetTZinfo(this TimeZoneFromTwitter timeZoneFromTwitter)
        {
            var twitterTimeZoneAttribute = GetAttribute(timeZoneFromTwitter);
            return twitterTimeZoneAttribute != null ? twitterTimeZoneAttribute.TZinfo : null;
        }

        private static TimeZoneFromTwitterAttribute GetAttribute(TimeZoneFromTwitter timeZoneFromTwitter)
        {
            var field = timeZoneFromTwitter.GetType().GetField(timeZoneFromTwitter.ToString());
            return (TimeZoneFromTwitterAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(TimeZoneFromTwitterAttribute));
        }
    }
}