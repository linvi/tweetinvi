using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        string GetUserQuery(IGetUserParameters parameters, TweetMode? tweetMode);
        string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode);

        string GetFriendIdsQuery(IGetFriendIdsParameters parameters);
        string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters);

        // Download Profile Image
        string DownloadProfileImageURL(IGetProfileImageParameters parameters);
    }
}