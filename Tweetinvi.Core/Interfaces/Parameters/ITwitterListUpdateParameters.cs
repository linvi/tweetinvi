using Tweetinvi.Core.Enum;

namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface ITwitterListUpdateParameters : ICustomRequestParameters
    {
        string Name { get; set; }
        string Description { get; set; }
        PrivacyMode PrivacyMode { get; set; }
    }
}