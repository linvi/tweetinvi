using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
    }

    public class GetUserParameters : CustomRequestParameters, IGetUserParameters
    {
        public GetUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public GetUserParameters(IGetUserParameters source) : base(source)
        {
            if (source == null) {  return;}

            UserIdentifier = source.UserIdentifier;
            IncludeEntities = source.IncludeEntities;
        }

        public IUserIdentifier UserIdentifier { get; set; }
        public bool? IncludeEntities { get; set; }
    }
}
