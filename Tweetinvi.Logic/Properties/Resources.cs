using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Logic.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to Not Modified - There was no new data to return..
        /// </summary>
        public static string ExceptionDescription_304 = "Not Modified - There was no new data to return.";

        /// <summary>
        ///   Looks up a localized string similar to Bad Request - The request was invalid or cannot be otherwise served. An accompanying error message will explain further. In API v1.1, requests without authentication are considered invalid and will yield this response..
        /// </summary>
        public static string ExceptionDescription_400 = "Bad Request - The request was invalid or cannot be otherwise served. An accompanying error message will explain further. In API v1.1, requests without authentication are considered invalid and will yield this response.";

        /// <summary>
        ///   Looks up a localized string similar to Unauthorized -  Authentication credentials were missing or incorrect..
        /// </summary>
        public static string ExceptionDescription_401 = "Unauthorized -  Authentication credentials were missing or incorrect.";

        /// <summary>
        ///   Looks up a localized string similar to Forbidden - The request is understood, but it has been refused or access is not allowed. An accompanying error message will explain why. This code is used when requests are being denied due to update limits..
        /// </summary>
        public static string ExceptionDescription_403 = "Forbidden - The request is understood, but it has been refused or access is not allowed. An accompanying error message will explain why. This code is used when requests are being denied due to update limits.";

        /// <summary>
        ///   Looks up a localized string similar to Not Found - The URI requested is invalid or the resource requested, such as a user, does not exists. Also returned when the requested format is not supported by the requested method..
        /// </summary>
        public static string ExceptionDescription_404 = "Not Found - The URI requested is invalid or the resource requested, such as a user, does not exists. Also returned when the requested format is not supported by the requested method.";

        /// <summary>
        ///   Looks up a localized string similar to Not Acceptable - Returned by the Search API when an invalid format is specified in the request..
        /// </summary>
        public static string ExceptionDescription_406 = "Not Acceptable - Returned by the Search API when an invalid format is specified in the request.";

        /// <summary>
        ///   Looks up a localized string similar to Gone - This resource is gone. Used to indicate that an API endpoint has been turned off. For example: &quot;The Twitter REST API v1 will soon stop functioning. Please migrate to API v1.1..
        /// </summary>
        public static string ExceptionDescription_410 = "Gone - This resource is gone. Used to indicate that an API endpoint has been turned off. For example: \"The Twitter REST API v1 will soon stop functioning. Please migrate to API v1.1.\"";

        /// <summary>
        ///   Looks up a localized string similar to Enhance Your Calm
        /// </summary>
        public static string ExceptionDescription_420 = "Enhance Your Calm";

        /// <summary>
        ///   Looks up a localized string similar to Unprocessable Entity - Returned when an image uploaded to POST account/update_profile_banner is unable to be processed..
        /// </summary>
        public static string ExceptionDescription_422 = "Unprocessable Entity - Returned when an image uploaded to POST account/update_profile_banner is unable to be processed.";

        /// <summary>
        ///   Looks up a localized string similar to Too Many Requests - Returned in API v1.1 when a request cannot be served due to the application&apos;s rate limit having been exhausted for the resource. See Rate Limiting in API v1.1..
        /// </summary>
        public static string ExceptionDescription_429 = "Too Many Requests - Returned in API v1.1 when a request cannot be served due to the application's rate limit having been exhausted for the resource. See Rate Limiting in API v1.1.";

        /// <summary>
        ///   Looks up a localized string similar to Internal Server Error - Something is broken. Please post to the group so the Twitter team can investigate..
        /// </summary>
        public static string ExceptionDescription_500 = "Internal Server Error - Something is broken. Please post to the group so the Twitter team can investigate.";

        /// <summary>
        ///   Looks up a localized string similar to Bad Gateway - Twitter is down or being upgraded..
        /// </summary>
        public static string ExceptionDescription_502 = "Bad Gateway - Twitter is down or being upgraded.";

        /// <summary>
        ///   Looks up a localized string similar to Service Unavailable - The Twitter servers are up, but overloaded with requests. Try again later..
        /// </summary>
        public static string ExceptionDescription_503 = "Service Unavailable - The Twitter servers are up, but overloaded with requests. Try again later.";

        /// <summary>
        ///   Looks up a localized string similar to Gateway timeout - The Twitter servers are up, but the request couldn&apos;t be serviced due to some failure within our stack. Try again later..
        /// </summary>
        public static string ExceptionDescription_504 = "Gateway timeout - The Twitter servers are up, but the request couldn't be serviced due to some failure within our stack. Try again later.";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}