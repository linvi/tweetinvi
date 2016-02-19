using System;
using System.Text;

namespace Tweetinvi.Core.Extensions
{
    /// <summary>
    /// Class providing methods to format a string
    /// </summary>
    public class StringFormater
    {
        private const string UNRESERVED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

        /// <summary>
        /// Clean a string so that it can be used in an URL
        /// </summary>
        /// <param name="str">string to clean</param>
        /// <returns>Cleaned string that can be added into an URL</returns>
        public static string UrlEncode(string str)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in str)
            {
                if (UNRESERVED_CHARS.Contains(c.ToString()))
                {
                    result.Append(c);
                }
                else
                {
                    result.Append('%' + string.Format("{0:X2}", (int)c));
                }
            }

            return result.ToString();
        }
    }
}