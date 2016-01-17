using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

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

        public TweetFactory(
            ITweetFactoryQueryExecutor tweetDTOFactory,
            IFactory<ITweet> tweetUnityFactory,
            IFactory<ITweetWithSearchMetadata> tweetWithSearchMetadataFactory,
            IFactory<IMention> mentionFactory,
            IFactory<IOEmbedTweet> oembedTweetUnityFactory,
            IJsonObjectConverter jsonObjectConverter)
        {
            _tweetDTOFactory = tweetDTOFactory;
            _tweetUnityFactory = tweetUnityFactory;
            _tweetWithSearchMetadataFactory = tweetWithSearchMetadataFactory;
            _mentionUnityFactory = mentionFactory;
            _oembedTweetUnityFactory = oembedTweetUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
        }

        // Get Tweet
        public ITweet GetTweet(long tweetId)
        {
            var tweetDTO = _tweetDTOFactory.GetTweetDTO(tweetId);
            return GenerateTweetFromDTO(tweetDTO);
        }

        public IEnumerable<ITweet> GetTweets(IEnumerable<long> tweetIds)
        {
            var tweetDTOs = _tweetDTOFactory.GetTweetDTOs(tweetIds);
            return GenerateTweetsFromDTO(tweetDTOs);
        }

        // Create Tweet
        public ITweet CreateTweet(string text)
        {
            var tweetDTO = _tweetDTOFactory.CreateTweetDTO(text);
            return GenerateTweetFromDTO(tweetDTO);
        }

        // Generate Tweet from Json
        public ITweet GenerateTweetFromJson(string jsonTweet)
        {
            var tweetDTO = _jsonObjectConverter.DeserializeObject<ITweetDTO>(jsonTweet);
            if (tweetDTO == null || tweetDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateTweetFromDTO(tweetDTO);
        }

        // Generate Tweet From DTO
        public ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var parameterOverride = _tweetUnityFactory.GenerateParameterOverrideWrapper("tweetDTO", tweetDTO);
            var tweet = _tweetUnityFactory.Create(parameterOverride);

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

        public IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            List<ITweet> tweets = new List<ITweet>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (int i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = GenerateTweetFromDTO(tweetsDTOArray[i]);
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