using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Web
{
    /// <summary>
    /// Custom HttpClientHandler.
    /// </summary>
    public interface ITwitterClientHandler
    {
        /// <summary>
        /// Contains all the information required for the HttpClient to create and execute the request.
        /// </summary>
        ITwitterQuery TwitterQuery { get; set; }
    }
}
