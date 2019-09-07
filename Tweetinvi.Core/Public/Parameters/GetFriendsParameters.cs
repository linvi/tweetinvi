using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetFriendsParameters : IGetFriendIdsParameters
    {
        bool? IncludeEntities { get; set; }
    }

    public class GetFriendsParameters : GetFriendIdsParameters, IGetFriendsParameters
    {
        public GetFriendsParameters(IUserIdentifier userIdentifier) : base(userIdentifier)
        {
        }

        public GetFriendsParameters(string username) : base(username)
        {
        }

        public GetFriendsParameters(long userId) : base(userId)
        {
        }

        public GetFriendsParameters(IGetFriendsParameters parameters) : base(parameters)
        {
        }

        public bool? IncludeEntities { get; set; }
    }
}
