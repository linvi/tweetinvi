using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Logic.TwitterEntities;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// Class representing a Tweet
    /// https://dev.twitter.com/docs/api/1/get/statuses/show/%3Aid
    /// </summary>
    public class Tweet : ITweet
    {
        public const Int16 MAX_TWEET_SIZE = 140;
        public const Int16 MEDIA_CONTENT_SIZE = 23;

        private ITweetDTO _tweetDTO;

        private readonly ITweetController _tweetController;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITaskFactory _taskFactory;
        private readonly IFactory<IMedia> _mediaFactory;

        #region Public Attributes

        private IUser _createdBy;
        private ITweetEntities _entities;

        private void DTOUpdated()
        {
            _createdBy = _tweetDTO == null ? null : _userFactory.GenerateUserFromDTO(_tweetDTO.CreatedBy);
            _entities = _tweetDTO == null ? null : new TweetEntities(_tweetDTO);
        }

        public ITweetDTO TweetDTO
        {
            get { return _tweetDTO; }
            set
            {
                _tweetDTO = value;
                DTOUpdated();
            }
        }

        #region Twitter API Attributes

        public long Id
        {
            get { return _tweetDTO.Id; }
        }

        public string IdStr
        {
            get { return _tweetDTO.IdStr; }
        }

        public string Text
        {
            get { return _tweetDTO.Text; }
            set { _tweetDTO.Text = value; }
        }

        public bool Favourited
        {
            get { return _tweetDTO.Favourited; }
        }

        public int FavouriteCount
        {
            get { return _tweetDTO.FavouriteCount; }
        }

        public ICoordinates Coordinates
        {
            get { return _tweetDTO.Coordinates; }
            set { _tweetDTO.Coordinates = value; }
        }

        public ITweetEntities Entities
        {
            get { return _entities; }
        }

        public IUser CreatedBy
        {
            get { return _createdBy; }
        }

        public ITweetIdentifier CurrentUserRetweetIdentifier
        {
            get { return _tweetDTO.CurrentUserRetweetIdentifier; }
        }

        public DateTime CreatedAt
        {
            get { return _tweetDTO.CreatedAt; }
        }

        public string Source
        {
            get { return _tweetDTO.Source; }
            set { _tweetDTO.Source = value; }
        }

        public bool Truncated
        {
            get { return _tweetDTO.Truncated; }
        }

        public long? InReplyToStatusId
        {
            get { return _tweetDTO.InReplyToStatusId; }
            set { _tweetDTO.InReplyToStatusId = value; }
        }

        public string InReplyToStatusIdStr
        {
            get { return _tweetDTO.InReplyToStatusIdStr; }
            set { _tweetDTO.InReplyToStatusIdStr = value; }
        }

        public long? InReplyToUserId
        {
            get { return _tweetDTO.InReplyToUserId; }
            set { _tweetDTO.InReplyToUserId = value; }
        }

        public string InReplyToUserIdStr
        {
            get { return _tweetDTO.InReplyToUserIdStr; }
            set { _tweetDTO.InReplyToUserIdStr = value; }
        }

        public string InReplyToScreenName
        {
            get { return _tweetDTO.InReplyToScreenName; }
            set { _tweetDTO.InReplyToScreenName = value; }
        }

        public int[] ContributorsIds
        {
            get { return _tweetDTO.ContributorsIds; }
        }

        public IEnumerable<long> Contributors
        {
            get { return _tweetDTO.Contributors; }
        }

        public int RetweetCount
        {
            get { return _tweetDTO.RetweetCount; }
        }

        public bool Retweeted
        {
            get { return _tweetDTO.Retweeted; }
        }

        public bool IsRetweet
        {
            get { return _tweetDTO.RetweetedTweetDTO != null; }
        }

        private ITweet _retweetedTweet;
        public ITweet RetweetedTweet
        {
            get
            {
                if (_retweetedTweet == null)
                {
                    _retweetedTweet = _tweetFactory.GenerateTweetFromDTO(_tweetDTO.RetweetedTweetDTO);
                }

                return _retweetedTweet;
            }
        }

        public bool PossiblySensitive
        {
            get { return _tweetDTO.PossiblySensitive; }
        }

        public Language Language
        {
            get { return _tweetDTO.Language; }
        }

        public IPlace Place
        {
            get { return _tweetDTO.Place; }
        }

        public Dictionary<string, object> Scopes
        {
            get { return _tweetDTO.Scopes; }
        }

        public string FilterLevel
        {
            get { return _tweetDTO.FilterLevel; }
        }

        public bool WithheldCopyright
        {
            get { return _tweetDTO.WithheldCopyright; }
        }

        public IEnumerable<string> WithheldInCountries
        {
            get { return _tweetDTO.WithheldInCountries; }
        }

        public string WithheldScope
        {
            get { return _tweetDTO.WithheldScope; }
        }

        #endregion

        #region Tweetinvi API Accessors

        public List<IHashtagEntity> Hashtags
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Hashtags;
                }

                return null;
            }
        }

        public List<IUrlEntity> Urls
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Urls;
                }

                return null;
            }
        }

        public List<IMediaEntity> Media
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.Medias;
                }

                return null;
            }
        }

        public List<IUserMentionEntity> UserMentions
        {
            get
            {
                if (Entities != null)
                {
                    return Entities.UserMentions;
                }

                return null;
            }
        }

        #endregion

        #region Tweetinvi API Attributes

        public int Length
        {
            get
            {
                var textTweetLength = _tweetDTO.Text == null ? 0 : StringExtension.TweetLength(_tweetDTO.Text);

                var mediaTweetLength = 0;
                if (!_tweetDTO.IsTweetPublished && _tweetDTO.MediasToPublish != null && _tweetDTO.MediasToPublish.Any())
                {
                    mediaTweetLength = MEDIA_CONTENT_SIZE;
                }

                if (_tweetDTO.IsTweetPublished && _tweetDTO.LegacyEntities != null && _tweetDTO.LegacyEntities.Medias != null && _tweetDTO.LegacyEntities.Medias.Any())
                {
                    mediaTweetLength = MEDIA_CONTENT_SIZE;
                }

                return textTweetLength + mediaTweetLength;
            }
        }

        public bool IsTweetPublished
        {
            get { return _tweetDTO.IsTweetPublished; }
        }

        public bool IsTweetDestroyed
        {
            get { return _tweetDTO.IsTweetDestroyed; }
        }

        private readonly DateTime _tweetLocalCreationDate = DateTime.Now;
        public DateTime TweetLocalCreationDate
        {
            get { return _tweetLocalCreationDate; }
        }

        public List<ITweet> Retweets { get; set; }

        #endregion

        #endregion

        public Tweet(
            ITweetDTO tweetDTO,
            ITweetController tweetController,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITaskFactory taskFactory,
            IFactory<IMedia> mediaFactory)
        {
            _tweetController = tweetController;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _taskFactory = taskFactory;
            _mediaFactory = mediaFactory;

            TweetDTO = tweetDTO;
        }

        public int TweetRemainingCharacters()
        {
            return MAX_TWEET_SIZE - Length;
        }

        public IMedia AddMedia(byte[] data, string name = null)
        {
            var media = _mediaFactory.Create();
            media.Data = data;
            media.Name = name;

            AddMediaToPublishList(media);
            return media;
        }

        public IMedia AddMediaAsAClone(IMedia media)
        {
            var clone = media.CloneWithoutMediaInfo(media);
            AddMediaToPublishList(clone);
            return clone;
        }

        private void AddMediaToPublishList(IMedia media)
        {
            if (IsTweetPublished)
            {
                throw new InvalidOperationException("Cannot add media after the tweet has been published!");
            }

            if (TweetDTO.MediasToPublish == null)
            {
                TweetDTO.MediasToPublish = new List<IMedia>();
            }

            if (TweetDTO.MediasToPublish.Count >= 4)
            {
                throw new InvalidOperationException("A tweet cannot contain more than 4 media.");
            }

            TweetDTO.MediasToPublish.Add(media);
        }

        public bool Publish()
        {
            return _tweetController.PublishTweet(this);
        }

        public ITweet PublishReply(string text)
        {
            var tweetToPublish = _tweetFactory.CreateTweet(text);
            if (!_tweetController.PublishTweetInReplyTo(tweetToPublish, this))
            {
                return null;
            }

            return tweetToPublish;
        }

        public bool PublishReply(ITweet replyTweet)
        {
            return _tweetController.PublishTweetInReplyTo(replyTweet, this);
        }

        public bool PublishInReplyTo(ITweet replyToTweet)
        {
            if (replyToTweet == null || replyToTweet.Id == TweetinviSettings.DEFAULT_ID)
            {
                return false;
            }

            return PublishInReplyTo(replyToTweet.Id);
        }

        public bool PublishInReplyTo(long replyToTweetId)
        {
            return _tweetController.PublishTweetInReplyTo(this, replyToTweetId);
        }

        public bool PublishWithGeo(ICoordinates coordinates)
        {
            _tweetDTO.Coordinates = coordinates;
            return _tweetController.PublishTweet(this);
        }

        public bool PublishWithGeo(double longitude, double latitude)
        {
            var coordinates = new CoordinatesDTO(longitude, latitude);
            return PublishWithGeo(coordinates);
        }

        public bool PublishWithGeoInReplyTo(double longitude, double latitude, ITweet replyToTweet)
        {
            if (replyToTweet == null || replyToTweet.Id == TweetinviSettings.DEFAULT_ID)
            {
                return false;
            }

            return PublishWithGeoInReplyTo(longitude, latitude, replyToTweet.Id);
        }

        public bool PublishWithGeoInReplyTo(double longitude, double latitude, long replyToTweetId)
        {
            var coordinates = new CoordinatesDTO(longitude, latitude);
            return PublishWithGeoInReplyTo(coordinates, replyToTweetId);
        }

        public bool PublishWithGeoInReplyTo(ICoordinates coordinates, ITweet replyToTweet)
        {
            if (replyToTweet == null || replyToTweet.Id == TweetinviSettings.DEFAULT_ID)
            {
                return false;
            }

            return PublishWithGeoInReplyTo(coordinates, replyToTweet.Id);
        }

        public bool PublishWithGeoInReplyTo(ICoordinates coordinates, long replyToTweetId)
        {
            return _tweetController.PublishTweetWithGeoInReplyTo(this, coordinates, replyToTweetId);
        }

        private bool CanTweetBeRetweeted()
        {
            return _tweetDTO != null && _tweetDTO.Id != TweetinviSettings.DEFAULT_ID && IsTweetPublished && !IsTweetDestroyed;
        }

        public ITweet PublishRetweet()
        {
            if (!CanTweetBeRetweeted())
            {
                return null;
            }

            return _tweetController.PublishRetweet(this);
        }

        public List<ITweet> GetRetweets()
        {
            var retweets = _tweetController.GetRetweets(_tweetDTO);
            if (retweets == null)
            {
                return null;
            }

            return retweets.ToList();
        }

        public IOEmbedTweet GenerateOEmbedTweet()
        {
            return _tweetController.GenerateOEmbedTweet(_tweetDTO);
        }

        public bool Destroy()
        {
            return _tweetController.DestroyTweet(_tweetDTO);
        }

        public void Favourite()
        {
            if (_tweetController.FavoriteTweet(_tweetDTO))
            {
                _tweetDTO.Favourited = true;
            }
        }

        public void UnFavourite()
        {
            if (_tweetController.UnFavoriteTweet(_tweetDTO))
            {
                _tweetDTO.Favourited = false;
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public bool Equals(ITweet other)
        {
            // Equals is currently used to compare only if 2 tweets are the same
            // We do not look for the tweet version (DateTime)

            bool result = _tweetDTO.Equals(other.TweetDTO) &&
                          IsTweetPublished == other.IsTweetPublished &&
                          IsTweetDestroyed == other.IsTweetDestroyed;

            return result;
        }

        #region ICloneable

        /// <summary>
        /// Copy a Tweet into a new one
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return _tweetFactory.GenerateTweetFromDTO(_tweetDTO);
        }

        #endregion

        #region Async
        public async Task<bool> PublishAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Publish());
        }

        public async Task<ITweet> PublishReplyAsync(string text)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishReply(text));
        }

        public async Task<bool> PublishInReplyToAsync(long replyToTweetId)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishInReplyTo(replyToTweetId));
        }

        public async Task<bool> PublishInReplyToAsync(ITweet replyToTweet)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishInReplyTo(replyToTweet));
        }

        public async Task<ITweet> PublishRetweetAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishRetweet());
        }

        public async Task<List<ITweet>> GetRetweetsAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetRetweets());
        }

        public async Task<bool> PublishWithGeoAsync(ICoordinates coordinates)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishWithGeo(coordinates));
        }

        public async Task<bool> PublishWithGeoAsync(double longitude, double latitude)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishWithGeo(longitude, latitude));
        }

        public async Task<bool> PublishWithGeoInReplyToAsync(double longitude, double latitude, long replyToTweetId)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishWithGeoInReplyTo(longitude, latitude, replyToTweetId));
        }

        public async Task<bool> PublishWithGeoInReplyToAsync(double longitude, double latitude, ITweet replyToTweet)
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishWithGeoInReplyTo(longitude, latitude, replyToTweet));
        }

        public async Task FavouriteAsync()
        {
            await _taskFactory.ExecuteTaskAsync(Favourite);
        }

        public async Task UnFavouriteAsync()
        {
            await _taskFactory.ExecuteTaskAsync(UnFavourite);
        }

        public async Task<IOEmbedTweet> GenerateOEmbedTweetAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GenerateOEmbedTweet());
        }

        public async Task<bool> DestroyAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => Destroy());
        }
        #endregion
    }
}