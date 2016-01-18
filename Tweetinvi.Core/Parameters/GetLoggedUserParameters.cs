using Tweetinvi.Core.Credentials;

namespace Tweetinvi.Core.Parameters
{
    public interface IGetLoggedUserParameters : ICustomRequestParameters
    {
        bool IncludeEntities { get; set; }
        bool IncludeEmail { get; set; }
        bool SkipStatus { get; set; }
    }

    public class GetLoggedUserParameters : CustomRequestParameters, IGetLoggedUserParameters
    {
        public GetLoggedUserParameters()
        {
            IncludeEntities = true;
            IncludeEmail = true;
            SkipStatus = true;
        }

        public bool IncludeEntities { get; set; }
        public bool IncludeEmail { get; set; }
        public bool SkipStatus { get; set; }
    }
}