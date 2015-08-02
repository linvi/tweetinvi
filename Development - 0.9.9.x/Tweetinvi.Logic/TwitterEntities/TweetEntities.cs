using System.Collections.Generic;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models.Entities;

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
