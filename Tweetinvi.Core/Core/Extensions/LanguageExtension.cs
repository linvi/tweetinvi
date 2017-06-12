using System;
using System.Linq;
using System.Reflection;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Extensions
{
    public static class LanguageExtension
    {
        public static string GetLanguageCode(this Language language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(LanguageAttribute));

            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static string GetLanguageCode(this LanguageFilter language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(LanguageAttribute));

            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static string GetLanguageCode(this LanguageFilter? language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)CustomAttributeExtensions.GetCustomAttribute(field, typeof(LanguageAttribute));
            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static Language GetLangFromDescription(string descriptionValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(descriptionValue))
                {
                    descriptionValue = descriptionValue.Substring(0, 2).ToLower();
                }

                var language = typeof(Language).GetFields().First(field => IsValidDescriptionField(descriptionValue, field));
                return (Language)language.GetValue(null);
            }
            catch (Exception)
            {
                return Language.Undefined;
            }
            
        }

        public static Language GetLangFromDescription(int descriptionIndex)
        {
            try
            {
                return (Language)descriptionIndex;
            }
            catch (Exception)
            {
                return Language.Undefined;
            }
        }

        private static bool IsValidDescriptionField(string descriptionValue, FieldInfo field)
        {
            var descriptionAttribute = CustomAttributeExtensions.GetCustomAttribute(field, typeof(LanguageAttribute));

            if (descriptionAttribute == null)
            {
                return false;
            }
            
            var attribute = ((LanguageAttribute) descriptionAttribute);
            if (!attribute.HasMultipleCodes)
            {
                return attribute.Code == descriptionValue;
            }

            return attribute.Codes.Any(x => x == descriptionValue);
        }
    }
}