using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core;
using Tweetinvi.Core.Client;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Helpers;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;

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
        private readonly ITwitterResultFactory _twitterResultFactory;

        public TweetFactory(
            ITweetFactoryQueryExecutor tweetDTOFactory,
            IFactory<ITweet> tweetUnityFactory,
            IFactory<ITweetWithSearchMetadata> tweetWithSearchMetadataFactory,
            IFactory<IMention> mentionFactory,
            IFactory<IOEmbedTweet> oembedTweetUnityFactory,
            IJsonObjectConverter jsonObjectConverter,
            ITweetinviSettingsAccessor tweetinviSettingsAccessor,
            ITwitterResultFactory twitterResultFactory)
        {
            _tweetDTOFactory = tweetDTOFactory;
            _tweetUnityFactory = tweetUnityFactory;
            _tweetWithSearchMetadataFactory = tweetWithSearchMetadataFactory;
            _mentionUnityFactory = mentionFactory;
            _oembedTweetUnityFactory = oembedTweetUnityFactory;
            _jsonObjectConverter = jsonObjectConverter;
            _tweetinviSettingsAccessor = tweetinviSettingsAccessor;
            _twitterResultFactory = twitterResultFactory;
        }

        // Get Tweet

        public async Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId, ITwitterRequest request)
        {
            var result = await _tweetDTOFactory.GetTweetDTO(tweetId, request);

            return _twitterResultFactory.Create(result, dto => GenerateTweetFromDTO(dto, request.ExecutionContext.TweetMode, request.ExecutionContext));
        }

        public async Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds, ITwitterRequest request)
        {
            var result = await _tweetDTOFactory.GetTweetDTOs(tweetIds, request);

            return _twitterResultFactory.Create(result, dtos => GenerateTweetsFromDTO(dtos, request.ExecutionContext.TweetMode, request.ExecutionContext).ToArray());
        }

        // Create Tweet

        public ITweet GenerateTweetFromJson(string json)
        {
            return CreateTweet(json, null, new TwitterExecutionContext());
        }

        public ITweet CreateTweet(string text, TweetMode? tweetMode, ITwitterExecutionContext executionContext)
        {
            var tweetDTO = _tweetDTOFactory.CreateTweetDTO(text);

            tweetMode = tweetMode ?? _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode;

            return GenerateTweetFromDTO(tweetDTO, tweetMode, executionContext);
        }

        // Generate Tweet from Json
        public ITweet GenerateTweetFromJson(string json, TweetMode? tweetMode, ITwitterExecutionContext executionContext)
        {
            var tweetDTO = _jsonObjectConverter.DeserializeObject<ITweetDTO>(json);
            if (tweetDTO == null || tweetDTO.Id == TweetinviSettings.DEFAULT_ID)
            {
                return null;
            }

            return GenerateTweetFromDTO(tweetDTO, tweetMode, executionContext);
        }

        // Generate Tweet From DTO
        public ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO, TweetMode? tweetMode, ITwitterExecutionContext executionContext)
        {
            if (tweetDTO == null)
            {
                return null;
            }

            var context = executionContext ?? new TwitterExecutionContext();

            var tweetDTOParameter = _tweetUnityFactory.GenerateParameterOverrideWrapper("tweetDTO", tweetDTO);
            var tweetModeParameter = _tweetUnityFactory.GenerateParameterOverrideWrapper("tweetMode", tweetMode ?? executionContext?.TweetMode ?? _tweetinviSettingsAccessor.CurrentThreadSettings.TweetMode);
            var twitterRequestFactoryParameter = _tweetUnityFactory.GenerateParameterOverrideWrapper("executionContext", context);

            var tweet = _tweetUnityFactory.Create(tweetDTOParameter, tweetModeParameter, twitterRequestFactoryParameter);

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

        public IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode, ITwitterExecutionContext executionContext)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            List<ITweet> tweets = new List<ITweet>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (int i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = GenerateTweetFromDTO(tweetsDTOArray[i], tweetMode, executionContext);
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
            return tweetsDTO?.Select(GenerateMentionFromDTO).ToList();
        }
    }
}