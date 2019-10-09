using Tweetinvi.Models;
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
        
        
        
        
        
        

        // Get Retweeters
        string GetRetweeterIdsQuery(ITweetIdentifier tweet, int maxRetweetersToRetrieve);


        // Generate OembedTweet
        string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO);
        string GetGenerateOEmbedTweetQuery(long? tweetId);

        // Favorite Tweet
        string GetFavoriteTweetQuery(ITweetDTO tweetDTO);
        string GetFavoriteTweetQuery(long? tweetId);

        string GetUnFavoriteTweetQuery(ITweetDTO tweetDTO);
        string GetUnFavoriteTweetQuery(long? tweetId);

    }
}
