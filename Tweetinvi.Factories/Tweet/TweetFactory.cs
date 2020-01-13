using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Tweet
{
    public class TweetFactory : ITweetFactory
    {
        private readonly ITwitterClientFactories _factories;

        public TweetFactory(ITwitterClientFactories factories)
        {
            _factories = factories;
        }

        public ITweet[] GenerateTweetsFromDTO(IEnumerable<ITweetDTO> tweetsDTO, TweetMode? tweetMode, ITwitterClient client)
        {
            if (tweetsDTO == null)
            {
                return null;
            }

            var tweets = new List<ITweet>();
            var tweetsDTOArray = tweetsDTO.ToArray();

            for (var i = 0; i < tweetsDTOArray.Length; ++i)
            {
                var tweet = _factories.CreateTweet(tweetsDTOArray[i]);

                if (tweet != null)
                {
                    tweets.Add(tweet);
                }
            }

            return tweets.ToArray();
        }
    }
}