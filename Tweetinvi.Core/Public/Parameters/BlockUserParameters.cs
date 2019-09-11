using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IBlockUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
        bool? SkipStatus { get; set; }
    }
    
    public class BlockUserParameters : CustomRequestParameters, IBlockUserParameters
    {
        public BlockUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
            SkipStatus = true;
        }
        
        public BlockUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public BlockUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }
        
        public BlockUserParameters(IBlockUserParameters source) : base(source)
        {
            UserIdentifier = source.UserIdentifier;
            IncludeEntities = source.IncludeEntities;
            SkipStatus = source.SkipStatus;
        }
        
        public IUserIdentifier UserIdentifier { get; set; }
        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
    }
}