using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Parameters;

namespace Tweetinvi.Core.Parameters
{
    public class TwitterListUpdateParameters : CustomRequestParameters, ITwitterListUpdateParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public PrivacyMode PrivacyMode { get; set; }
    }
}