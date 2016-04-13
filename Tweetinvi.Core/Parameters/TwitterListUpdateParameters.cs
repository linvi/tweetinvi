using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/lists/update
    /// </summary>
    public interface ITwitterListUpdateParameters : ICustomRequestParameters
    {
        /// <summary>
        /// New list name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// New list description.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// New privacy mode.
        /// </summary>
        PrivacyMode PrivacyMode { get; set; }
    }

    /// <summary>
    /// https://dev.twitter.com/rest/reference/post/lists/update
    /// </summary>
    public class TwitterListUpdateParameters : CustomRequestParameters, ITwitterListUpdateParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PrivacyMode PrivacyMode { get; set; }
    }
}