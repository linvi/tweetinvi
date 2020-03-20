using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface ITweetController
    {
        // TWEET
        Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetTweets(IGetTweetsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request);


        // FAVORITES
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request);


        bool CanBePublished(string text);
        bool CanBePublished(IPublishTweetParameters publishTweetParameters);

        // Retweets - Publish
        Task<ITwitterResult<ITweetDTO>> PublishRetweet(IPublishRetweetParameters parameters, ITwitterRequest request);

        // Retweets - Destroy
        Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweets(IGetRetweetsParameters parameters, ITwitterRequest request);

        // Get Retweeters
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters, ITwitterRequest request);

        // Favorite Tweet
        Task<ITwitterResult<ITweetDTO>> FavoriteTweet(IFavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> UnfavoriteTweet(IUnfavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters, ITwitterRequest request);

    }
}