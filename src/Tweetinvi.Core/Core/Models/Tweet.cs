using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
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

        #region Public Attributes

        private IUser _createdBy;
        private ITweetEntities _entities;

        private void DTOUpdated()
        {
            _createdBy = _tweetDTO == null ? null : Client.Factories.CreateUser(_tweetDTO.CreatedBy);
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

        public long Id
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
            set => _tweetDTO.Text = value;
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
            get => _tweetDTO.ExtendedTweet?.FullText ?? _tweetDTO.FullText ?? _tweetDTO.Text;
            set => _tweetDTO.FullText = value;
        }

        public int[] DisplayTextRange => _tweetDTO.ExtendedTweet?.DisplayTextRange ?? _tweetDTO.DisplayTextRange;

        public int[] SafeDisplayTextRange => DisplayTextRange ?? new[] { 0, FullText.Length };

        public IExtendedTweet ExtendedTweet
        {
            get => _tweetDTO.ExtendedTweet;
            set => _tweetDTO.ExtendedTweet = value;
        }

        public bool Favorited => _tweetDTO.Favorited;

        public int FavoriteCount => _tweetDTO.FavoriteCount ?? 0;

        public ICoordinates Coordinates
        {
            get => _tweetDTO.Coordinates;
            set => _tweetDTO.Coordinates = value;
        }

        public ITweetEntities Entities => _entities;

        public IUser CreatedBy => _createdBy;

        public ITweetIdentifier CurrentUserRetweetIdentifier => _tweetDTO.CurrentUserRetweetIdentifier;

        public DateTimeOffset CreatedAt => _tweetDTO.CreatedAt;

        public string Source
        {
            get => _tweetDTO.Source;
            set => _tweetDTO.Source = value;
        }

        public bool Truncated => _tweetDTO.Truncated;

        public int? ReplyCount
        {
            get => _tweetDTO.ReplyCount;
            set => _tweetDTO.QuoteCount = value;
        }

        public long? InReplyToStatusId
        {
            get => _tweetDTO.InReplyToStatusId;
            set => _tweetDTO.InReplyToStatusId = value;
        }

        public string InReplyToStatusIdStr
        {
            get => _tweetDTO.InReplyToStatusIdStr;
            set => _tweetDTO.InReplyToStatusIdStr = value;
        }

        public long? InReplyToUserId
        {
            get => _tweetDTO.InReplyToUserId;
            set => _tweetDTO.InReplyToUserId = value;
        }

        public string InReplyToUserIdStr
        {
            get => _tweetDTO.InReplyToUserIdStr;
            set => _tweetDTO.InReplyToUserIdStr = value;
        }

        public string InReplyToScreenName
        {
            get => _tweetDTO.InReplyToScreenName;
            set => _tweetDTO.InReplyToScreenName = value;
        }

        public int[] ContributorsIds => _tweetDTO.ContributorsIds;

        public IEnumerable<long> Contributors => _tweetDTO.Contributors;

        public int RetweetCount => _tweetDTO.RetweetCount;

        public bool Retweeted => _tweetDTO.Retweeted;

        public bool IsRetweet => _tweetDTO.RetweetedTweetDTO != null;

        private ITweet _retweetedTweet;

        public ITweet RetweetedTweet
        {
            get
            {
                if (_retweetedTweet == null)
                {
                    _retweetedTweet = Client.Factories.CreateTweet(_tweetDTO.RetweetedTweetDTO);
                }

                return _retweetedTweet;
            }
        }

        public int? QuoteCount
        {
            get => _tweetDTO.QuoteCount;
            set => _tweetDTO.QuoteCount = value;
        }

        public long? QuotedStatusId => _tweetDTO.QuotedStatusId;

        public string QuotedStatusIdStr => _tweetDTO.QuotedStatusIdStr;

        private ITweet _quotedTweet;

        public ITweet QuotedTweet
        {
            get
            {
                if (_quotedTweet == null)
                {
                    _quotedTweet = Client.Factories.CreateTweet(_tweetDTO.QuotedTweetDTO);
                }

                return _quotedTweet;
            }
        }

        public bool PossiblySensitive => _tweetDTO.PossiblySensitive;

        public Language? Language => _tweetDTO.Language;

        public IPlace Place => _tweetDTO.Place;

        public Dictionary<string, object> Scopes => _tweetDTO.Scopes;

        public string FilterLevel => _tweetDTO.FilterLevel;

        public bool WithheldCopyright => _tweetDTO.WithheldCopyright;

        public IEnumerable<string> WithheldInCountries => _tweetDTO.WithheldInCountries;

        public string WithheldScope => _tweetDTO.WithheldScope;

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

        public string Url => $"https://twitter.com/{CreatedBy?.ScreenName}/status/{Id.ToString(CultureInfo.InvariantCulture).ToLowerInvariant()}";

        public TweetMode TweetMode { get; }

        #endregion

        #endregion

        public Tweet(ITweetDTO tweetDTO, TweetMode? tweetMode, ITwitterClient client)
        {
            Client = client;

            // IMPORTANT: POSITION MATTERS! Look line below!
            TweetMode = tweetMode ?? TweetMode.Extended;

            // IMPORTANT: Make sure that the TweetDTO is set up after the TweetMode because it uses the TweetMode to initialize the Entities
            TweetDTO = tweetDTO;
        }

        public Task<ITweet> PublishRetweetAsync()
        {
            return Client.Tweets.PublishRetweetAsync(this);
        }

        public Task<ITweet[]> GetRetweetsAsync()
        {
            return Client.Tweets.GetRetweetsAsync(this);
        }

        public Task DestroyRetweetAsync()
        {
            return Client.Tweets.DestroyTweetAsync(this);
        }

        public Task<IOEmbedTweet> GenerateOEmbedTweetAsync()
        {
            return Client.Tweets.GetOEmbedTweetAsync(this);
        }

        public Task DestroyAsync()
        {
            return Client.Tweets.DestroyTweetAsync(this);
        }

        public Task FavoriteAsync()
        {
            return Client.Tweets.FavoriteTweetAsync(this);
        }

        public Task UnfavoriteAsync()
        {
            return Client.Tweets.UnfavoriteTweetAsync(this);
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

            return _tweetDTO.Equals(other.TweetDTO);
        }
    }
}