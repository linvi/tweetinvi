using System.Net;

namespace Tweetinvi.Core.Extensions
{
    /// <summary>
    /// Extension methods for Exception
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// Provide the exception status number of a WebException
        /// </summary>
        /// <param name="wex">WebException</param>
        /// <returns>Status Number</returns>
        public static int GetWebExceptionStatusNumber(this WebException wex)
        {
            if (wex == null)
            {
                return -1;
            }

            var wexResponse = wex.Response as HttpWebResponse;
            if (wexResponse != null)
            {
                return (int) wexResponse.StatusCode;
            }

            return -1;
        }
    }
}