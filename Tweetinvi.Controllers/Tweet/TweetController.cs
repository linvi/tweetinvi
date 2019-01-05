using System;
using System.Collections.Generic;
using System.Linq;
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

        public ITweet PublishTweet(IPublishTweetParameters parameters)
        {
            return _tweetFactory.GenerateTweetFromDTO(InternalPublishTweet(parameters));
        }

        public ITweet PublishTweet(string text)
        {
            var parameters = new PublishTweetParameters(text);
            var tweetDTO = InternalPublishTweet(parameters);

            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public ITweet PublishTweetInReplyTo(string text, long tweetId)
        {
            var parameters = new PublishTweetParameters(text)
            {
                InReplyToTweetId = tweetId
            };

            return PublishTweet(parameters);
        }

        public ITweet PublishTweetInReplyTo(string text, ITweetIdentifier tweet)
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

        private ITweetDTO InternalPublishTweet(IPublishTweetParameters parameters)
        {
            // The exceptions have to be raised before the QueryGenerator as 
            // We do not want to wait for the media to be uploaded to throw the
            // Exception. And The logic of uploading the media should live in
            // the TweetController

            _tweetQueryValidator.ThrowIfTweetCannotBePublished(parameters);

            UploadMedias(parameters);

            return _tweetQueryExecutor.PublishTweet(parameters);
        }

        public void UploadMedias(IPublishTweetParameters parameters)
        {
            if (parameters.Medias.Any(x => !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the medias could not be published!");
            }

            parameters.MediaIds.AddRange(parameters.Medias.Select(x => x.UploadedMediaInfo.MediaId));

            var uploadedMedias = parameters.MediaBinaries
                .Select(binary => { return _uploadQueryExecutor.UploadBinary(binary); }).ToArray();

            if (uploadedMedias.Any(x => x == null || !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the binaries could not be published!");
            }

            parameters.MediaIds.AddRange(uploadedMedias.Select(x => x.UploadedMediaInfo.MediaId));
        }

        // Publish Retweet
        public ITweet PublishRetweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return PublishRetweet(tweet.TweetDTO);
        }

        public ITweet PublishRetweet(ITweetDTO tweet)
        {
            var tweetDTO = _tweetQueryExecutor.PublishRetweet(tweet);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public ITweet PublishRetweet(long tweetId)
        {
            var tweetDTO = _tweetQueryExecutor.PublishRetweet(tweetId);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }
        
        // Publish UnRetweet

        public ITweet UnRetweet(ITweetIdentifier tweet)
        {
            var tweetDTO = _tweetQueryExecutor.UnRetweet(tweet);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public ITweet UnRetweet(long tweetId)
        {
            var tweetDTO = _tweetQueryExecutor.UnRetweet(tweetId);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        #region GetRetweets

        public IEnumerable<ITweet> GetRetweets(ITweetIdentifier tweetIdentifier, int maxRetweetsToRetrieve = 100)
        {
            var retweetsDTO = _tweetQueryExecutor.GetRetweets(tweetIdentifier, maxRetweetsToRetrieve);
            return _tweetFactory.GenerateTweetsFromDTO(retweetsDTO);
        }

        public IEnumerable<ITweet> GetRetweets(long tweetId, int maxRetweetsToRetrieve = 100)
        {
            return GetRetweets(new TweetIdentifier(tweetId), maxRetweetsToRetrieve);
        }

        #endregion

        #region Get Retweeters Ids

        public IEnumerable<long> GetRetweetersIds(long tweetId, int maxRetweetersToRetrieve = 100)
        {
            return _tweetQueryExecutor.GetRetweetersIds(new TweetIdentifier(tweetId), maxRetweetersToRetrieve);
        }

        public IEnumerable<long> GetRetweetersIds(ITweetIdentifier tweetIdentifier, int maxRetweetersToRetrieve = 100)
        {
            return _tweetQueryExecutor.GetRetweetersIds(tweetIdentifier, maxRetweetersToRetrieve);
        }

        #endregion

        // Destroy Tweet
        public bool DestroyTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return DestroyTweet(tweet.TweetDTO);
        }

        public bool DestroyTweet(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return false;
            }

            tweetDTO.IsTweetDestroyed = _tweetQueryExecutor.DestroyTweet(tweetDTO);
            return tweetDTO.IsTweetDestroyed;
        }

        public bool DestroyTweet(long tweetId)
        {
            return _tweetQueryExecutor.DestroyTweet(tweetId);
        }

        // Favorite Tweet
        public bool FavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return FavoriteTweet(tweet.TweetDTO);
        }

        public bool FavoriteTweet(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return false;
            }

            // if the favourite operation failed the tweet should still be favourited if it previously was
            tweetDTO.Favorited |= _tweetQueryExecutor.FavoriteTweet(tweetDTO);
            return tweetDTO.Favorited;
        }

        public bool FavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.FavoriteTweet(tweetId);
        }

        // UnFavorite
        public bool UnFavoriteTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return UnFavoriteTweet(tweet.TweetDTO);
        }

        public bool UnFavoriteTweet(ITweetDTO tweetDTO)
        {
            return _tweetQueryExecutor.UnFavoriteTweet(tweetDTO);
        }

        public bool UnFavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.UnFavoriteTweet(tweetId);
        }

        // Generate OembedTweet
        public IOEmbedTweet GenerateOEmbedTweet(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return GenerateOEmbedTweet(tweet.TweetDTO);
        }

        public IOEmbedTweet GenerateOEmbedTweet(ITweetDTO tweetDTO)
        {
            var oembedTweetDTO = _tweetQueryExecutor.GenerateOEmbedTweet(tweetDTO);
            return _tweetFactory.GenerateOEmbedTweetFromDTO(oembedTweetDTO);
        }

        public IOEmbedTweet GenerateOEmbedTweet(long tweetId)
        {
            var oembedTweetDTO = _tweetQueryExecutor.GenerateOEmbedTweet(tweetId);
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
