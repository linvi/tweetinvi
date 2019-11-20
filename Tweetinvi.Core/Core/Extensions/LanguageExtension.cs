using System;
using System.Collections.Generic;
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
            var descriptionAttribute = (LanguageAttribute)field.GetCustomAttribute(typeof(LanguageAttribute));

            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static string GetLanguageCode(this LanguageFilter language)
        {
            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)field.GetCustomAttribute(typeof(LanguageAttribute));

            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static string GetLanguageCode(this LanguageFilter? language)
        {
            if (language == null)
            {
                return null;
            }

            var field = language.GetType().GetField(language.ToString());
            var descriptionAttribute = (LanguageAttribute)field.GetCustomAttribute(typeof(LanguageAttribute));
            return descriptionAttribute != null ? descriptionAttribute.Code : language.ToString();
        }

        public static Language GetLangFromDescription(string descriptionValue)
        {
            try
            {
                var language = typeof(Language).GetFields().FirstOrDefault(field => IsValidDescriptionField(descriptionValue, field));

                if (language != null)
                {
                    return (Language)language.GetValue(null);
                }

                if (!string.IsNullOrEmpty(descriptionValue))
                {
                    var lessGenericLanguageCode = descriptionValue.Substring(0, 2).ToLowerInvariant();
                    language = typeof(Language).GetFields().First(field => IsValidDescriptionField(lessGenericLanguageCode, field));
                    return (Language)language.GetValue(null);
                }

                return Language.Undefined;
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
            var descriptionAttribute = field.GetCustomAttribute(typeof(LanguageAttribute));

            if (descriptionAttribute == null)
            {
                return false;
            }

            var attribute = ((LanguageAttribute)descriptionAttribute);
            if (!attribute.HasMultipleCodes)
            {
                return attribute.Code == descriptionValue;
            }

            return attribute.Codes.Any(x => x == descriptionValue);
        }

        private static HashSet<Language> ExistingDisplayLanguages { get; set; }


        public static bool IsADisplayLanguage(this Language? language)
        {
            if (language == null)
            {
                return true;
            }

            return IsADisplayLanguage(language.Value);
        }

        private static readonly object _displayLanguagesLock = new object();

        public static bool IsADisplayLanguage(this Language language)
        {
            lock (_displayLanguagesLock)
            {
                if (ExistingDisplayLanguages == null)
                {
                    var languages = typeof(DisplayLanguages).GetFields().Select(x => x.GetValue(null)).OfType<Language>();
                    ExistingDisplayLanguages = new HashSet<Language>(languages);
                }
            }

            return ExistingDisplayLanguages.Contains(language);
        }
    }
}