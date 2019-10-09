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
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetController : ITweetController
    {
        private readonly ITweetQueryExecutor _tweetQueryExecutor;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly ITweetFactory _tweetFactory;

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetFactory tweetFactory)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetFactory = tweetFactory;
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
        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(ITweetIdentifier tweetId, ITwitterRequest request)
        {
            return _tweetQueryExecutor.PublishRetweet(tweetId, request);
        }

        // Retweets - Destroy

        public Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweet, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyRetweet(retweet, request);
        }

        #region GetRetweets

        public Task<ITwitterResult<ITweetDTO[]>> GetRetweets(ITweetIdentifier tweetIdentifier, int? maxRetweetsToRetrieve, ITwitterRequest request)
        {
            return _tweetQueryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve, request);
        }

        #endregion

        #region Get Retweeters Ids

        public Task<IEnumerable<long>> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100)
        {
            return _tweetQueryExecutor.GetRetweetersIds(new TweetIdentifier(tweetId), maxRetweetersToRetrieve);
        }

        public Task<IEnumerable<long>> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
            return _tweetQueryExecutor.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);
        }

        #endregion

        // Destroy Tweet
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyTweet(parameters, request);
        }

        // Favorite Tweet
        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters, ITwitterRequest request)
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?>(
                parameters.MaxId,
                cursor =>
                {
                    var cursoredParameters = new GetFavoriteTweetsParameters(parameters)
                    {
                        MaxId = cursor
                    };

                    return _tweetQueryExecutor.GetFavoriteTweets(cursoredParameters, new TwitterRequest(request));
                },
                page =>
                {
                    return page.DataTransferObject.Min(x => x.Id);
                },
                page => page.DataTransferObject.Length < parameters.PageSize);

            return twitterCursorResult;
        }
        
        public Task<bool> FavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return FavoriteTweet(tweet.TweetDTO);
        }

        public async Task<bool> FavoriteTweet(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return false;
            }

            // if the favourite operation failed the tweet should still be favourited if it previously was
            tweetDTO.Favorited |= await _tweetQueryExecutor.FavoriteTweet(tweetDTO).ConfigureAwait(false);
            return tweetDTO.Favorited;
        }

        public Task<bool> FavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.FavoriteTweet(tweetId);
        }

        // UnFavorite
        public Task<bool> UnFavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return UnFavoriteTweet(tweet.TweetDTO);
        }

        public Task<bool> UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            return _tweetQueryExecutor.UnFavoriteTweet(tweetDTO);
        }

        public Task<bool> UnFavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.UnFavoriteTweet(tweetId);
        }

        // Generate OembedTweet
        public Task<IOEmbedTweet> GenerateOEmbedTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return GenerateOEmbedTweet(tweet.TweetDTO);
        }

        public async Task<IOEmbedTweet> GenerateOEmbedTweet(ITweetDTO tweetDTO)
        {
            var oembedTweetDTO = await _tweetQueryExecutor.GenerateOEmbedTweet(tweetDTO).ConfigureAwait(false);
            return _tweetFactory.GenerateOEmbedTweetFromDTO(oembedTweetDTO);
        }

        public async Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            var oembedTweetDTO = await _tweetQueryExecutor.GenerateOEmbedTweet(tweetId).ConfigureAwait(false);
            return _tweetFactory.GenerateOEmbedTweetFromDTO(oembedTweetDTO);
        }

        // Update Tweet
        public void UpdateTweetIfTweetSuccessfullyBeenPublished(ITweet sourceTweet, ITweetDTO publishedTweetDTO)
        {
            if (sourceTweet != null &&
                publishedTweetDTO != null &&
                publishedTweetDTO.IsTweetPublished)
            {
                sourceTweet.TweetDTO = publishedTweetDTO;
            }
        }
    }
}
