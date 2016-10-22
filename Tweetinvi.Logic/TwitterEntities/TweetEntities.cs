using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    internal class TweetEntities : ITweetEntities
    {
        private readonly ITweetDTO _tweetDTO;

        public TweetEntities(ITweetDTO tweetDTO)
        {
            _tweetDTO = tweetDTO;

            InitializeEntities();
        }

        private void InitializeEntities()
        {
            bool useExtendedTweetEntities = _tweetDTO?.ExtendedTweet != null;

            if (useExtendedTweetEntities)
            {
                // URLS
                var allUrls = new List<IUrlEntity>().SafeConcat
                (
                    _tweetDTO?.ExtendedTweet?.LegacyEntities?.Urls,
                    _tweetDTO?.ExtendedTweet?.ExtendedEntities?.Urls
                );

                _urls = new List<IUrlEntity>(allUrls.Distinct((x, y) => x.Equals(y)));

                // USER MENTIONS
                var allUserMentions = new List<IUserMentionEntity>().SafeConcat
                (
                    _tweetDTO?.ExtendedTweet?.LegacyEntities?.UserMentions,
                    _tweetDTO?.ExtendedTweet?.ExtendedEntities?.UserMentions
                );

                _userMentions = new List<IUserMentionEntity>(allUserMentions.Distinct((x, y) => x.Equals(y)));

                // HASHTAGS
                var allHashtags = new List<IHashtagEntity>().SafeConcat
                (
                    _tweetDTO?.ExtendedTweet?.LegacyEntities?.Hashtags,
                    _tweetDTO?.ExtendedTweet?.ExtendedEntities?.Hashtags
                );

                _hashtags = new List<IHashtagEntity>(allHashtags.Distinct((x, y) => x.Equals(y)));
                
                // SYMBOLS
                var allSymbols = _tweetDTOEntities.Symbols.SafeConcat
                (
                    _tweetDTOLegacyEntities.Symbols,
                    _tweetDTO?.ExtendedTweet?.LegacyEntities?.Symbols,
                    _tweetDTO?.ExtendedTweet?.ExtendedEntities?.Symbols
                );

                _symbols = new List<ISymbolEntity>(allSymbols.Distinct((x, y) => x.Equals(y)));

                // MEDIAS
                var allMedias = new List<IMediaEntity>().SafeConcat
                (
                    _tweetDTO?.ExtendedTweet?.LegacyEntities?.Medias,
                    _tweetDTO?.ExtendedTweet?.ExtendedEntities?.Medias
                );

                _medias = new List<IMediaEntity>(allMedias.Distinct((x, y) => x.Equals(y)));
            }
            else
            {
                var allURLs = _tweetDTOEntities.Urls.SafeConcat(_tweetDTOLegacyEntities.Urls);
                _urls = new List<IUrlEntity>(allURLs.Distinct((x, y) => x.Equals(y)));

                var allUserMentions = _tweetDTOEntities.UserMentions.SafeConcat(_tweetDTOLegacyEntities.UserMentions);
                _userMentions = new List<IUserMentionEntity>(allUserMentions.Distinct((x, y) => x.Equals(y)));

                var allHashtags = _tweetDTOEntities.Hashtags.SafeConcat(_tweetDTOLegacyEntities.Hashtags);
                _hashtags = new List<IHashtagEntity>(allHashtags.Distinct((x, y) => x.Equals(y)));

                var allSymbols = _tweetDTOEntities.Symbols.SafeConcat(_tweetDTOLegacyEntities.Symbols);
                _symbols = new List<ISymbolEntity>(allSymbols.Distinct((x, y) => x.Equals(y)));

                var allMedias = _tweetDTOEntities.Medias.SafeConcat(_tweetDTOLegacyEntities.Medias);
                _medias = new List<IMediaEntity>(allMedias.Distinct((x, y) => x.Equals(y)));
            }
        }

        private ITweetEntities _tweetDTOEntities
        {
            get
            {
                if (_tweetDTO == null || _tweetDTO.Entities == null)
                {
                    return new TweetEntitiesDTO();
                }

                return _tweetDTO.Entities;
            }
        }

        private ITweetEntities _tweetDTOLegacyEntities
        {
            get
            {
                if (_tweetDTO == null || _tweetDTO.LegacyEntities == null)
                {
                    return new TweetEntitiesDTO();
                }

                return _tweetDTO.LegacyEntities;
            }
        }

        private List<IUrlEntity> _urls;
        public List<IUrlEntity> Urls
        {
            get { return _urls; }
        }

        private List<IUserMentionEntity> _userMentions;
        public List<IUserMentionEntity> UserMentions
        {
            get { return _userMentions; }
        }

        private List<IHashtagEntity> _hashtags;
        public List<IHashtagEntity> Hashtags
        {
            get { return _hashtags; }
        }

        private List<ISymbolEntity> _symbols;
        public List<ISymbolEntity> Symbols
        {
            get { return _symbols; }
        }

        private List<IMediaEntity> _medias;
        public List<IMediaEntity> Medias
        {
            get { return _medias; }
        }
    }
}