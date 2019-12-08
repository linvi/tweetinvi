using Tweetinvi.Auth;

namespace Tweetinvi.Parameters.Auth
{
    /// <inheritdoc/>
    public class RequestPinAuthUrlParameters : CustomRequestParameters, IRequestAuthUrlParameters
    {
        public RequestPinAuthUrlParameters()
        {
            CallbackUrl = "oob";
        }

        /// <inheritdoc/>
        public string CallbackUrl { get; set; }
        /// <inheritdoc/>
        public string RequestId { get; set; }
        /// <inheritdoc/>
        public AuthAccessType? AuthAccessType { get; set; }
    }
}