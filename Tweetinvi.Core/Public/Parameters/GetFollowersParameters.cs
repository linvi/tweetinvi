using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetFollowersParameters : IGetFollowerIdsParameters
    {
        bool? IncludeEntities { get; set; }
    }

    public class GetFollowersParameters : GetFollowerIdsParameters, IGetFollowersParameters
    {
        public GetFollowersParameters(IUserIdentifier userIdentifier) : base(userIdentifier)
        {
        }

        public GetFollowersParameters(string username) : base(username)
        {
        }

        public GetFollowersParameters(long userId) : base(userId)
        {
        }

        public GetFollowersParameters(IGetFollowersParameters parameters) : base(parameters)
        {
        }

        public bool? IncludeEntities { get; set; }
    }
}
