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
        Task<ITwitterResult<ITweetDTO>> GetTweetAsync(IGetTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO[]>> GetTweetsAsync(IGetTweetsParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> PublishTweetAsync(IPublishTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> DestroyTweetAsync(IDestroyTweetParameters parameters, ITwitterRequest request);


        // FAVORITES
        ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request);


        bool CanBePublished(string text);
        bool CanBePublished(IPublishTweetParameters publishTweetParameters);

        // Retweets - Publish
        Task<ITwitterResult<ITweetDTO>> PublishRetweetAsync(IPublishRetweetParameters parameters, ITwitterRequest request);

        // Retweets - Destroy
        Task<ITwitterResult<ITweetDTO>> DestroyRetweetAsync(IDestroyRetweetParameters parameters, ITwitterRequest request);

        // Get Retweets
        Task<ITwitterResult<ITweetDTO[]>> GetRetweetsAsync(IGetRetweetsParameters parameters, ITwitterRequest request);

        // Get Retweeters
        ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters, ITwitterRequest request);

        // Favorite Tweet
        Task<ITwitterResult<ITweetDTO>> FavoriteTweetAsync(IFavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<ITweetDTO>> UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters, ITwitterRequest request);
        Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters, ITwitterRequest request);

    }
}