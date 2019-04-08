using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITweetController
    {
        // Publish Tweet
        Task<ITweet> PublishTweet(string text);
        Task<ITweet> PublishTweet(IPublishTweetParameters parameters);

        Task<ITweet> PublishTweetInReplyTo(string text, long tweetId);
        Task<ITweet> PublishTweetInReplyTo(string text, ITweetIdentifier tweet);

        bool CanBePublished(string text);
        bool CanBePublished(IPublishTweetParameters publishTweetParameters);

        // Publish Retweet
        Task<ITweet> PublishRetweet(ITweet tweetToPublish);
        Task<ITweet> PublishRetweet(ITweetDTO tweetToPublish);
        Task<ITweet> PublishRetweet(long tweetId);
        
        // Publish UnRetweet
        Task<ITweet> UnRetweet(ITweetIdentifier tweetToPublish);
        Task<ITweet> UnRetweet(long tweetId);

        // Get Retweets
        Task<IEnumerable<ITweet>> GetRetweets(ITweetIdentifier tweet, int maxRetweetsToRetrieve = 100);
        Task<IEnumerable<ITweet>> GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100);

        // Get Retweeters
        Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweet, int maxRetweetersToRetrieve = 100);
        Task<IEnumerable<long>> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100);

        // Destroy Tweet
        Task<bool> DestroyTweet(ITweet tweet);
        Task<bool> DestroyTweet(ITweetDTO tweetDTO);
        Task<bool> DestroyTweet(long tweetId);

        // Generate OembedTweet
        Task<IOEmbedTweet> GenerateOEmbedTweet(ITweet tweet);
        Task<IOEmbedTweet> GenerateOEmbedTweet(ITweetDTO tweetDTO);
        Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId);

        // Favorite Tweet
        Task<bool> FavoriteTweet(ITweet tweet);
        Task<bool> FavoriteTweet(ITweetDTO tweetDTO);
        Task<bool> FavoriteTweet(long tweetId);

        Task<bool> UnFavoriteTweet(ITweet tweet);
        Task<bool> UnFavoriteTweet(ITweetDTO tweetDTO);
        Task<bool> UnFavoriteTweet(long tweetId);

        // Update Published Tweet
        void UpdateTweetIfTweetSuccessfullyBeenPublished(ITweet sourceTweet, ITweetDTO publishedTweetDTO);
        Task UploadMedias(IPublishTweetParameters parameters);
    }
}