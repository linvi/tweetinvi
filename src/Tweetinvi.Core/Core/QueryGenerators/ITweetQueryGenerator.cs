using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface ITweetQueryGenerator
    {
        string GetTweetQuery(IGetTweetParameters parameters, ComputedTweetMode tweetMode);
        string GetTweetsQuery(IGetTweetsParameters parameters, ComputedTweetMode tweetMode);
        string GetPublishTweetQuery(IPublishTweetParameters parameters, ComputedTweetMode tweetMode);
        string GetDestroyTweetQuery(IDestroyTweetParameters parameters, ComputedTweetMode tweetMode);

        string GetFavoriteTweetsQuery(IGetUserFavoriteTweetsParameters parameters, ComputedTweetMode tweetMode);

        string GetRetweetsQuery(IGetRetweetsParameters parameters, ComputedTweetMode tweetMode);
        string GetPublishRetweetQuery(IPublishRetweetParameters parameters, ComputedTweetMode tweetMode);
        string GetDestroyRetweetQuery(IDestroyRetweetParameters parameters, ComputedTweetMode tweetMode);
        string GetRetweeterIdsQuery(IGetRetweeterIdsParameters parameters);

        string GetCreateFavoriteTweetQuery(IFavoriteTweetParameters parameters);
        string GetUnfavoriteTweetQuery(IUnfavoriteTweetParameters parameters);

        string GetOEmbedTweetQuery(IGetOEmbedTweetParameters parameters);
    }
}
