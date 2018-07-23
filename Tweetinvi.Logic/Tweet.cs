using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Core.Helpers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Logic.TwitterEntities;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

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
            _entities = _tweetDTO == null ? null : new TweetEntities(_tweetDTO, TweetMode);
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
            get
            {
                if (_tweetDTO.Text != null)
                {
                    return _tweetDTO.Text;
                }

                if (_tweetDTO.FullText == null)
                {
                    return null;
                }

                if (DisplayTextRange == null)
                {
                    return _tweetDTO.FullText;
                }

                var contentStartIndex = DisplayTextRange[0];
                var contentEndIndex = DisplayTextRange[1];

                return UnicodeHelper.SubstringByTextElements(_tweetDTO.FullText, contentStartIndex, contentEndIndex - contentStartIndex);
            }
            set { _tweetDTO.Text = value; }
        }

        public string Prefix
        {
            get
            {
                var text = _tweetDTO.ExtendedTweet?.FullText ?? _tweetDTO.FullText;

                if (text != null && DisplayTextRange != null)
                {
                    var prefixEndIndex = DisplayTextRange[0];
                    return text.Substring(0, prefixEndIndex);
                }

                return null; 
            }
        }

        public string Suffix
        {
            get
            {
                var text = _tweetDTO.ExtendedTweet?.FullText ?? _tweetDTO.FullText;

                if (text != null && DisplayTextRange != null)
                {
                    var suffixStartIndex = DisplayTextRange[1];
                    return UnicodeHelper.SubstringByTextElements(text, suffixStartIndex);
                }

                return null;
            }
        }

        public string FullText
        {
            get { return _tweetDTO.ExtendedTweet?.FullText ?? _tweetDTO.FullText ?? _tweetDTO.Text; }
            set { _tweetDTO.FullText = value; }
        }

        public int[] DisplayTextRange
        {
            get { return _tweetDTO.ExtendedTweet?.DisplayTextRange ?? _tweetDTO.DisplayTextRange; }
        }

        public int[] SafeDisplayTextRange => DisplayTextRange ?? new int[] { 0, FullText.Length };

        public IExtendedTweet ExtendedTweet
        {
            get { return _tweetDTO.ExtendedTweet; }
            set { _tweetDTO.ExtendedTweet = value; }
        }

        public bool Favorited
        {
            get { return _tweetDTO.Favorited; }
        }

        public int FavoriteCount
        {
            get { return _tweetDTO.FavoriteCount ?? 0; }
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

        public int? ReplyCount
        {
            get { return _tweetDTO.ReplyCount; }
            set { _tweetDTO.QuoteCount = value; }
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

        public int? QuoteCount
        {
            get { return _tweetDTO.QuoteCount; }
            set { _tweetDTO.QuoteCount = value; }
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

        public TweetMode TweetMode { get; private set; }

        #endregion

        #endregion

        public Tweet(
            ITweetDTO tweetDTO,
            TweetMode? tweetMode,
            ITweetController tweetController,
            ITweetFactory tweetFactory,
            IUserFactory userFactory,
            ITaskFactory taskFactory,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _tweetController = tweetController;
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;
            _taskFactory = taskFactory;

            // IMPORTANT: POSITION MATTERS! Look line below!
            TweetMode = tweetMode ?? tweetinviSettingsAccessor?.CurrentThreadSettings?.TweetMode ?? TweetMode.Extended;

            // IMPORTANT: Make sure that the TweetDTO is set up after the TweetMode because it uses the TweetMode to initialize the Entities
            TweetDTO = tweetDTO;
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
            return FullText;
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