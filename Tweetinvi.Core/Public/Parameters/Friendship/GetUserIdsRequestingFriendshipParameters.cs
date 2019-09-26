namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming
    /// </summary>
    /// <inheritdoc />
    public interface IGetUserIdsRequestingFriendshipParameters : ICursorQueryParameters
    {
    }

    /// <inheritdoc />
    public class GetUserIdsRequestingFriendshipParameters : CursorQueryParameters, IGetUserIdsRequestingFriendshipParameters
    {
        public GetUserIdsRequestingFriendshipParameters()
        {
            PageSize = TweetinviConsts.FRIENDSHIPS_INCOMING_IDS_MAX_PER_REQ;
        }

        public GetUserIdsRequestingFriendshipParameters(IGetUserIdsRequestingFriendshipParameters parameters) : base(parameters)
        {
        }
    }
}