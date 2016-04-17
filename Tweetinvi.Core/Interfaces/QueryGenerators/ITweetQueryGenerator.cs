using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Parameters.QueryParameters;

namespace Tweetinvi.Core.Interfaces.QueryGenerators
{
    public interface ITweetQueryGenerator
    {
        // Get Tweet
        string GetTweetQuery(long tweetId);
        string GetTweetsQuery(IEnumerable<long> tweetIds);

        // Publish Tweet
        string GetPublishTweetQuery(IPublishTweetParameters queryParameters);

        // Publish Retweet
        string GetPublishRetweetQuery(ITweetDTO tweetDTO);
        string GetPublishRetweetQuery(long tweetId);

        // Get Retweets
        string GetRetweetsQuery(ITweetIdentifier tweetDTO, int maxRetweetsToRetrieve);

        // Get Retweeters
        string GetRetweeterIdsQuery(ITweetIdentifier tweet, int maxRetweetersToRetrieve);

        // Publish UnRetweet
        string GetUnRetweetQuery(ITweetIdentifier tweetIdentifier);
        string GetUnRetweetQuery(long tweetId);

        // Destroy Tweet
        string GetDestroyTweetQuery(ITweetDTO tweetDTO);
        string GetDestroyTweetQuery(long tweetId);

        // Generate OembedTweet
        string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO);
        string GetGenerateOEmbedTweetQuery(long tweetId);

        // Favorite Tweet
        string GetFavoriteTweetQuery(ITweetDTO tweetDTO);
        string GetFavoriteTweetQuery(long tweetId);

        string GetUnFavoriteTweetQuery(ITweetDTO tweetDTO);
        string GetUnFavoriteTweetQuery(long tweetId);
    }
}
