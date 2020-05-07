using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetController : ITweetController
    {
        private readonly ITweetQueryExecutor _tweetQueryExecutor;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            IUploadQueryExecutor uploadQueryExecutor,
            IPageCursorIteratorFactories pageCursorIteratorFactories)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _uploadQueryExecutor = uploadQueryExecutor;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
        }

        public Task<ITwitterResult<ITweetDTO>> GetTweetAsync(IGetTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetTweetAsync(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweetsAsync(IGetTweetsParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetTweetsAsync(parameters, request);
        }

        public async Task<ITwitterResult<ITweetDTO>> PublishTweetAsync(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            parameters.MediaIds.AddRange(parameters.Medias.Select(x => x.UploadedMediaInfo.MediaId));
            return await _tweetQueryExecutor.PublishTweetAsync(parameters, request).ConfigureAwait(false);
        }

        public bool CanBePublished(IPublishTweetParameters publishTweetParameters)
        {
            return true;
            //return TweetinviConsts.MAX_TWEET_SIZE >= EstimateTweetLength(publishTweetParameters);
        }

        public bool CanBePublished(string text)
        {
            return true;
            //return TweetinviConsts.MAX_TWEET_SIZE >= EstimateTweetLength(text);
        }

        public static int EstimateTweetLength(string text)
        {
            var parameters = new PublishTweetParameters(text);
            return EstimateTweetLength(parameters);
        }

        private static int EstimateTweetLength(IPublishTweetParameters publishTweetParameters)
        {
            var text = publishTweetParameters.Text ?? "";
#pragma warning disable 618
            var textLength = StringExtension.EstimateTweetLength(text);

            if (publishTweetParameters.QuotedTweet != null)
            {
                textLength = StringExtension.EstimateTweetLength(text.TrimEnd()) +
                             1 + // for the space that needs to be added before the link to quoted tweet.
                             TweetinviConsts.MEDIA_CONTENT_SIZE;
#pragma warning restore 618
            }

            if (publishTweetParameters.HasMedia)
            {
                textLength += TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            return textLength;
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishRetweetAsync(IPublishRetweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.PublishRetweetAsync(parameters, request);
        }

        // Retweets - Destroy

        public Task<ITwitterResult<ITweetDTO>> DestroyRetweetAsync(IDestroyRetweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyRetweetAsync(parameters, request);
        }

        #region GetRetweets

        public Task<ITwitterResult<ITweetDTO[]>> GetRetweetsAsync(IGetRetweetsParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetRetweetsAsync(parameters, request);
        }

        #endregion

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetRetweeterIdsParameters(parameters)
                {
                    Cursor = cursor
                };

                return _tweetQueryExecutor.GetRetweeterIdsAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        // Destroy Tweet
        public Task<ITwitterResult<ITweetDTO>> DestroyTweetAsync(IDestroyTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyTweetAsync(parameters, request);
        }

        // Favorite Tweet
        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserFavoriteTweetsParameters(parameters)
                {
                    MaxId = cursor
                };

                return _tweetQueryExecutor.GetFavoriteTweetsAsync(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweetAsync(IFavoriteTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.FavoriteTweetAsync(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.UnfavoriteTweetAsync(parameters, request);
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetOEmbedTweetAsync(parameters, request);
        }
    }
}