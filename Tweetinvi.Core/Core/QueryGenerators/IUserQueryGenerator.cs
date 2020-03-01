using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IUserQueryGenerator
    {
        string GetAuthenticatedUserQuery(IGetAuthenticatedUserParameters parameters, TweetMode? tweetMode);

        string GetUserQuery(IGetUserParameters parameters, TweetMode? tweetMode);
        string GetUsersQuery(IGetUsersParameters parameters, TweetMode? tweetMode);

        string GetFriendIdsQuery(IGetFriendIdsParameters parameters);
        string GetFollowerIdsQuery(IGetFollowerIdsParameters parameters);

        string GetRelationshipBetweenQuery(IGetRelationshipBetweenParameters parameters);
        string DownloadProfileImageURL(IGetProfileImageParameters parameters);

        // BLOCK
        string GetBlockUserQuery(IBlockUserParameters parameters);
        string GetUnblockUserQuery(IUnblockUserParameters parameters);
        string GetReportUserForSpamQuery(IReportUserForSpamParameters parameters);
        string GetBlockedUserIdsQuery(IGetBlockedUserIdsParameters parameters);
        string GetBlockedUsersQuery(IGetBlockedUsersParameters parameters);

        // FOLLOWERS
        string GetFollowUserQuery(IFollowUserParameters parameters);
        string GetUnFollowUserQuery(IUnFollowUserParameters parameters);

        // RELATIONSHIPS
        string GetUpdateRelationshipQuery(IUpdateRelationshipParameters parameters);

        // ONGOING REQUESTS
        string GetUserIdsRequestingFriendshipQuery(IGetUserIdsRequestingFriendshipParameters parameters);
        string GetUserIdsYouRequestedToFollowQuery(IGetUserIdsYouRequestedToFollowParameters parameters);

        // FRIENDSHIPS
        string GetRelationshipsWithQuery(IGetRelationshipsWithParameters parameters);

        // MUTE
        string GetUserIdsWhoseRetweetsAreMutedQuery(IGetUserIdsWhoseRetweetsAreMutedParameters parameters);
        string GetMutedUserIdsQuery(IGetMutedUserIdsParameters parameters);
        string GetMutedUsersQuery(IGetMutedUsersParameters parameters);
        string GetMuteUserQuery(IMuteUserParameters parameters);
        string GetUnMuteUserQuery(IUnMuteUserParameters parameters);
    }
}