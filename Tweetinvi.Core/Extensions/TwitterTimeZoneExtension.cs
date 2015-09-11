using System;
using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Extensions
{
    public static class TwitterTimeZoneExtension
    {
        public static string GetDisplayableValue(this TwitterTimeZone twitterTimeZone)
        {
            var twitterTimeZoneAttribute = GetAttribute(twitterTimeZone);
            return twitterTimeZoneAttribute != null ? twitterTimeZoneAttribute.DisplayValue : twitterTimeZone.ToString();
        }

        public static string GetTZinfo(this TwitterTimeZone twitterTimeZone)
        {
            var twitterTimeZoneAttribute = GetAttribute(twitterTimeZone);
            return twitterTimeZoneAttribute != null ? twitterTimeZoneAttribute.TZinfo : null;
        }

        private static TwitterTimeZoneAttribute GetAttribute(TwitterTimeZone twitterTimeZone)
        {
            var field = twitterTimeZone.GetType().GetField(twitterTimeZone.ToString());
            return (TwitterTimeZoneAttribute)Attribute.GetCustomAttribute(field, typeof(TwitterTimeZoneAttribute));
        }
    }
}