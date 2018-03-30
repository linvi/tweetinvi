using System;
using System.Globalization;
using System.Text;

namespace Tweetinvi.Core.Core.Helpers
{
    public static class UnicodeHelper
    {
        // Some versions of the .Net framework have this functionality built in
        // See StringInfo.SubstringByTextElements()
        public static string SubstringByTextElements(string str, int startingTextElement)
        {
            return SubstringByTextElements(str, startingTextElement, str?.Length ?? 0);
        }

        public static string SubstringByTextElements(string str, int startingTextElement, int lengthInTextElements)
        {
            if (str == null)
            {
                return null;
            }

            var textElements = StringInfo.GetTextElementEnumerator(str);
            var substr = new StringBuilder();
            var substrElementCount = 0;
            var i = 0;

            while (textElements.MoveNext())
            {
                if (i >= startingTextElement && substrElementCount < lengthInTextElements)
                {
                    substr.Append(textElements.GetTextElement());
                    substrElementCount++;
                }
                
                ++i;
            }

            return substr.ToString();
        }

        public static bool AnyUnicode(string str)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if (char.IsSurrogatePair(str, i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get the UTF32 length of a string
        /// </summary>
        public static int UTF32Length(this string str)
        {
            return new StringInfo(str).LengthInTextElements;
        }
    }
}