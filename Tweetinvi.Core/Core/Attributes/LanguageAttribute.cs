using System;

namespace Tweetinvi.Core.Attributes
{
    /// <summary>
    /// Attribute allowing to link a language with its Twitter code.
    /// </summary>
    public class LanguageAttribute : Attribute
    {
        /// <summary>
        /// Primary language code.
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// All available language codes.
        /// </summary>
        public string[] Languages { get; private set; }

        /// <summary>
        /// Does Twitter represent this language with different codes.
        /// </summary>
        public bool HasMultipleCodes { get; private set; }

        public LanguageAttribute(params string[] languages)
        {
            if (languages == null || languages.Length == 0)
            {
                throw new ArgumentException("You must specify a language code to a Language");
            }

            Language = languages[0];
            Languages = languages;
            HasMultipleCodes = languages.Length > 1;
        }
    }
}