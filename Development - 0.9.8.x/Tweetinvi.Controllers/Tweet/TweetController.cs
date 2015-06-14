using System;
using System.Collections.Generic;
using Tweetinvi.Controllers.Upload;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetController : ITweetController
    {
        private readonly ITweetQueryExecutor _tweetQueryExecutor;
        private readonly IUploadQueryExecutor _uploadQueryExecutor;
        private readonly ITweetFactory _tweetFactory;
        private readonly IGeoFactory _geoFactory;

        public TweetController(
            ITweetQueryExecutor tweetQueryExecutor,
            IUploadQueryExecutor uploadQueryExecutor,
            ITweetFactory tweetFactory,
            IGeoFactory geoFactory)
        {
            _tweetQueryExecutor = tweetQueryExecutor;
            _uploadQueryExecutor = uploadQueryExecutor;
            _tweetFactory = tweetFactory;
            _geoFactory = geoFactory;
        }

        // Publish Tweet
        public bool PublishTweet(ITweet tweetToPublish)
        {
            if (tweetToPublish == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var publishedTweetDTO = PublishTweetDTO(tweetToPublish.TweetDTO);
            UpdateTweetIfTweetSuccessfullyBeenPublished(tweetToPublish, publishedTweetDTO);

            return tweetToPublish.IsTweetPublished;
        }

        public ITweet PublishTweet(ITweetDTO tweetToPublish)
        {
            var tweetDTO = PublishTweetDTO(tweetToPublish);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        private ITweetDTO PublishTweetDTO(ITweetDTO tweetToPublish)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            var publishedTweetDTO = _tweetQueryExecutor.PublishTweet(tweetToPublish);
            return publishedTweetDTO;
        }

        // Publish Tweet InReplyTo
        public bool PublishTweetInReplyTo(ITweet tweetToPublish, ITweet tweetToReplyTo)
        {
            if (tweetToPublish == null || tweetToReplyTo == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var publishedTweetDTO = PublishTweetDTOInReplyTo(tweetToPublish.TweetDTO, tweetToReplyTo.TweetDTO);
            UpdateTweetIfTweetSuccessfullyBeenPublished(tweetToPublish, publishedTweetDTO);

            return tweetToPublish.IsTweetPublished;
        }

        public ITweet PublishTweetInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo)
        {
            var tweetDTO = PublishTweetDTOInReplyTo(tweetToPublish, tweetToReplyTo);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        private ITweetDTO PublishTweetDTOInReplyTo(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            var tweetDTO = _tweetQueryExecutor.PublishTweetInReplyTo(tweetToPublish, tweetToReplyTo);
            return tweetDTO;
        }

        public bool PublishTweetInReplyTo(ITweet tweetToPublish, long tweetIdToReplyTo)
        {
            if (tweetToPublish == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            var publishedTweetDTO = PublishTweetDTOInReplyTo(tweetToPublish.TweetDTO, tweetIdToReplyTo);
            UpdateTweetIfTweetSuccessfullyBeenPublished(tweetToPublish, publishedTweetDTO);

            return tweetToPublish.IsTweetPublished;
        }

        public ITweet PublishTweetInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo)
        {
            var tweetDTO = PublishTweetDTOInReplyTo(tweetToPublish, tweetIdToReplyTo);
            return _tweetFactory.GenerateTweetFromDTO(tweetDTO);
        }

        private ITweetDTO PublishTweetDTOInReplyTo(ITweetDTO tweetToPublish, long tweetIdToReplyTo)
        {
            _uploadQueryExecutor.UploadTweetMediasBeforePublish(tweetToPublish);

            var tweetDTO = _tweetQueryExecutor.PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);
            return tweetDTO;
        }

        // Publish With Geo
        public bool PublishTweetWithGeo(ITweet tweetToPublish, ICoordinates coordinates)
        {
            if (tweetToPublish == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            tweetToPublish.Coordinates = coordinates;
            return PublishTweet(tweetToPublish);
        }

        public ITweet PublishTweetWithGeo(ITweetDTO tweetToPublishDTO, ICoordinates coordinates)
        {
            if (tweetToPublishDTO == null)
            {
                throw new ArgumentException("TweetDTO cannot be null!");
            }

            tweetToPublishDTO.Coordinates = coordinates;
            return PublishTweet(tweetToPublishDTO);
        }

        public bool PublishTweetWithGeo(ITweet tweetToPublish, double longitude, double latitude)
        {
            var coordinates = _geoFactory.GenerateCoordinates(longitude, latitude);
            return PublishTweetWithGeo(tweetToPublish, coordinates);
        }

        public ITweet PublishTweetWithGeo(ITweetDTO tweetToPublishDTO, double longitude, double latitude)
        {
            var coordinates = _geoFactory.GenerateCoordinates(longitude, latitude);
            return PublishTweetWithGeo(tweetToPublishDTO, coordinates);
        }

        // Publish With Geo In Reply To
        public bool PublishTweetWithGeoInReplyTo(ITweet tweetToPublish, ICoordinates coordinates, long tweetIdToReplyTo)
        {
            if (tweetToPublish == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            tweetToPublish.Coordinates = coordinates;
            return PublishTweetInReplyTo(tweetToPublish, tweetIdToReplyTo);
        }

        public ITweet PublishTweetWithGeoInReplyTo(ITweetDTO tweetToPublishDTO, ICoordinates coordinates, long tweetIdToReplyTo)
        {
            if (tweetToPublishDTO == null)
            {
                throw new ArgumentException("TweetDTO cannot be null!");
            }

            tweetToPublishDTO.Coordinates = coordinates;
            return PublishTweetInReplyTo(tweetToPublishDTO, tweetIdToReplyTo);
        }

        public bool PublishTweetWithGeoInReplyTo(ITweet tweetToPublish, double longitude, double latitude, long tweetIdToReplyTo)
        {
            var coordinates = _geoFactory.GenerateCoordinates(longitude, latitude);
            return PublishTweetWithGeoInReplyTo(tweetToPublish, coordinates, tweetIdToReplyTo);
        }

        public ITweet PublishTweetWithGeoInReplyTo(ITweetDTO tweetToPublishDTO, double longitude, double latitude, long tweetIdToReplyTo)
        {
            var coordinates = _geoFactory.GenerateCoordinates(longitude, latitude);
            return PublishTweetWithGeoInReplyTo(tweetToPublishDTO, coordinates, tweetIdToReplyTo);
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

        // Publish GetRetweets
        public IEnumerable<ITweet> GetRetweets(ITweet tweet)
        {
            if (tweet == null)
            {
                throw new ArgumentException("Tweet cannot be null!");
            }

            return GetRetweets(tweet.TweetDTO);
        }

        public IEnumerable<ITweet> GetRetweets(ITweetDTO tweet)
        {
            var retweetsDTO = _tweetQueryExecutor.GetRetweets(tweet);
            return _tweetFactory.GenerateTweetsFromDTO(retweetsDTO);
        }

        public IEnumerable<ITweet> GetRetweets(long tweetId)
        {
            var retweetsDTO = _tweetQueryExecutor.GetRetweets(tweetId);
            return _tweetFactory.GenerateTweetsFromDTO(retweetsDTO);
        }

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
            tweetDTO.Favourited |= _tweetQueryExecutor.FavouriteTweet(tweetDTO);
            return tweetDTO.Favourited;
        }

        public bool FavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.FavouriteTweet(tweetId);
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
            return _tweetQueryExecutor.UnFavouriteTweet(tweetDTO);
        }

        public bool UnFavoriteTweet(long tweetId)
        {
            return _tweetQueryExecutor.UnFavouriteTweet(tweetId);
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