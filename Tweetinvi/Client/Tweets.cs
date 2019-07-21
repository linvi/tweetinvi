﻿using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class Tweets
    {
        private readonly ITweetsRequester _tweetsRequester;

        public Tweets(TwitterClient client)
        {
            _tweetsRequester = client.RequestExecutor.Tweets;
        }

        // Tweets
        public async Task<ITweet> GetTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.GetTweet(tweetId);
            return requestResult?.Result;
        }

        public async Task<ITweet[]> GetTweets(long[] tweetIds)
        {
            var requestResult = await _tweetsRequester.GetTweets(tweetIds);
            return requestResult?.Result;
        }

        // Tweets - Publish
        public async Task<ITweet> PublishTweet(string text)
        {
            var requestResult = await _tweetsRequester.PublishTweet(text);
            return requestResult?.Result;
        }

        public async Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishTweet(parameters);
            return requestResult?.Result;
        }

        // Tweets - Destroy
        public async Task<bool> DestroyTweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweetId);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        public async Task<bool> DestroyTweet(ITweetDTO tweet)
        {
            var requestResult = await _tweetsRequester.DestroyTweet(tweet);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        public Task<bool> DestroyTweet(ITweet tweet)
        {
            return DestroyTweet(tweet?.TweetDTO);
        }

        // Retweets
        public async Task<ITweet[]> GetRetweets(long tweetId)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null);
            return requestResult?.Result;
        }

        public async Task<ITweet[]> GetRetweets(long tweetId, int maxNumberOfTweetsToRetrieve)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null);
            return requestResult?.Result;
        }

        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, null);
            return requestResult?.Result;
        }


        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet, int maxNumberOfTweetsToRetrieve)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, maxNumberOfTweetsToRetrieve);
            return requestResult?.Result;
        }

        // Retweets - Publish
        public async Task<ITweet> PublishRetweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweetId);
            return requestResult?.Result;
        }

        public async Task<ITweet> PublishRetweet(ITweetDTO tweet)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweet);
            return requestResult?.Result;
        }

        public Task<ITweet> PublishRetweet(ITweet tweet)
        {
            return PublishRetweet(tweet?.TweetDTO);
        }

    }
}
