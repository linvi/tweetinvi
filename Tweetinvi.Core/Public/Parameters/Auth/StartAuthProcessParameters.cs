using System;

namespace Tweetinvi.Parameters.Auth
{
    /// <summary>
    /// Type of permissions requested when authenticating a user
    /// </summary>
    public enum AuthAccessType
    {
        ReadWrite = 0,
        Read = 1,
    }

    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/basics/authentication/api-reference/request_token
    /// </summary>
    public interface IStartAuthProcessParameters : ICustomRequestParameters
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
        string AuthorizationId { get; set; }

        /// <summary>
        /// Overrides the access level an application requests to a users account. Supported values are read or write.
        /// This parameter is intended to allow a developer to register a read/write application but also
        /// request read only access when appropriate.
        /// </summary>
        AuthAccessType? AuthAccessType { get; set; }
    }

    /// <inheritdoc/>
    public class StartPinAuthProcessParameters : CustomRequestParameters, IStartAuthProcessParameters
    {
        public StartPinAuthProcessParameters()
        {
            CallbackUrl = "oob";
        }

        /// <inheritdoc/>
        public string CallbackUrl { get; set; }
        /// <inheritdoc/>
        public string AuthorizationId { get; set; }
        /// <inheritdoc/>
        public AuthAccessType? AuthAccessType { get; set; }
    }

    /// <inheritdoc/>
    public class StartUrlAuthProcessParameters : CustomRequestParameters, IStartAuthProcessParameters
    {
        public StartUrlAuthProcessParameters(string url)
        {
            CallbackUrl = url;
            AuthorizationId = Guid.NewGuid().ToString();
        }

        public StartUrlAuthProcessParameters(IStartAuthProcessParameters parameters)
        {
            CallbackUrl = parameters?.CallbackUrl;
            AuthorizationId = parameters?.AuthorizationId;
            AuthAccessType = parameters?.AuthAccessType;
        }

        /// <inheritdoc/>
        public string CallbackUrl { get; set; }
        /// <inheritdoc/>
        public string AuthorizationId { get; set; }
        /// <inheritdoc/>
        public AuthAccessType? AuthAccessType { get; set; }
    }
}