using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Core.Factories
{
    public interface ITweetFactory
    {
        // Generate Tweet from DTO
        ITweet[] GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode, ITwitterClient client);
    }
}