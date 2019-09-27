using Tweetinvi.Parameters;
using Tweetinvi.Public.Parameters.UsersClient;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        string GetUserQuery(IGetUserParameters parameters, TweetMode? tweetMode);
        string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode);

        string GetFriendIdsQuery(IGetFriendIdsParameters parameters);
        string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters);

        string GetRelationshipBetweenQuery(IGetRelationshipBetweenParameters parameters);
        string DownloadProfileImageURL(IGetProfileImageParameters parameters);
    }
}