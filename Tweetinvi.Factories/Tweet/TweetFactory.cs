using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core;
using Tweetinvi.Core.DTO;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public class TweetFactory : ITweetFactory
    {
        private readonly IFactory<ITweetWithSearchMetadata> _tweetWithSearchMetadataFactory;
        private readonly IFactory<IMention> _mentionUnityFactory;
        private readonly IFactory<IOEmbedTweet> _oembedTweetUnityFactory;
        private readonly IJsonObjectConverter _jsonObjectConverter;
        private readonly ITweetinviSettingsAccessor _tweetinviSettingsAccessor;
        private readonly IUserFactory _userFactory;

        public TweetFactory(
            IFactory<ITweetWithSearchMetadata> tweetWithSearchMetadataFactory,
            IFactory<IMention> mentionFactory,
            IFactory<IOEmbedTweet> oembedTweetUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            IUserFactory userFactory)
        {
            _tweetWithSearchMetadataFactory = tweetWithSearchMetadataFactory;
            _mentionUnityFactory = mentionFactory;
            _oembedTweetUnityFactory = oembedTweetUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _userFactory = userFactory;
        }

        // Create Tweet

        public ITweet GenerateTweetFromJson(string json)
        {
            return CreateTweet(json, null, null);
        }

        public ITweet CreateTweet(string text, TweetMode? tweetMode, ITwitterClient client)
        {
            var tweetDTO = new TweetDTO
            {
                Text = text
            };

            tweetMode = tweetMode ?? _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode;

            return GenerateTweetFromDTO(tweetDTO, tweetMode, client);
        }

        // Generate Tweet from Json
        public ITweet GenerateTweetFromJson(string json, TweetMode? tweetMode, ITwitterClient client)
        {
            var tweetDTO = _jsonObjectConverter.DeserializeObject<ITweetDTO>(json);
            if (tweetDTO == null || tweetDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateTweetFromDTO(tweetDTO, tweetMode, client);
        }

        // Generate Tweet From DTO
        public ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO, TweetMode? tweetMode, ITwitterClient client)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var tweet = new Core.Models.Tweet(tweetDTO, tweetMode, this, _userFactory)
            {
                Client = client
            };

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

        public ITweet[] GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode, ITwitterClient client)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            var tweets = new List<ITweet>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (int i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = GenerateTweetFromDTO(tweetsDTOArray[i], tweetMode, client);

                if (tweet != null)
                {
                    tweets.Add(tweet);
                }
            }

            return tweets.ToArray();
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
            return tweetsDTO?.Select(GenerateMentionFromDTO).ToList();
        }
    }
}