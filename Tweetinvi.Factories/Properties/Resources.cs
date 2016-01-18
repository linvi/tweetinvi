using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Factories.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/create.json?name={0}&amp;mode={1}.
        /// </summary>
        public static string List_Create = "https://api.twitter.com/1.1/lists/create.json?name={0}&mode={1}";

        /// <summary>
        ///   Looks up a localized string similar to &amp;description={0}.
        /// </summary>
        public static string List_Create_DescriptionParameter = "&description={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/lists/show.json?{0}.
        /// </summary>
        public static string List_GetExistingList = "https://api.twitter.com/1.1/lists/show.json?{0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/direct_messages/show.json?id={0}.
        /// </summary>
        public static string Message_GetMessageFromId = "https://api.twitter.com/1.1/direct_messages/show.json?id={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/create.json?query={0}.
        /// </summary>
        public static string SavedSearch_Create = "https://api.twitter.com/1.1/saved_searches/create.json?query={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/show/{0}.json.
        /// </summary>
        public static string SavedSearch_Get = "https://api.twitter.com/1.1/saved_searches/show/{0}.json";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/show.json?user_id={0}.
        /// </summary>
        public static string User_GetUserFromId = "https://api.twitter.com/1.1/users/show.json?user_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/show.json?screen_name={0}.
        /// </summary>
        public static string User_GetUserFromName = "https://api.twitter.com/1.1/users/show.json?screen_name={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/lookup.json?user_id={0}.
        /// </summary>
        public static string User_GetUsersFromIds = "https://api.twitter.com/1.1/users/lookup.json?user_id={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/users/lookup.json?screen_name={0}.
        /// </summary>
        public static string User_GetUsersFromNames = "https://api.twitter.com/1.1/users/lookup.json?screen_name={0}";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}