using Tweetinvi.Auth;

namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/request_token
    /// </summary>
    public interface IRequestAuthUrlParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The url Twitter will redirect to after attempting to authenticate the user
        /// If not specified the user will not be redirected but will obtain a code to use to validate the authentications.
        /// </summary>
        string CallbackUrl { get; set; }

        /// <summary>
        /// If not removed, Tweetinvi will use this authorization id to automatically
        /// retrieve the AuthenticationContext after the redirection.
        /// <para>Set to null if you do not wish this behaviour</para>
        /// <para>This parameters is not a Twitter parameter and is only consumed by Tweetinvi</para>
        /// </summary>
        string RequestId { get; set; }

        /// <summary>
        /// Overrides the access level an application requests to a users account. Supported values are read or write.
        /// This parameter is intended to allow a developer to register a read/write application but also
        /// request read only access when appropriate.
        /// </summary>
        AuthAccessType? AuthAccessType { get; set; }

        /// <summary>
        /// Defines how Tweetinvi will search for AuthToken after url callback.
        /// If null you will be responsible for storing such information and retrieving it from the callback.
        /// </summary>
        IAuthenticationTokenProvider AuthenticationTokenProvider { get; }
    }
}