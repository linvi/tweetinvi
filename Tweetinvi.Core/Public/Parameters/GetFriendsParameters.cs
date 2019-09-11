using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetFriendsParameters : IGetFriendIdsParameters
    {
        /// <summary>
        /// Page size when retrieving the user ids from Twitter
        /// </summary>
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
        
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
        }

        public bool? IncludeEntities { get; set; }
        public int GetUsersPageSize { get; set; }
    }
}
