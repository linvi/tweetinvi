using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Factories;
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

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            ITweetQueryValidator tweetQueryValidator,
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetFactory tweetFactory)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _tweetQueryValidator = tweetQueryValidator;
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetFactory = tweetFactory;
        }

        // Publish Tweet

        public async Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            var tweetDTO = await InternalPublishTweet(parameters);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public async Task<ITweet> PublishTweet(string text)
        {
            var parameters = new PublishTweetParameters(text);
            var tweetDTO = await InternalPublishTweet(parameters);

            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public Task<ITweet> PublishTweetInReplyTo(string text, long tweetId)
        {
            var parameters = new PublishTweetParameters(text)
            {
                InReplyToTweetId = tweetId
            };

            return PublishTweet(parameters);
        }

        public Task<ITweet> PublishTweetInReplyTo(string text, ITweetIdentifier tweet)
        {
            var parameters = new PublishTweetParameters(text)
            {
                InReplyToTweet = tweet
            };

            return PublishTweet(parameters);
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
            var textLength = StringExtension.EstimateTweetLength(text);

            if (publishTweetParameters.QuotedTweet != null)
            {
                textLength = StringExtension.EstimateTweetLength(text.TrimEnd()) + 
                             1 + // for the space that needs to be added before the link to quoted tweet.
                             TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            if (publishTweetParameters.HasMedia)
            {
                textLength += TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            return textLength;
        }

        private async Task<ITweetDTO> InternalPublishTweet(IPublishTweetParameters parameters)
        {
            // The exceptions have to be raised before the QueryGenerator as 
            // We do not want to wait for the media to be uploaded to throw the
            // Exception. And The logic of uploading the media should live in
            // the TweetController

            _tweetQueryValidator.ThrowIfTweetCannotBePublished(parameters);

            await UploadMedias(parameters);

            return await _tweetQueryExecutor.PublishTweet(parameters);
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

        // Publish Retweet
        public Task<ITweet> PublishRetweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return PublishRetweet(tweet.TweetDTO);
        }

        public async Task<ITweet> PublishRetweet(ITweetDTO tweet)
        {
            var tweetDTO = await _tweetQueryExecutor.PublishRetweet(tweet);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public async Task<ITweet> PublishRetweet(long tweetId)
        {
            var tweetDTO = await _tweetQueryExecutor.PublishRetweet(tweetId);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }
        
        // Publish UnRetweet

        public async Task<ITweet> UnRetweet(ITweetIdentifier tweet)
        {
            var tweetDTO = await _tweetQueryExecutor.UnRetweet(tweet);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public async Task<ITweet> UnRetweet(long tweetId)
        {
            var tweetDTO = await _tweetQueryExecutor.UnRetweet(tweetId);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        #region GetRetweets

        public async Task<IEnumerable<ITweet>> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve = 100)
        {
            var retweetsDTO = await _tweetQueryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve);
            return _tweetFactory.GenerateTweetsFromDTO(retweetsDTO);
        }

        public Task<IEnumerable<ITweet>> GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100)
        {
            return GetRetweets(new TweetIdentifier(tweetId), maxRetweetsToRetrieve);
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
        public Task<bool> DestroyTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return DestroyTweet(tweet.TweetDTO);
        }

        public async Task<bool> DestroyTweet(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return false;
            }

            tweetDTO.IsTweetDestroyed = await _tweetQueryExecutor.DestroyTweet(tweetDTO);
            return tweetDTO.IsTweetDestroyed;
        }

        public Task<bool> DestroyTweet(long tweetId)
        {
            return _tweetQueryExecutor.DestroyTweet(tweetId);
        }

        // Favorite Tweet
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
