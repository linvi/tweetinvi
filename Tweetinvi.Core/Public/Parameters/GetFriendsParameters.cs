using Tweetinvi.Models;
using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    public interface IGetFriendsParameters : IGetFriendIdsParameters, IGetUsersOptionalParameters
    {
        /// <summary>
        /// Page size when retrieving the users objects from Twitter
        /// </summary>
        int GetUsersPageSize { get; set; }
    }

    public class GetFriendsParameters : GetFriendIdsParameters, IGetFriendsParameters
    {
        public GetFriendsParameters(IUserIdentifier userIdentifier) : base(userIdentifier)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFriendsParameters(string username) : base(username)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFriendsParameters(long userId) : base(userId)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFriendsParameters(IGetFriendsParameters parameters) : base(parameters)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
            
            if (parameters == null) { return; }
            
            SkipStatus = parameters.SkipStatus;
            IncludeEntities = parameters.IncludeEntities;
            GetUsersPageSize = parameters.GetUsersPageSize;
        }

        public bool? IncludeEntities { get; set; }
        public bool? SkipStatus { get; set; }
        public int GetUsersPageSize { get; set; }
    }
}