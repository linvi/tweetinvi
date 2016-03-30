using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.Models.Entities;
using Tweetinvi.Logic.TwitterEntities;

namespace Tweetinvi.Logic
{
    /// <summary>
    /// Class representing a Tweet
    /// https://dev.twitter.com/docs/api/1/get/statuses/show/%3Aid
    /// </summary>
    public class Tweet : ITweet
    {
        private ITweetDTO _tweetDTO;

        private readonly ITweetController _tweetController;
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;
        private readonly ITaskFactory _taskFactory;

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

        public bool Favorited
        {
            get { return _tweetDTO.Favorited; }
        }

        public int FavoriteCount
        {
            get { return _tweetDTO.FavoriteCount; }
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

        public long? QuotedStatusId
        {
            get { return _tweetDTO.QuotedStatusId; }
        }

        public string QuotedStatusIdStr
        {
            get { return _tweetDTO.QuotedStatusIdStr; }
        }

        private ITweet _quotedTweet;
        public ITweet QuotedTweet
        {
            get
            {
                if (_quotedTweet == null)
                {
                    _quotedTweet = _tweetFactory.GenerateTweetFromDTO(_tweetDTO.QuotedTweetDTO);
                }

                return _quotedTweet;
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

        public int PublishedTweetLength
        {
            get
            {
                if (!_tweetDTO.IsTweetPublished)
                {
                    throw new InvalidOperationException("Cannot calculate the length before the tweet has been published. Use the CalculateLength method instead.");
                }

                return GetLength(false);
            }
        }

        public int CalculateLength(bool willBePublishedWithMedia)
        {
            return GetLength(willBePublishedWithMedia);
        }

        private int GetLength(bool willBePublishedWithMedia)
        {
            var textLength = _tweetDTO.Text == null ? 0 : _tweetDTO.Text.TweetLength(willBePublishedWithMedia);

            var mediaTweetLength = 0;
            if (_tweetDTO.IsTweetPublished && _tweetDTO.LegacyEntities != null && _tweetDTO.LegacyEntities.Medias != null && _tweetDTO.LegacyEntities.Medias.Any())
            {
                mediaTweetLength = TweetinviConsts.MEDIA_CONTENT_SIZE;
            }

            return textLength + mediaTweetLength;
        }

        public bool IsTweetPublished
        {
            get { return _tweetDTO.IsTweetPublished; }
        }

        public bool IsTweetDestroyed
        {
            get { return _tweetDTO.IsTweetDestroyed; }
        }

        public string Url
        {
            get { return string.Format("https://twitter.com/{0}/status/{1}", CreatedBy?.ScreenName, Id.ToString().ToLowerInvariant()); }
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
            ITaskFactory taskFactory)
        {
            _tweetController = tweetController;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _taskFactory = taskFactory;

            TweetDTO = tweetDTO;
        }

        public int TweetRemainingCharacters(bool hasMedia)
        {
            return TweetinviConsts.MAX_TWEET_SIZE - CalculateLength(hasMedia);
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

        public bool UnRetweet()
        {
            var updatedTweet = _tweetController.UnRetweet(this);
            if (updatedTweet != null)
            {
                _tweetDTO.Retweeted = false;
            }

            return _tweetController.UnRetweet(this) != null;
        }

        public IOEmbedTweet GenerateOEmbedTweet()
        {
            return _tweetController.GenerateOEmbedTweet(_tweetDTO);
        }

        public bool Destroy()
        {
            return _tweetController.DestroyTweet(_tweetDTO);
        }

        public void Favorite()
        {
            if (_tweetController.FavoriteTweet(_tweetDTO))
            {
                _tweetDTO.Favorited = true;
            }
        }

        public void UnFavorite()
        {
            if (_tweetController.UnFavoriteTweet(_tweetDTO))
            {
                _tweetDTO.Favorited = false;
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
        

        public async Task<ITweet> PublishRetweetAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => PublishRetweet());
        }

        public async Task<List<ITweet>> GetRetweetsAsync()
        {
            return await _taskFactory.ExecuteTaskAsync(() => GetRetweets());
        }

        public async Task FavoriteAsync()
        {
            await _taskFactory.ExecuteTaskAsync(Favorite);
        }

        public async Task UnFavoriteAsync()
        {
            await _taskFactory.ExecuteTaskAsync(UnFavorite);
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