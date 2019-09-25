using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IBlockUserParameters : ICustomRequestParameters
    {
        IUserIdentifier UserIdentifier { get; set; }
    }
    
    public class BlockUserParameters : CustomRequestParameters, IBlockUserParameters
    {
        public BlockUserParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }
        
        public BlockUserParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public BlockUserParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }
        
        public BlockUserParameters(IBlockUserParameters source) : base(source)
        {
            UserIdentifier = source?.UserIdentifier;
        }
        
        public IUserIdentifier UserIdentifier { get; set; }
    }
}