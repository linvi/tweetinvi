namespace Tweetinvi.Core.QueryGenerators
{
    public interface IFriendshipQueryGenerator
    {
        string GetUserIdsWhoseRetweetsAreMutedQuery();
    }
}