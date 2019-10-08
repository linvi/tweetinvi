using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITweetController
    {
        // TWEET
        Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request);

        
        // FAVORITES
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters, ITwitterRequest request);


        bool CanBePublished(string text);
        bool CanBePublished(IPublishTweetParameters publishTweetParameters);

        // Retweets - Publish
        Task<ITwitterResult<ITweetDTO>> PublishRetweet(ITweetIdentifier tweetId1, ITwitterRequest request);
        
        // Retweets - Destroy
        Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweet, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweets(ITweetIdentifier tweet, int? maxRetweetsToRetrieve, ITwitterRequest request);

        // Get Retweeters
        Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweet, int maxRetweetersToRetrieve = 100);
        Task<IEnumerable<long>> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100);

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
    }
}