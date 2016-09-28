using System;

namespace Tweetinvi.Core.Attributes
{
    /// <summary>
    /// Attribute allowing to link a language with its Twitter code.
    /// </summary>
    public class LanguageAttribute : Attribute
    {
        /// <summary>
        /// Name of the language in English.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Language names also reference by the code.
        /// </summary>
        public string[] Names { get; private set; }

        /// <summary>
        /// Primary language code.
        /// </summary>
        public string Code { get; private set; }

        /// <summary>
        /// All available language codes.
        /// </summary>
        public string[] Codes { get; private set; }

        /// <summary>
        /// Does Twitter represent this language with different codes.
        /// </summary>
        public bool HasMultipleCodes { get; private set; }

        public LanguageAttribute(string name, params string[] codes)
        {
            // Validation
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.Trim() == "")
            {
                throw new ArgumentException("Name must not be whitespace.", nameof(name));
            }

            if (codes == null || codes.Length == 0)
            {
                throw new ArgumentException("You must specify a language code.", nameof(codes));
            }

            Name = name;
            Names = new[] { name };
            Code = codes[0];
            Codes = codes;
            HasMultipleCodes = codes.Length > 1;
        }

        public LanguageAttribute(string[] names, params string[] codes)
        {
            // Validation
            if (names == null || names.Length == 0)
            {
                throw new ArgumentException("You must specify a language name.", nameof(names));
            }

            if (codes == null || codes.Length == 0)
            {
                throw new ArgumentException("You must specify a language code.", nameof(codes));
            }

            Name = names[0];
            Names = names;
            Code = codes[0];
            Codes = codes;
            HasMultipleCodes = codes.Length > 1;
        }
    }
}