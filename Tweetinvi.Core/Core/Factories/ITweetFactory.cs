using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Core.Factories
{
    public interface ITweetFactory
    {
        Task<ITwitterResult<TweetDTO, ITweet>> GetTweet(long tweetId, TweetMode? tweetMode, ITwitterRequest request);
        Task<IEnumerable<ITweet>> GetTweets(IEnumerable<long> tweetIds, TweetMode? tweetMode = null);

        ITweet CreateTweet(string text, TweetMode? tweetMode = null);

        // Generate Tweet From Json
        ITweet GenerateTweetFromJson(string json);
        ITweet GenerateTweetFromJson(string json, TweetMode? tweetMode);

        // Generate Tweet from DTO
        ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO, TweetMode? tweetMode = null);
        IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode = null);

        ITweetWithSearchMetadata GenerateTweetWithSearchMetadataFromDTO(ITweetWithSearchMetadataDTO tweetDTO);
        IEnumerable<ITweetWithSearchMetadata> GenerateTweetsWithSearchMetadataFromDTOs(IEnumerable<ITweetWithSearchMetadataDTO> tweetsDTO);

        // Generate OEmbedTweet from DTO
        IOEmbedTweet GenerateOEmbedTweetFromDTO(IOEmbedTweetDTO oEmbedTweetDTO);

        // Generate Mention from DTO
        IMention GenerateMentionFromDTO(ITweetDTO tweetDTO);
        IEnumerable<IMention> GenerateMentionsFromDTO(IEnumerable<ITweetDTO> tweetsDTO);
        IOEmbedTweet GenerateOEmbedTweetFromJson(string json);
    }
}