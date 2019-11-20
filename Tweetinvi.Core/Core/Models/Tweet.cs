using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Models.TwitterEntities;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Core.Models
{
    /// <summary>
    /// Class representing a Tweet
    /// https://dev.twitter.com/docs/api/1/get/statuses/show/%3Aid
    /// </summary>
    public class Tweet : ITweet
    {
        private ITweetDTO _tweetDTO;

        public ITwitterClient Client { get; set; }
#pragma warning disable 649
        // TODO : REMOVE AS SOON AS Client has migrated all the methods of TweetController
        private readonly ITweetController _tweetController;
#pragma warning restore 649
        private readonly ITweetFactory _tweetFactory;
        private readonly IUserFactory _userFactory;

        #region Public Attributes

        private IUser _createdBy;
        private ITweetEntities _entities;

        private void DTOUpdated()
        {
            _createdBy = _tweetDTO == null ? null : _userFactory.GenerateUserFromDTO(_tweetDTO.CreatedBy, null);
            _entities = _tweetDTO == null ? null : new TweetEntities(_tweetDTO, TweetMode);
        }

        public ITweetDTO TweetDTO
        {
            get => _tweetDTO;
            set
            {
                _tweetDTO = value;
                DTOUpdated();
            }
        }

        #region Twitter API Attributes

        public long? Id
        {
            get => _tweetDTO.Id;
            set => _tweetDTO.Id = value;
        }

        public string IdStr
        {
            get => _tweetDTO.IdStr;
            set => _tweetDTO.IdStr = value;
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

        public int[] SafeDisplayTextRange => DisplayTextRange ?? new[] { 0, FullText.Length };

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
                    _retweetedTweet = _tweetFactory.GenerateTweetFromDTO(_tweetDTO.RetweetedTweetDTO, Client.Config.TweetMode, Client);
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
                    _quotedTweet = _tweetFactory.GenerateTweetFromDTO(_tweetDTO.QuotedTweetDTO, TweetMode, Client);
                }

                return _quotedTweet;
            }
        }

        public bool PossiblySensitive
        {
            get { return _tweetDTO.PossiblySensitive; }
        }

        public Language? Language
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

        public TweetMode TweetMode { get; }

        #endregion

        #endregion

        public Tweet(
            ITweetDTO tweetDTO,
            TweetMode? tweetMode,
            ITweetFactory tweetFactory,
            IUserFactory userFactory)
        {
            _tweetFactory = tweetFactory;
            _userFactory = userFactory;

            // IMPORTANT: POSITION MATTERS! Look line below!
            TweetMode = tweetMode ?? TweetMode.Extended;

            // IMPORTANT: Make sure that the TweetDTO is set up after the TweetMode because it uses the TweetMode to initialize the Entities
            TweetDTO = tweetDTO;
        }

        public Task<ITweet> PublishRetweet()
        {
            ThrowIfTweetCannotBeUsed();
            return Client.Tweets.PublishRetweet(this);
        }

        public Task<ITweet[]> GetRetweets()
        {
            ThrowIfTweetCannotBeUsed();
            return Client.Tweets.GetRetweets(this);
        }

        public Task<bool> DestroyRetweet()
        {
            ThrowIfTweetCannotBeUsed();
            return Client.Tweets.DestroyTweet(this);
        }

        private void ThrowIfTweetCannotBeUsed()
        {
            if (!IsTweetPublished)
            {
                throw new InvalidOperationException("Cannot execute the operation when the Tweet has not yet been published");
            }

            if (IsTweetDestroyed)
            {
                throw new InvalidOperationException("Cannot execute the operation if the Tweet has been deleted.");
            }
        }

        public Task<IOEmbedTweet> GenerateOEmbedTweet()
        {
            ThrowIfTweetCannotBeUsed();
            return _tweetController.GenerateOEmbedTweet(_tweetDTO);
        }

        public Task<bool> Destroy()
        {
            ThrowIfTweetCannotBeUsed();
            return Client.Tweets.DestroyTweet(this);
        }

        public async Task Favorite()
        {
            ThrowIfTweetCannotBeUsed();

            if (await _tweetController.FavoriteTweet(_tweetDTO))
            {
                _tweetDTO.Favorited = true;
            }
        }

        public async Task UnFavorite()
        {
            ThrowIfTweetCannotBeUsed();

            if (await _tweetController.UnFavoriteTweet(_tweetDTO))
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
            if (other == null)
            {
                return false;
            }
            
            // Equals is currently used to compare only if 2 tweets are the same
            // We do not look for the tweet version (DateTime)

            return _tweetDTO.Equals(other.TweetDTO) &&
                   IsTweetPublished == other.IsTweetPublished &&
                   IsTweetDestroyed == other.IsTweetDestroyed;
        }
    }
}