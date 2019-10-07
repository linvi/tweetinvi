using System.Collections.Generic;
using Tweetinvi.Core.Client;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface ITweetQueryGenerator
    {
        string GetTweetQuery(IGetTweetParameters parameters, TweetMode? tweetMode);
        string GetPublishTweetQuery(IPublishTweetParameters parameters, TweetMode? tweetMode);

        string GetFavoriteTweetsQuery(IGetFavoriteTweetsParameters parameters, TweetMode? tweetMode);
        
        
        
        
        
        
        
        string GetTweetsQuery(IEnumerable<long> tweetIds);

        // Publish Tweet

        // Publish Retweet
        string GetPublishRetweetQuery(ITweetIdentifier tweetId, TweetMode? tweetMode);

        // Get Retweets
        string GetRetweetsQuery(ITweetIdentifier tweetId, int? maxRetweetsToRetrieve, ITwitterExecutionContext executionContext);

        // Get Retweeters
        string GetRetweeterIdsQuery(ITweetIdentifier tweet, int maxRetweetersToRetrieve);

        // Publish UnRetweet
        string GetUnRetweetQuery(ITweetIdentifier tweetIdentifier);
        string GetUnRetweetQuery(long? tweetId);

        // Destroy Tweet
        string GetDestroyTweetQuery(long? tweetId);

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
