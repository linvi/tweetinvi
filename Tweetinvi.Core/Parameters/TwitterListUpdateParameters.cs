using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Parameters
{
    public interface ITwitterListUpdateParameters : ICustomRequestParameters
    {
        string Name { get; set; }
        string Description { get; set; }
        PrivacyMode PrivacyMode { get; set; }
    }

    public class TwitterListUpdateParameters : CustomRequestParameters, ITwitterListUpdateParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PrivacyMode PrivacyMode { get; set; }
    }
}