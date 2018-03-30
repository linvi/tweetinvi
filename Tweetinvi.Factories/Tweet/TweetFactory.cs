using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public class TweetFactory : ITweetFactory
    {
        private readonly ITweetFactoryQueryExecutor _tweetDTOFactory;
        private readonly IFactory<ITweet> _tweetUnityFactory;
        private readonly IFactory<ITweetWithSearchMetadata> _tweetWithSearchMetadataFactory;
        private readonly IFactory<IMention> _mentionUnityFactory;
        private readonly IFactory<IOEmbedTweet> _oembedTweetUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;

        public TweetFactory(
            ITweetFactoryQueryExecutor tweetDTOFactory,
            IFactory<ITweet> tweetUnityFactory,
            IFactory<ITweetWithSearchMetadata> tweetWithSearchMetadataFactory,
            IFactory<IMention> mentionFactory,
            IFactory<IOEmbedTweet> oembedTweetUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor)
        {
            _tweetDTOFactory = tweetDTOFactory;
            _tweetUnityFactory = tweetUnityFactory;
            _tweetWithSearchMetadataFactory = tweetWithSearchMetadataFactory;
            _mentionUnityFactory = mentionFactory;
            _oembedTweetUnityFactory = oembedTweetUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
        }

        // Get Tweet
        public ITweet GetTweet(long tweetId, TweetMode? tweetMode = null)
        {
            var tweetDTO = _tweetDTOFactory.GetTweetDTO(tweetId);
            return GenerateTweetFromDTO(tweetDTO, tweetMode);
        }

        public IEnumerable<ITweet> GetTweets(IEnumerable<long> tweetIds, TweetMode? tweetMode = null)
        {
            var tweetDTOs = _tweetDTOFactory.GetTweetDTOs(tweetIds);
            return GenerateTweetsFromDTO(tweetDTOs);
        }

        // Create Tweet

        public ITweet GenerateTweetFromJson(string json)
        {
            return CreateTweet(json, null);
        }

        public ITweet CreateTweet(string text, TweetMode? tweetMode)
        {
            var tweetDTO = _tweetDTOFactory.CreateTweetDTO(text);

            tweetMode = tweetMode ?? _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode;

            return GenerateTweetFromDTO(tweetDTO, (TweetMode)tweetMode);
        }

        // Generate Tweet from Json
        public ITweet GenerateTweetFromJson(string json, TweetMode? tweetMode = null)
        {
            var tweetDTO = _jsonObjectConverter.DeserializeObject<ITweetDTO>(json);
            if (tweetDTO == null || tweetDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateTweetFromDTO(tweetDTO, tweetMode);
        }

        // Generate Tweet From DTO
        public ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO, TweetMode? tweetMode = null)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var tweetDTOParameter = _tweetUnityFactory.GenerateParameterOverrideWrapper("tweetDTO", tweetDTO);
            var tweetModeParameter = _tweetUnityFactory.GenerateParameterOverrideWrapper("tweetMode", tweetMode ?? _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode);

            var tweet = _tweetUnityFactory.Create(tweetDTOParameter, tweetModeParameter);

            return tweet;
        }

        public ITweetWithSearchMetadata GenerateTweetWithSearchMetadataFromDTO(ITweetWithSearchMetadataDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var parameterOverride = _tweetWithSearchMetadataFactory.GenerateParameterOverrideWrapper("tweetDTO", tweetDTO);
            var tweet = _tweetWithSearchMetadataFactory.Create(parameterOverride);

            return tweet;
        }

        public IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode = null)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            List<ITweet> tweets = new List<ITweet>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (int i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = GenerateTweetFromDTO(tweetsDTOArray[i], tweetMode);
                if (tweet != null)
                {
                    tweets.Add(tweet);
                }
            }

            return tweets;
        }

        public IEnumerable<ITweetWithSearchMetadata> GenerateTweetsWithSearchMetadataFromDTOs(IEnumerable<ITweetWithSearchMetadataDTO> tweetsDTO)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            List<ITweetWithSearchMetadata> tweets = new List<ITweetWithSearchMetadata>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (int i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = GenerateTweetWithSearchMetadataFromDTO(tweetsDTOArray[i]);
                if (tweet != null)
                {
                    tweets.Add(tweet);
                }
            }

            return tweets;
        }

        // Generate OEmbedTweet from DTO
        public IOEmbedTweet GenerateOEmbedTweetFromDTO(IOEmbedTweetDTO oEmbedTweetDTO)
        {
            if (oEmbedTweetDTO == null)
            {
                return null;
            }

            var parameterOverride = _mentionUnityFactory.GenerateParameterOverrideWrapper("oEmbedTweetDTO", oEmbedTweetDTO);
            var oEmbedTweet = _oembedTweetUnityFactory.Create(parameterOverride);

            return oEmbedTweet;
        }

        public IOEmbedTweet GenerateOEmbedTweetFromJson(string json)
        {
            var dto = _jsonObjectConverter.DeserializeObject<IOEmbedTweetDTO>(json);
            return GenerateOEmbedTweetFromDTO(dto);
        }

        // Generate Mention from DTO
        public IMention GenerateMentionFromDTO(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var parameterOverride = _mentionUnityFactory.GenerateParameterOverrideWrapper("tweetDTO", tweetDTO);
            var mention = _mentionUnityFactory.Create(parameterOverride);

            return mention;
        }

        public IEnumerable<IMention> GenerateMentionsFromDTO(IEnumerable<ITweetDTO> tweetsDTO)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            return tweetsDTO.Select(GenerateMentionFromDTO).ToList();
        }
    }
}