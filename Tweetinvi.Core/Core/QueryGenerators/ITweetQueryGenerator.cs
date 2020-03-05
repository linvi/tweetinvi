using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface ITweetQueryGenerator
    {
        string GetTweetQuery(IGetTweetParameters parameters, TweetMode? tweetMode);
        string GetTweetsQuery(IGetTweetsParameters parameters, TweetMode? tweetMode);
        string GetPublishTweetQuery(IPublishTweetParameters parameters, TweetMode? tweetMode);
        string GetDestroyTweetQuery(IDestroyTweetParameters parameters, TweetMode? tweetMode);

        string GetFavoriteTweetsQuery(IGetFavoriteTweetsParameters parameters, TweetMode? tweetMode);

        string GetRetweetsQuery(IGetRetweetsParameters parameters, TweetMode? tweetMode);
        string GetPublishRetweetQuery(IPublishRetweetParameters parameters, TweetMode? tweetMode);
        string GetDestroyRetweetQuery(IDestroyRetweetParameters parameters, TweetMode? tweetMode);
        string GetRetweeterIdsQuery(IGetRetweeterIdsParameters parameters);

        string GetCreateFavoriteTweetQuery(IFavoriteTweetParameters parameters);
        string GetUnfavoriteTweetQuery(IUnfavoriteTweetParameters parameters);

        string GetOEmbedTweetQuery(IGetOEmbedTweetParameters parameters);
    }
}
