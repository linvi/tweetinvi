using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    public interface IGetFollowersParameters : IGetFollowerIdsParameters
    {
        /// <summary>
        /// Page size when retrieving the user ids from Twitter
        /// </summary>
        new int PageSize { get; set; } 
        
        /// <summary>
        /// Include user entities.
        /// </summary>
        bool? IncludeEntities { get; set; }
        
        /// <summary>
        /// Page size when retrieving the users objects from Twitter
        /// </summary>
        int GetUsersPageSize { get; set; }
    }

    public class GetFollowersParameters : GetFollowerIdsParameters, IGetFollowersParameters
    {
        public GetFollowersParameters(IUserIdentifier userIdentifier) : base(userIdentifier)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFollowersParameters(string username) : base(username)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFollowersParameters(long userId) : base(userId)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetFollowersParameters(IGetFollowersParameters parameters) : base(parameters)
        {
        }

        public bool? IncludeEntities { get; set; }
        public int GetUsersPageSize { get; set; }
    }
}
