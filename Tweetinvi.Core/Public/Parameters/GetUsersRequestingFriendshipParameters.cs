using Tweetinvi.Parameters.Optionals;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming
    /// </summary>
    public interface IGetUsersRequestingFriendshipParameters : IGetCursorUsersOptionalParameters, IGetUserIdsRequestingFriendshipParameters
    {
        /// <summary>
        /// Page size when retrieving the users objects from Twitter
        /// </summary>
        int GetUsersPageSize { get; set; }
    }
    
    /// <summary>
    /// https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming
    /// </summary>
    public class GetUsersRequestingFriendshipParameters : GetCursorUsersOptionalParameters, IGetUsersRequestingFriendshipParameters
    {
        public GetUsersRequestingFriendshipParameters()
        {
            PageSize = TweetinviConsts.FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ;
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
        }

        public GetUsersRequestingFriendshipParameters(IGetUsersRequestingFriendshipParameters source) : base(source)
        {
            GetUsersPageSize = TweetinviConsts.GET_USERS_MAX_PAGE_SIZE;
            
            if (source == null) { return; }
            
            GetUsersPageSize = source.GetUsersPageSize;
        }

        public int GetUsersPageSize { get; set; }
    }
}