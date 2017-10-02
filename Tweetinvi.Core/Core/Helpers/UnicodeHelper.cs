using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Core.Core.Helpers
{
    public static class UnicodeHelper
    {
        public static IEnumerable<string> GetUnicodeCleanupGraphemeEnumerator(string str)
        {
            var enumerator = StringInfo.GetTextElementEnumerator(str);

            while (enumerator.MoveNext())
            {
                var grapheme = enumerator.GetTextElement();

                
                UnicodeCategory characterChategory = CharUnicodeInfo.GetUnicodeCategory(grapheme, 0);

                // Other potential categories to consider 
                // cat == UnicodeCategory.NonSpacingMark || cat == UnicodeCategory.SpacingCombiningMark || cat == UnicodeCategory.EnclosingMark

                if (characterChategory == UnicodeCategory.ModifierSymbol)
                {
                    continue;
                }

                yield return grapheme;
            }
        }

        public static string UnicodeCleanup(string str)
        {
            var cleanStringBuilder = new StringBuilder();

            UnicodeHelper.GetUnicodeCleanupGraphemeEnumerator(str).ForEach(c => cleanStringBuilder.Append(c));

            return cleanStringBuilder.ToString();
        }

        public static string UnicodeSubstring(string str, int startIndex, int length)
        {
            if (str == null)
            {
                return null;
            }

            var graphemes = GetUnicodeCleanupGraphemeEnumerator(str).Skip(startIndex).Take(length);
            return string.Join("", graphemes);
        }

        private const char HIGH_SURROGATE_START = '\uD800';
        private const char HIGH_SURROGATE_END = '\uDBFF';
        private const char LOW_SURROGATE_START = '\uDC00';
        private const char LOW_SURROGATE_END = '\uDFFF';

        /// <summary>
        /// Get the UTF32 length of a string
        /// </summary>
        public static int UTF32Length(this string str)
        {
            var length = 0;

            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] >= HIGH_SURROGATE_START && str[i] <= HIGH_SURROGATE_END &&
                    str[i + 1] >= LOW_SURROGATE_START && str[i + 1] <= LOW_SURROGATE_END)
                {
                    i++;
                }

                ++length;
            }

            return length;
        }
    }
}