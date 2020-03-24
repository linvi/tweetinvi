using System.Reflection;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Models
{
    public static class TimeZoneFromTwitterExtension
    {
        public static string ToTZinfo(this TimeZoneFromTwitter language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (TimeZoneFromTwitterAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(TimeZoneFromTwitterAttribute));

            return descriptionAttribute != null ? descriptionAttribute.TZinfo : language.ToString();
        }
    }
}