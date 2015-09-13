using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface ITweetFactory
    {
        ITweet GetTweet(long tweetId);
        IEnumerable<ITweet> GetTweets(IEnumerable<long> tweetIds);

        ITweet CreateTweet(string text);

        // Generate Tweet From Json
        ITweet GenerateTweetFromJson(string jsonTweet);

        // Generate Tweet from DTO
        ITweet GenerateTweetFromDTO(ITweetDTO tweetDTO);
        IEnumerable<ITweet> GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO);

        ITweetWithSearchMetadata GenerateTweetWithSearchMetadataFromDTO(ITweetWithSearchMetadataDTO tweetDTO);
        IEnumerable<ITweetWithSearchMetadata> GenerateTweetsWithSearchMetadataFromDTOs(IEnumerable<ITweetWithSearchMetadataDTO> tweetsDTO);

        // Generate OEmbedTweet from DTO
        IOEmbedTweet GenerateOEmbedTweetFromDTO(IOEmbedTweetDTO oEmbedTweetDTO);

        // Generate Mention from DTO
        IMention GenerateMentionFromDTO(ITweetDTO tweetDTO);
        IEnumerable<IMention> GenerateMentionsFromDTO(IEnumerable<ITweetDTO> tweetsDTO);
    }
}