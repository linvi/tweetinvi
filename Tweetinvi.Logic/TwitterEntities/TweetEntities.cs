using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    internal class TweetEntities : ITweetEntities
    {
        private readonly ITweetDTO _tweetDTO;

        public TweetEntities(ITweetDTO tweetDTO, TweetMode tweetMode)
        {
            _tweetDTO = tweetDTO;

            InitializeEntities(tweetMode);
        }

        private void InitializeEntities(TweetMode tweetMode)
        {
            // NOTE: The STREAMING API and REST API does not provide the same JSON structure based on the TweetMode used.
            // 
            // * STREAMING API : Adds a new ExtendedTweet regardless of the TweetMode. To have some consistency with the REST API,
            //   we decided that in COMPAT mode, the Entities will be restricted to what is available in the REST API.
            // * REST API : Adds FullText and additional properties if the TweetMode is extended.

            bool isTweetComingFromStreamingAPI = _tweetDTO?.ExtendedTweet != null;
            bool useStreamingApiExtendedTweetForEntities = tweetMode == TweetMode.Extended && isTweetComingFromStreamingAPI;

            // Get the entities and extended_entities for whichever Tweet DTO we're using
            var entities = useStreamingApiExtendedTweetForEntities ? _tweetDTO.ExtendedTweet.LegacyEntities : _tweetDTO?.LegacyEntities;
            var extendedEntities = useStreamingApiExtendedTweetForEntities ? _tweetDTO.ExtendedTweet.ExtendedEntities : _tweetDTO?.Entities;

            // Populate for each type of entity.

            _urls = entities?.Urls;
            _userMentions = entities?.UserMentions;
            _hashtags = entities?.Hashtags;
            _symbols = entities?.Symbols;

            // Media can also be in the extended_entities field. https://dev.twitter.com/overview/api/entities-in-twitter-objects#extended_entities
            // If that's populated, we must use it instead or risk missing media
            _medias = extendedEntities?.Medias ?? entities?.Medias ?? new List<IMediaEntity>();

            // If this is a retweet, it's also now possible for an entity to get cut off of the end of the tweet entirely.
            // If the same Tweet is fetched over the REST API, these entities get excluded, so lets do the same.
            if (_tweetDTO?.RetweetedTweetDTO != null)
            {
                _urls = _urls?.Where(e => e.Indices[0] != e.Indices[1]).ToList();
                _userMentions = _userMentions?.Where(e => e.Indices[0] != e.Indices[1]).ToList();
                _hashtags = _hashtags?.Where(e => e.Indices[0] != e.Indices[1]).ToList();
                _symbols = _symbols?.Where(e => e.Indices[0] != e.Indices[1]).ToList();
                _medias = _medias?.Where(e => e.Indices[0] != e.Indices[1]).ToList();
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