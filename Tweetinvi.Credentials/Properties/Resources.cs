using Tweetinvi.Core.Helpers;

// ReSharper disable InconsistentNaming
namespace Tweetinvi.Credentials.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/access_token.
        /// </summary>
        public static string OAuthRequestAccessToken = "https://api.twitter.com/oauth/access_token";

        /// <summary>
        ///   Looks up a localized string similar to (?:\?|%3f|&amp;|%26)oauth_token(?:=|%3d)(?&lt;oauth_token&gt;(?:\w|\-)*)(?:&amp;|%26)oauth_verifier(?:=|%3d)(?&lt;oauth_verifier&gt;(?:\w|\-)*).
        /// </summary>
        public static string OAuthToken_GetVerifierCode_Regex = "(?:\\?|%3f|&|%26)oauth_token(?:=|%3d)(?<oauth_token>(?:\\w|\\-)*)(?:&|%26)oauth_verifier(?:=|%3d)(?<oauth_verifier>(?:\\w|\\-)*)";

        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?&lt;oauth_token&gt;(?:\w|\-)*)&amp;oauth_token_secret=(?&lt;oauth_token_secret&gt;(?:\w)*)&amp;user_id=(?&lt;user_id&gt;(?:\d)*)&amp;screen_name=(?&lt;screen_name&gt;(?:\w)*).
        /// </summary>
        public static string OAuthTokenAccessRegex = "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}