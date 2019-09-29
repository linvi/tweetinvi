namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-no_retweets-ids
    /// </summary>
    public interface IGetUserIdsWhoseRetweetsAreMutedParameters : ICustomRequestParameters
    {
    }
    
    public class GetUserIdsWhoseRetweetsAreMutedParameters : CustomRequestParameters, IGetUserIdsWhoseRetweetsAreMutedParameters
    {
        public GetUserIdsWhoseRetweetsAreMutedParameters()
        {
        }
        
        public GetUserIdsWhoseRetweetsAreMutedParameters(IGetUserIdsWhoseRetweetsAreMutedParameters source) : base(source)
        {
        }
    }
}