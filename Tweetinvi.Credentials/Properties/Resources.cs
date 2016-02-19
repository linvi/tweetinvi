using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Credentials.Properties
{
    internal static class Resources
    {
        /// <summary>
        ///   Looks up a localized string similar to oob.
        /// </summary>
        public static string OAuth_PINCode_CallbackURL = "oob";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/access_token.
        /// </summary>
        public static string OAuthRequestAccessToken = "https://api.twitter.com/oauth/access_token";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/access_token?x_auth_password ={0}x_auth_password={1}&amp;x_auth_mode=client_auth.
        /// </summary>
        public static string OAuthRequestAccessTokenWithUserCredentials = "https://api.twitter.com/oauth/access_token?x_auth_password ={0}x_auth_password={1}&x_auth_mode=client_auth";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/authorize.
        /// </summary>
        public static string OAuthRequestAuthorize = "https://api.twitter.com/oauth/authorize";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/request_token.
        /// </summary>
        public static string OAuthRequestToken = "https://api.twitter.com/oauth/request_token";

        /// <summary>
        ///   Looks up a localized string similar to Failed to validate oauth signature and token.
        /// </summary>
        public static string OAuthRequestTokenFailedValidation = "Failed to validate oauth signature and token";

        /// <summary>
        ///  Looks up a localized string similar to authorization_id
        /// </summary>
        public static string RedirectRequest_CredsParamId = "authorization_id";

        public static string RedirectRequest_AuthorizationKeyId = "authorization_key";

        public static string RedirectRequest_AuthorizationSecretId = "authorization_secret";

        /// <summary>
        ///   Looks up a localized string similar to (?:\?|%3f|&amp;|%26)oauth_token(?:=|%3d)(?&lt;oauth_token&gt;(?:\w|\-)*)(?:&amp;|%26)oauth_verifier(?:=|%3d)(?&lt;oauth_verifier&gt;(?:\w|\-)*).
        /// </summary>
        public static string OAuthToken_GetVerifierCode_Regex = "(?:\\?|%3f|&|%26)oauth_token(?:=|%3d)(?<oauth_token>(?:\\w|\\-)*)(?:&|%26)oauth_verifier(?:=|%3d)(?<oauth_verifier>(?:\\w|\\-)*)";

        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?&lt;oauth_token&gt;(?:\w|\-)*)&amp;oauth_token_secret=(?&lt;oauth_token_secret&gt;(?:\w)*)&amp;user_id=(?&lt;user_id&gt;(?:\d)*)&amp;screen_name=(?&lt;screen_name&gt;(?:\w)*).
        /// </summary>
        public static string OAuthTokenAccessRegex = "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&user_id=(?<user_id>(?:\\d)*)&screen_name=(?<screen_name>(?:\\w)*)";

        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?&lt;oauth_token&gt;(?:\w|\-)*)&amp;oauth_token_secret=(?&lt;oauth_token_secret&gt;(?:\w)*)&amp;oauth_callback_confirmed=(?&lt;oauth_callback_confirmed&gt;(?:\w)*).
        /// </summary>
        public static string OAuthTokenRequestRegex = "oauth_token=(?<oauth_token>(?:\\w|\\-)*)&oauth_token_secret=(?<oauth_token_secret>(?:\\w)*)&oauth_callback_confirmed=(?<oauth_callback_confirmed>(?:\\w)*)";

        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/application/rate_limit_status.json.
        /// </summary>
        public static string QueryRateLimit = "https://api.twitter.com/1.1/application/rate_limit_status.json";

        public static string GetResourceByName(string resourceName)
        {
            return ResourcesHelper.GetResourceByType(typeof(Resources), resourceName);
        }
    }
}