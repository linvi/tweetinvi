using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetController : ITweetController
    {
        private readonly ITweetQueryExecutor _tweetQueryExecutor;
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly IGeoFactory _geoFactory;

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            ITweetQueryValidator tweetQueryValidator,
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetFactory tweetFactory,
            IGeoFactory geoFactory)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _tweetQueryValidator = tweetQueryValidator;
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetFactory = tweetFactory;
            _geoFactory = geoFactory;
        }

        // Publish Tweet

        public ITweet PublishTweet(IPublishTweetParameters parameters)
        {
            return _tweetFactory.GenerateTweetFromDTO(InternalPublishTweet(parameters));
        }

        public ITweet PublishTweet(string text, IPublishTweetOptionalParameters optionalParameters = null)
        {
            var parameters = new PublishTweetParameters(text, optionalParameters);
            var tweetDTO = InternalPublishTweet(parameters);

            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        public bool PublishTweet(ITweet tweet, IPublishTweetOptionalParameters optionalParameters = null)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var parameters = new PublishTweetParameters(tweet.Text, optionalParameters);
            var tweetDTO = InternalPublishTweet(parameters);

            UpdateTweetIfTweetSuccessfullyBeenPublished(tweet, tweetDTO);

            return tweet.IsTweetPublished;
        }

        public ITweet PublishTweetWithMedia(string text, long mediaId)
        {
            var parameters = new PublishTweetOptionalParameters();
            parameters.MediaIds.Add(mediaId);

            return PublishTweet(text, parameters);
        }

        public ITweet PublishTweetWithMedia(string text, byte[] media)
        {
            var parameters = new PublishTweetOptionalParameters();
            parameters.MediaBinaries.Add(media);

            return PublishTweet(text, parameters);
        }

        public ITweet PublishTweetWithVideo(string text, byte[] video)
        {
            var media = _uploadQueryExecutor.UploadVideo(video);
            if (media == null || media.MediaId == null || !media.HasBeenUploaded)
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the medias could not be published!");
            }

            var parameters = new PublishTweetOptionalParameters();
            parameters.MediaIds.Add((long)media.MediaId);

            return PublishTweet(text, parameters);
        }

        public ITweet PublishTweetInReplyTo(string text, long tweetId)
        {
            var parameters = new PublishTweetOptionalParameters();
            parameters.InReplyToTweetId = tweetId;

            return PublishTweet(text, parameters);
        }

        public ITweet PublishTweetInReplyTo(string text, ITweetIdentifier tweet)
        {
            var parameters = new PublishTweetOptionalParameters();
            parameters.InReplyToTweet = tweet;

            return PublishTweet(text, parameters);
        }

        public int Length(IPublishTweetParameters publishTweetParameters)
        {
            return Length(publishTweetParameters.Text, publishTweetParameters.Parameters);
        }

        public int Length(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            var textLength = text == null ? 0 : text.TweetLength();

            if (publishTweetOptionalParameters == null)
            {
                return textLength;
            }

            if (publishTweetOptionalParameters.QuotedTweet != null)
            {
                textLength += TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            if (!publishTweetOptionalParameters.Medias.IsNullOrEmpty() ||
                !publishTweetOptionalParameters.MediaIds.IsNullOrEmpty() ||
                !publishTweetOptionalParameters.MediaBinaries.IsNullOrEmpty())
            {
                textLength += TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            return textLength;
        }

        public bool CanBePublished(IPublishTweetParameters publishTweetParameters)
        {
            return TweetinviConsts.MAX_TWEET_SIZE >= Length(publishTweetParameters);
        }

        public bool CanBePublished(string text, IPublishTweetOptionalParameters publishTweetOptionalParameters = null)
        {
            return TweetinviConsts.MAX_TWEET_SIZE >= Length(text, publishTweetOptionalParameters);
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
            _uploadQueryExecutor.UploadMedias(parameters.Medias, false);

            if (parameters.Medias.Any(x => !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the medias could not be published!");
            }
            else
            {
                parameters.MediaIds.AddRange(parameters.Medias.Select(x => x.UploadedMediaInfo.MediaId));
            }

            var binariesMedia = _uploadQueryExecutor.UploadBinaries(parameters.MediaBinaries);
            if (binariesMedia.Any(x => !x.HasBeenUploaded))
            {
                throw new OperationCanceledException("The tweet cannot be published as some of the binaries could not be published!");
            }
            else
            {
                parameters.MediaIds.AddRange(binariesMedia.Select(x => x.UploadedMediaInfo.MediaId));
            }
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
