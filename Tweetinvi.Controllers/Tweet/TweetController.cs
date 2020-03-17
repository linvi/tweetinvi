using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
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

        public Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetTweet(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweets(IGetTweetsParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetTweets(parameters, request);
        }

        public async Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            await UploadMedias(parameters, request).ConfigureAwait(false);
            return await _tweetQueryExecutor.PublishTweet(parameters, request).ConfigureAwait(false);
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

        public async Task UploadMedias(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            if (parameters.Medias.Any(x => !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the medias could not be published!");
            }

            parameters.MediaIds.AddRange(parameters.Medias.Select(x => x.UploadedMediaInfo.MediaId));

            var uploadedMedias = new List<IMedia>();

            foreach (var binary in parameters.MediaBinaries)
            {
                var uploadResult = await _uploadQueryExecutor.UploadBinary(new UploadParameters(binary), request).ConfigureAwait(false);
                uploadedMedias.Add(uploadResult.Media);
            }

            if (uploadedMedias.Any(x => x == null || !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the binaries could not be published!");
            }

            parameters.MediaIds.AddRange(uploadedMedias.Select(x => x.UploadedMediaInfo.MediaId));
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(IPublishRetweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.PublishRetweet(parameters, request);
        }

        // Retweets - Destroy

        public Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyRetweet(parameters, request);
        }

        #region GetRetweets

        public Task<ITwitterResult<ITweetDTO[]>> GetRetweets(IGetRetweetsParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetRetweets(parameters, request);
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

                return _tweetQueryExecutor.GetRetweeterIds(cursoredParameters, new TwitterRequest(request));
            });
        }

        // Destroy Tweet
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyTweet(parameters, request);
        }

        // Favorite Tweet
        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweetsIterator(IGetFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetFavoriteTweetsParameters(parameters)
                {
                    MaxId = cursor
                };

                return _tweetQueryExecutor.GetFavoriteTweets(cursoredParameters, new TwitterRequest(request));
            });
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweet(IFavoriteTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.FavoriteTweet(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweet(IUnfavoriteTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.UnfavoriteTweet(parameters, request);
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetOEmbedTweet(parameters, request);
        }
    }
}