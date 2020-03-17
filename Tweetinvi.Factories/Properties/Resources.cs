using Tweetinvi.Core.Helpers;

// ReSharper disable InconsistentNaming
namespace Tweetinvi.Factories.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/create.json?query={0}.
        /// </summary>
        public static string SavedSearch_Create = "https://api.twitter.com/1.1/saved_searches/create.json?query={0}";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/saved_searches/show/{0}.json.
        /// </summary>
        public static string SavedSearch_Get = "https://api.twitter.com/1.1/saved_searches/show/{0}.json";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}