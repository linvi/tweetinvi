using System;
using Tweetinvi.Auth;

namespace Tweetinvi.Parameters.Auth
{
    /// <inheritdoc/>
    public class RequestUrlAuthUrlParameters : CustomRequestParameters, IRequestAuthUrlParameters
    {
        public RequestUrlAuthUrlParameters(string url)
        {
            CallbackUrl = url;
            RequestId = Guid.NewGuid().ToString();
        }

        public RequestUrlAuthUrlParameters(string url, IAuthenticationTokenProvider authenticationTokenProvider)
        {
            CallbackUrl = url;
            RequestId = authenticationTokenProvider.GenerateAuthTokenId();
            AuthenticationTokenProvider = authenticationTokenProvider;
        }

        public RequestUrlAuthUrlParameters(IRequestAuthUrlParameters parameters)
        {
            CallbackUrl = parameters?.CallbackUrl;
            RequestId = parameters?.RequestId;
            AuthAccessType = parameters?.AuthAccessType;
            AuthenticationTokenProvider = parameters?.AuthenticationTokenProvider;
        }

        /// <inheritdoc/>
        public string CallbackUrl { get; set; }
        /// <inheritdoc/>
        public string RequestId { get; set; }
        /// <inheritdoc/>
        public AuthAccessType? AuthAccessType { get; set; }
        /// <inheritdoc/>
        public IAuthenticationTokenProvider AuthenticationTokenProvider { get; }
    }
}