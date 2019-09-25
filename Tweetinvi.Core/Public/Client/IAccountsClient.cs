using Tweetinvi.Iterators;
using Tweetinvi.Parameters;

namespace Tweetinvi
{
    public interface IAccountsClient
    {
        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>Collection of user ids for every user who has a pending request to follow the clients' authenticated user</returns>
        ITwitterIterator<long> GetUserIdsRequestingFriendship();
        
        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>Collection of user ids for every user who has a pending request to follow the clients' authenticated user</returns>
        ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters);
    }
}