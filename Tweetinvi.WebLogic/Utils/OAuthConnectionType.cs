using System.Runtime.Serialization;

namespace Tweetinvi.WebLogic.Utils
{
    /// <summary>
    /// Provide different solution to connect to an url
    /// </summary>
    public enum OAuthConnectionType
    {
        AuthorizationHeaders,
        BaseHtmlString
    }
}