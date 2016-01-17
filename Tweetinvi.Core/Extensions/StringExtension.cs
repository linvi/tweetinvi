using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Core.Extensions
{
    /// <summary>
    /// Extension methods on string classes
    /// </summary>
    public static class StringExtension
    {
        private static Regex _linkParser;
        private static readonly HttpUtility _httpUtility = new HttpUtility();

        private const string TWITTER_URL_REGEX =
            @"(?<=^|\s+)" +                                            // URL can be prefixed by space or start of line
            @"\b(?<start>http(?<isSecured>s?)://(?:www\.)?|www\.|)" +  // Start of an url
            @"(?!www\.)" +                                             // The first keyword cannot be www.
            @"(?<firstPathElement>\w+\.)" +                            // first keyword required
            @"(?<secondPathElement>\w{2,})\w*?" +                      // second keyword required
            @"(?<multiplePathElements>(?:\.\w{2,})*)" +                // potential sub-sites
            @"?\.{0}" +                                                // . is forbidden at this stage
            @"(?<specialChar>[/?])?" +                                 // is there a character specifying the url will be extended
            @"(?(specialChar)" +                                       // has a specialChar been detected
            @"(?:" +                                                   // if so
            @"(?:(?:\w|\d)+)" +                                        // Get all the letters
            @"(?:(?:\p{P}|=)+)" +                                      // Followed by at least 1 or multiple punctuation (twitter behavior)
            @")*(?:(?:\w|\d)+))" +                                     // And the end should be a literal char
            @"(?<lastChar>[/?])?";                                     // Or a '/'                               


        // FOR COPY WITHIN REGEX EDITOR - KEEP Sync!
        // (?<=^|\s+)
        // \b(?<start>http(?<isSecured>s?)://(?:www\.)?|www\.|)
        // (?!www\.)
        // (?<firstPathElement>\w+\.)
        // (?<secondPathElement>\w{2,})\w*?
        // (?<multiplePathElements>(?:\.\w{2,})*)
        // ?\.{0}
        // (?<specialChar>[/?])?
        // (?(specialChar)
        // (?:
        // (?:(?:\w|\d)+)
        // (?:(?:\p{P}|=)+)
        // )*(?:(?:\w|\d)+))
        // (?<lastChar>[/?])?     


        // Create on demand
        private static Regex LinkParser
        {
            get
            {
                if (_linkParser == null)
                {
                    _linkParser = new Regex(TWITTER_URL_REGEX, RegexOptions.IgnoreCase);
                }

                return _linkParser;
            }
        }

        /// <summary>
        /// Calculate the length of a string using Twitter algorithm
        /// </summary>
        /// <returns>Size of the current Tweet</returns>
        public static int TweetLength(this string tweet, bool willBePublishedWithMedia = false)
        {
            if (tweet == null)
            {
                return 0;
            }

            int length = tweet.UTF32Length();

            foreach (Match link in LinkParser.Matches(tweet))
            {
                // If an url ends with . and 2 followed chars twitter does not
                // consider it as an URL
                if (link.Groups["start"].Value == string.Empty &&
                    link.Groups["multiplePathElements"].Value == string.Empty &&
                    link.Groups["secondPathElement"].Value.Length < 2 &&
                    link.Groups["specialChar"].Value == string.Empty &&
                    link.Groups["lastChar"].Value != "/")
                {
                    continue;
                }

                length = length - link.Value.Length + 23;
            }

            if (willBePublishedWithMedia)
            {
                length += TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            return length;
        }

        private const char HIGH_SURROGATE_START = '\uD800';
        private const char HIGH_SURROGATE_END = '\uDBFF';
        private const char LOW_SURROGATE_START = '\uDC00';
        private const char LOW_SURROGATE_END = '\uDFFF';

        /// <summary>
        /// Get the UTF32 length of a string
        /// </summary>
        public static int UTF32Length(this string s)
        {
            var length = 0;

            for (int i = 0; i < s.Length; ++i)
            {
                if (s[i] >= HIGH_SURROGATE_START && s[i] <= HIGH_SURROGATE_END &&
                    s[i + 1] >= LOW_SURROGATE_START && s[i + 1] <= LOW_SURROGATE_END)
                {
                    i++;
                }

                ++length;
            }

            return length;
        }

        /// <summary>
        /// Clean a string so that it can be used in a URL and
        /// sent to Twitter
        /// </summary>
        /// <returns>Cleaned string</returns>
        public static string CleanStringForMySQL(this string s)
        {
            return s != null ? (s.HTMLDecode().MySQLClean().ReplaceNonPrintableCharacters('\\')) : null;
        }

        public static bool IsMatchingJsonFormat(this string json)
        {
            return !string.IsNullOrEmpty(json) && json.Length >= 2 && ((json[0] == '{' && json[json.Length - 1] == '}') || (json[0] == '[' && json[json.Length - 1] == ']'));
        }

        /// <summary>
        /// Decode a string formatted to be used within a url
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string HTMLDecode(this string s)
        {
            return _httpUtility.HtmlDecode(s);
        }

        public static string GetURLParameter(this string url, string parameterName)
        {
            if (url == null)
            {
                return null;
            }

            Match urlInformation = Regex.Match(url, parameterName + "=(?<parameter_value>[^&]*)");
            return urlInformation.Groups["parameter_value"].Value;
        }

        /// <summary>
        /// Give the ability to replace NonPrintableCharacters to another character
        /// This is very useful for streaming as we receive Tweets from around the world
        /// </summary>
        /// <param name="s">String to be updated</param>
        /// <param name="replaceWith">Character to replace by</param>
        /// <returns>String without any of the special characters</returns>
        public static string ReplaceNonPrintableCharacters(this string s, char replaceWith)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] > 51135)
                {
                    result.Append(replaceWith);
                }

                result.Append(s[i]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Clean the string so that it can be used as 
        /// a parameter of a MySql query
        /// </summary>
        /// <param name="s">String to be updated</param>
        /// <returns>String that can be used within MySql</returns>
        public static string MySQLClean(this string s)
        {
            if (s == null)
            {
                return null;
            }

            StringBuilder result = new StringBuilder(s);
            return Regex.Replace(result.Replace("\\", "\\\\").ToString(), "['′ˈ]", "\\'");
        }

        /// <summary>
        /// Creates a groupName by replacing invalidCharacters with unique groupName
        /// </summary>
        public static string CleanForRegexGroupName(this string groupName)
        {
            string res = Regex.Replace(groupName, @"^[^a-zA-Z]", match => String.Format("special{0}", (int)match.Value[0]));
            return Regex.Replace(res, @"[^a-zA-Z0-9]", match => String.Format("special{0}", (int)match.Value[0]));
        }

        /// <summary>
        /// Clean a keyword that you want to search with a regex
        /// </summary>
        public static string CleanForRegex(this string regexKeyword)
        {
            return Regex.Replace(regexKeyword, @"[.^$*+?()[{\|#]", match => String.Format(@"\{0}", match));
        }

        /// <summary>
        /// Create a filtering Regex for all the expected keywords and creates a Group that you can inspect
        /// to see if the specific keyword has been matched
        /// </summary>
        public static string RegexFiltering(string[] keywords)
        {
            StringBuilder patternBuilder = new StringBuilder();
            foreach (var keywordPattern in keywords)
            {
                patternBuilder.Append(String.Format(@"(?=.*(?<{0}>(?:^|\s+){1}(?:\s+|$)))?",
                   CleanForRegexGroupName(keywordPattern), CleanForRegex(keywordPattern)));
            }

            // Check the first group to analyze the result of the Regex : 
            // MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.IgnoreCase);
            // GroupCollection groups = matches[0].Groups;

            return patternBuilder.ToString();
        }

        public static char Last(this string s)
        {
            if (s.Length == 0)
            {
                return default(char);
            }

            return s[s.Length - 1];
        }

        public static void AddParameterToQuery(this StringBuilder queryBuilder, string parameterName, string parameterValue)
        {
            if (string.IsNullOrEmpty(parameterValue))
            {
                return;
            }

            var query = queryBuilder.ToString();

            if (query.Contains("?") && query[query.Length - 1] != '?' && query[query.Length - 1] != '&')
            {
                queryBuilder.Append("&");
            }

            if (!query.Contains("?"))
            {
                queryBuilder.Append("?");
            }

            queryBuilder.Append(string.Format("{0}={1}", parameterName, parameterValue));
        }

        public static void AddParameterToQuery<T>(this StringBuilder queryBuilder, string parameterName, T parameterValue)
        {
            if (parameterValue == null)
            {
                return;
            }

            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
            {
                var stringValue = parameterValue.ToString();
                var typeofValue = parameterValue.GetType().ToString();

                if (stringValue == typeofValue)
                {
                    return;
                }

                var doubleValue = parameterValue as double?;
                if (doubleValue != null)
                {
                    stringValue = doubleValue.Value.ToString(CultureInfo.InvariantCulture);
                }

                AddParameterToQuery(queryBuilder, parameterName, stringValue.ToLowerInvariant());
            }
            else
            {
                var stringValue = parameterValue.ToString();

                if (parameterValue is double)
                {
                    stringValue = ((double) (object) parameterValue).ToString(CultureInfo.InvariantCulture);
                }

                AddParameterToQuery(queryBuilder, parameterName, stringValue);
            }
        }

        public static string AddParameterToQuery(this string query, string parameterName, string parameterValue)
        {
            if (query.Contains("?") && query[query.Length - 1] != '?' && query[query.Length - 1] != '&')
            {
                query += "&";
            }

            if (!query.Contains("?"))
            {
                query += "?";
            }

            query += string.Format("{0}={1}", parameterName, parameterValue);
            return query;
        }
    }
}