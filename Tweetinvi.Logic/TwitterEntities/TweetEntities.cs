using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace Tweetinvi.Logic.TwitterEntities
{
    internal class TweetEntities : ITweetEntities
    {
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly ITweetDTO _tweetDTO;

        public TweetEntities(
            ITweetinviSettingsAccessor tweetinviSettingsAccessor, 
            ITweetDTO tweetDTO)
        {
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _tweetDTO = tweetDTO;

            InitializeEntities();
        }

        private void InitializeEntities()
        {
            // Populate the entities with extended ones if this thread is running in Extended Tweet Mode
            bool populateExtendedTweetEntities = _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode ==
                                                 TweetMode.Extended;

            // Was this Tweet received over the streaming API & is an extended Tweet
            bool hasStreamingApiExtendedTweet = _tweetDTO?.ExtendedTweet != null;

            // Should we use the (streaming API) extended tweet data?
            bool useStreamingApiExtendedTweetForEntities = populateExtendedTweetEntities && hasStreamingApiExtendedTweet;

            // Get the entities and extended_entities for whichever Tweet DTO we're using
            ITweetEntities entities = useStreamingApiExtendedTweetForEntities
                ? _tweetDTO.ExtendedTweet.LegacyEntities
                : _tweetDTO?.LegacyEntities;
            ITweetEntities extendedEntities = useStreamingApiExtendedTweetForEntities
                ? _tweetDTO.ExtendedTweet.ExtendedEntities
                : _tweetDTO?.Entities;

            // Populate for each type of entity.
            _urls = entities?.Urls;
            _userMentions = entities?.UserMentions;
            _hashtags = entities?.Hashtags;
            _symbols = entities?.Symbols;

            // Media can also be in the extended_entities field. https://dev.twitter.com/overview/api/entities-in-twitter-objects#extended_entities
            //  If that's populated, we must use it instead or risk missing media
            _medias = extendedEntities?.Medias ?? entities?.Medias;

            // If this is a retweet, it's also now possible for an entity to get cut off of the end of the tweet entirely.
            //  If the same Tweet is fetched over the REST API, these entities get excluded, so lets do the same.
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