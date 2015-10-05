using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
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
        string GetRetweetsQuery(ITweetDTO tweetDTO);
        string GetRetweetsQuery(long tweetId);

        // Destroy Tweet
        string GetDestroyTweetQuery(ITweetDTO tweetDTO);
        string GetDestroyTweetQuery(long tweetId);

        // Generate OembedTweet
        string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO);
        string GetGenerateOEmbedTweetQuery(long tweetId);

        // Favorite Tweet
        string GetFavouriteTweetQuery(ITweetDTO tweetDTO);
        string GetFavouriteTweetQuery(long tweetId);

        string GetUnFavouriteTweetQuery(ITweetDTO tweetDTO);
        string GetUnFavouriteTweetQuery(long tweetId);
    }
}
