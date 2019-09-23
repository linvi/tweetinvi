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
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly ITwitterResultFactory _twitterResultFactory;

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            ITweetQueryValidator tweetQueryValidator,
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetFactory tweetFactory,
            ITwitterResultFactory twitterResultFactory)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _tweetQueryValidator = tweetQueryValidator;
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetFactory = tweetFactory;
            _twitterResultFactory = twitterResultFactory;
        }

        // Publish Tweet

        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            return InternalPublishTweet(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(string text, ITwitterRequest request)
        {
            var parameters = new PublishTweetParameters(text);
            return InternalPublishTweet(parameters, request);
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

        public static int EstimateTweetLength(IPublishTweetParameters publishTweetParameters)
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

        private async Task<ITwitterResult<ITweetDTO, ITweet>> InternalPublishTweet(IPublishTweetParameters parameters, ITwitterRequest request)
        {
            // The exceptions have to be raised before the QueryGenerator as 
            // We do not want to wait for the media to be uploaded to throw the
            // Exception. And The logic of uploading the media should live in
            // the TweetController

            _tweetQueryValidator.ThrowIfTweetCannotBePublished(parameters);

            await UploadMedias(parameters);

            var result = await _tweetQueryExecutor.PublishTweet(parameters, request);

            return _twitterResultFactory.Create(result, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, request.ExecutionContext));
        }

        public async Task UploadMedias(IPublishTweetParameters parameters)
        {
            if (parameters.Medias.Any(x => !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the medias could not be published!");
            }

            parameters.MediaIds.AddRange(parameters.Medias.Select(x => x.UploadedMediaInfo.MediaId));

            var uploadedMedias = new List<IMedia>();

            foreach (var binary in parameters.MediaBinaries)
            {
                var uploadedMedia = await _uploadQueryExecutor.UploadBinary(binary);
                uploadedMedias.Add(uploadedMedia);
            }

            if (uploadedMedias.Any(x => x == null || !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the binaries could not be published!");
            }

            parameters.MediaIds.AddRange(uploadedMedias.Select(x => x.UploadedMediaInfo.MediaId));
        }

        // Retweets - Publish
        public async Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(ITweetIdentifier tweetId, ITwitterRequest request)
        {
            var result = await _tweetQueryExecutor.PublishRetweet(tweetId, request);
            return _twitterResultFactory.Create(result, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, request.ExecutionContext));
        }

        // Retweets - Destroy

        public async Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweet, ITwitterRequest request)
        {
            var result = await _tweetQueryExecutor.DestroyRetweet(retweet, request);
            return result;
        }

        #region GetRetweets

        public async Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(ITweetIdentifier tweetIdentifier, int? maxRetweetsToRetrieve, ITwitterRequest request)
        {
            var retweetsDTO = await _tweetQueryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve, request);
            return _twitterResultFactory.Create(retweetsDTO, tweetDTOs => _tweetFactory.GenerateTweetsFromDTO(tweetDTOs, request.ExecutionContext.TweetMode, request.ExecutionContext));
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(long tweetId, int maxRetweetsToRetrieve, ITwitterRequest request)
        {
            return GetRetweets(new TweetIdentifier(tweetId), maxRetweetsToRetrieve, request);
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
        public async Task<ITwitterResult> DestroyTweet(ITweetDTO tweet, ITwitterRequest request)
        {
            _tweetQueryValidator.ThrowIfTweetCannotBeDestroyed(tweet);

            var twitterResult = await _tweetQueryExecutor.DestroyTweet(tweet.Id, request);
            tweet.IsTweetDestroyed = twitterResult.Response.IsSuccessStatusCode;

            return twitterResult;
        }

        public Task<ITwitterResult> DestroyTweet(long tweetId, ITwitterRequest request)
        {
            return _tweetQueryExecutor.DestroyTweet(tweetId, request);
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
            tweetDTO.Favorited |= await _tweetQueryExecutor.FavoriteTweet(tweetDTO);
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
            var oembedTweetDTO = await _tweetQueryExecutor.GenerateOEmbedTweet(tweetDTO);
            return _tweetFactory.GenerateOEmbedTweetFromDTO(oembedTweetDTO);
        }

        public async Task<IOEmbedTweet> GenerateOEmbedTweet(long tweetId)
        {
            var oembedTweetDTO = await _tweetQueryExecutor.GenerateOEmbedTweet(tweetId);
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
