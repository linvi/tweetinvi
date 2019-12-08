namespace Tweetinvi.Parameters.Auth
{
    /// <inheritdoc/>
    public class RequestUrlAuthUrlParameters : CustomRequestParameters, IRequestAuthUrlParameters
    {
        public RequestUrlAuthUrlParameters(string url)
        {
            CallbackUrl = url;
        }

        public RequestUrlAuthUrlParameters(IRequestAuthUrlParameters parameters)
        {
            CallbackUrl = parameters?.CallbackUrl;
            AuthAccessType = parameters?.AuthAccessType;
        }

        /// <inheritdoc/>
        public string CallbackUrl { get; set; }
        /// <inheritdoc/>
        public AuthAccessType? AuthAccessType { get; set; }
    }
}