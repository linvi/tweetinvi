using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.QueryGenerators;

namespace Tweetinvi.Controllers.Tweet
{
    public class TweetQueryGenerator : ITweetQueryGenerator
    {
        private readonly IGeoQueryGenerator _geoQueryGenerator;
        private readonly ITweetQueryValidator _tweetQueryValidator;
        private readonly ITwitterStringFormatter _twitterStringFormatter;

        public TweetQueryGenerator(
            IGeoQueryGenerator geoQueryGenerator,
            ITweetQueryValidator tweetQueryValidator,
            ITwitterStringFormatter twitterStringFormatter)
        {
            _geoQueryGenerator = geoQueryGenerator;
            _tweetQueryValidator = tweetQueryValidator;
            _twitterStringFormatter = twitterStringFormatter;
        }

        private string CleanupString(string source)
        {
            return _twitterStringFormatter.TwitterEncode(source);
        }

        // Get Tweet
        public string GetTweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Get, tweetId);
        }

        public string GetTweetsQuery(IEnumerable<long> tweetIds)
        {
            if (tweetIds == null || tweetIds.Count() == 1)
            {
                return null;
            }

            var idsParameter = string.Join("%2C", tweetIds);
            return string.Format(Resources.Tweet_Lookup, idsParameter);
        }

        // Publish Tweet
        public string GetPublishTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.CanTweetDTOBePublished(tweetDTO))
            {
                return null;
            }

            string baseQuery = String.Format(Resources.Tweet_Publish, CleanupString(tweetDTO.Text));
            return AddAdditionalParameters(tweetDTO, baseQuery);
        }

        // Publish Tweet in reply to
        public string GetPublishTweetInReplyToQuery(ITweetDTO tweetToPublish, ITweetDTO tweetToReplyTo)
        {
            if (!_tweetQueryValidator.CanTweetDTOBePublished(tweetToPublish) ||
                !_tweetQueryValidator.IsTweetPublished(tweetToReplyTo))
            {
                return null;
            }

            return GetPublishTweetInReplyToQuery(tweetToPublish, tweetToReplyTo.Id);
        }

        public string GetPublishTweetInReplyToQuery(ITweetDTO tweetToPublish, long tweetIdToReplyTo)
        {
            if (!_tweetQueryValidator.CanTweetDTOBePublished(tweetToPublish))
            {
                return null;
            }

            string baseQuery = String.Format(Resources.Tweet_PublishInReplyTo, CleanupString(tweetToPublish.Text), tweetIdToReplyTo);
            return AddAdditionalParameters(tweetToPublish, baseQuery);
        }

        // Publish Retweet
        public string GetPublishRetweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetPublishRetweetQuery(tweetDTO.Id);
        }

        public string GetPublishRetweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Retweet_Publish, tweetId);
        }

        // Get Retweets
        public string GetRetweetsQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetRetweetsQuery(tweetDTO.Id);
        }

        public string GetRetweetsQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Retweet_GetRetweets, tweetId);
        }

        // Destroy Tweet
        public string GetDestroyTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.CanTweetDTOBeDestroyed(tweetDTO))
            {
                return null;
            }

            return GetDestroyTweetQuery(tweetDTO.Id);
        }

        public string GetDestroyTweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Destroy, tweetId);
        }

        // Favorite Tweet
        public string GetFavouriteTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetFavouriteTweetQuery(tweetDTO.Id);
        }

        public string GetFavouriteTweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Favorite_Create, tweetId);
        }

        // Unfavourite Tweet
        public string GetUnFavouriteTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetUnFavouriteTweetQuery(tweetDTO.Id);
        }

        public string GetUnFavouriteTweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_Favorite_Destroy, tweetId);
        }

        // OEmbed Tweet
        public string GetGenerateOEmbedTweetQuery(ITweetDTO tweetDTO)
        {
            if (!_tweetQueryValidator.IsTweetPublished(tweetDTO))
            {
                return null;
            }

            return GetGenerateOEmbedTweetQuery(tweetDTO.Id);
        }

        public string GetGenerateOEmbedTweetQuery(long tweetId)
        {
            return String.Format(Resources.Tweet_GenerateOEmbed, tweetId);
        }

        // Helper
        private string AddAdditionalParameters(ITweetDTO tweet, string baseQuery)
        {
            StringBuilder query = new StringBuilder(baseQuery);

            string placeIdParameter = _geoQueryGenerator.GeneratePlaceIdParameter(tweet.PlaceId);
            if (!String.IsNullOrEmpty(placeIdParameter))
            {
                query.Append(String.Format("&{0}", placeIdParameter));
            }

            string coordinatesParameter = _geoQueryGenerator.GenerateGeoParameter(tweet.Coordinates);
            if (!String.IsNullOrEmpty(coordinatesParameter))
            {
                query.Append(String.Format("&{0}", coordinatesParameter));
            }

            var mediaIdsParameter = GetMediaIdsParameter(tweet);
            if (!string.IsNullOrEmpty(mediaIdsParameter))
            {
                query.Append(string.Format("&{0}", mediaIdsParameter));
            }

            return query.ToString();
        }

        private string GetMediaIdsParameter(ITweetDTO tweetDTO)
        {
            var uploadedMedias = GetTweetMediaIdsToPublish(tweetDTO);
            if (uploadedMedias.Length == 0)
            {
                return string.Empty;
            }

            var mediaIdsParameter = string.Join("%2C", uploadedMedias.Select(x => x.ToString(CultureInfo.InvariantCulture)));
            return string.Format("media_ids={0}", mediaIdsParameter);
        }

        private long[] GetTweetMediaIdsToPublish(ITweetDTO tweetToPublish)
        {
            if (tweetToPublish.MediasToPublish == null)
            {
                return new long[] { };
            }

            var tweetMedias = tweetToPublish.MediasToPublish.Where(x => x.UploadedMediaInfo != null);
            return tweetMedias.Select(x => x.UploadedMediaInfo.MediaId).ToArray();
        }
    }
}