using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class TweetsClient : ITweetsClient
    {
        private readonly TwitterClient _client;
        private readonly ITweetsRequester _tweetsRequester;
        private readonly ITweetFactory _tweetFactory;

        public TweetsClient(TwitterClient client)
        {
            _client = client;
            _tweetsRequester = client.RequestExecutor.Tweets;
            _tweetFactory = TweetinviContainer.Resolve<ITweetFactory>();
        }
        
        public ITweetsClientParametersValidator ParametersValidator => _client.ParametersValidator;

        // Tweets

        public Task<ITweet> GetTweet(long? tweetId)
        {
            return GetTweet(new GetTweetParameters(tweetId));
        }

        public async Task<ITweet> GetTweet(IGetTweetParameters parameters)
        {
            var twitterResult = await _tweetsRequester.GetTweet(parameters).ConfigureAwait(false);
            return twitterResult?.Result;
        }

        /// <summary>
        /// Get multiple tweets
        /// </summary>
        /// <returns>The specified tweets</returns>
        public async Task<ITweet[]> GetTweets(long[] tweetIds)
        {
            var requestResult = await _tweetsRequester.GetTweets(tweetIds).ConfigureAwait(false);
            return requestResult?.Result;
        }

        // Tweets - Publish

        public Task<ITweet> PublishTweet(string text)
        {
            return PublishTweet(new PublishTweetParameters(text));
        }

        public async Task<ITweet> PublishTweet(IPublishTweetParameters parameters)
        {
            var requestResult = await _tweetsRequester.PublishTweet(parameters).ConfigureAwait(false);
            return requestResult?.Result;
        }

        // Tweets - Destroy

        public Task<bool> DestroyTweet(long? tweetId)
        {
            return DestroyTweet(new DestroyTweetParameters(tweetId));
        }

        public Task<bool> DestroyTweet(ITweetIdentifier tweet)
        {
            return DestroyTweet(new DestroyTweetParameters(tweet));
        }

        public Task<bool> DestroyTweet(ITweet tweet)
        {
            return DestroyTweet(tweet.TweetDTO);
        }
        
        public async Task<bool> DestroyTweet(ITweetDTO tweet)
        {
            var tweetDestroyed = await DestroyTweet(new DestroyTweetParameters(tweet)).ConfigureAwait(false);
            
            if (tweetDestroyed)
            {
                tweet.IsTweetDestroyed = true;
            }

            return tweetDestroyed;
        }

        public async Task<bool> DestroyTweet(IDestroyTweetParameters parameters)
        {
            var twitterResult = await _tweetsRequester.DestroyTweet(parameters).ConfigureAwait(false);
            return twitterResult.Response.IsSuccessStatusCode;
        }

        // Retweets

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(long tweetId)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null).ConfigureAwait(false);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(long tweetId, int maxNumberOfTweetsToRetrieve)
        {
            var tweetIdentifier = new TweetIdentifier(tweetId);
            var requestResult = await _tweetsRequester.GetRetweets(tweetIdentifier, null).ConfigureAwait(false);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, null).ConfigureAwait(false);
            return requestResult?.Result;
        }

        /// <summary>
        /// Get the retweets associated with a specific tweet 
        /// </summary>
        /// <returns>Retweets</returns>
        public async Task<ITweet[]> GetRetweets(ITweetIdentifier tweet, int maxNumberOfTweetsToRetrieve)
        {
            var requestResult = await _tweetsRequester.GetRetweets(tweet, maxNumberOfTweetsToRetrieve).ConfigureAwait(false);
            return requestResult?.Result;
        }

        // Retweets - Publish

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        public async Task<ITweet> PublishRetweet(long tweetId)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(new TweetIdentifier(tweetId)).ConfigureAwait(false);
            return requestResult?.Result;
        }

        /// <summary>
        /// Publish a retweet 
        /// </summary>
        /// <returns>The retweet</returns>
        public async Task<ITweet> PublishRetweet(ITweetIdentifier tweet)
        {
            var requestResult = await _tweetsRequester.PublishRetweet(tweet).ConfigureAwait(false);
            return requestResult?.Result;
        }

        // Retweets - Destroy

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        public async Task<bool> UnRetweet(ITweetIdentifier retweet)
        {
            var requestResult = await _tweetsRequester.DestroyRetweet(retweet).ConfigureAwait(false);
            return requestResult?.Response?.IsSuccessStatusCode == true;
        }

        /// <summary>
        /// Destroy a retweet
        /// </summary>
        /// <returns>Whether the operation was a success</returns>
        public Task<bool> UnRetweet(long retweetId)
        {
            return UnRetweet(new TweetIdentifier(retweetId));
        }

        #region Favourite Tweets

        public ITwitterIterator<ITweet, long?> GetFavoriteTweets(long? userId)
        {
            return GetFavoriteTweets(new GetFavoriteTweetsParameters(userId));
        }

        public ITwitterIterator<ITweet, long?> GetFavoriteTweets(string username)
        {
            return GetFavoriteTweets(new GetFavoriteTweetsParameters(username));
        }

        public ITwitterIterator<ITweet, long?> GetFavoriteTweets(IUserIdentifier user)
        {
            return GetFavoriteTweets(new GetFavoriteTweetsParameters(user));
        }

        public ITwitterIterator<ITweet, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters)
        {
            var tweetMode = _client.Config.TweetMode;

            var favoriteTweetsIterator = _tweetsRequester.GetFavoriteTweets(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(favoriteTweetsIterator,
                twitterResult =>
                {
                    return _tweetFactory.GenerateTweetsFromDTO(twitterResult.DataTransferObject, tweetMode, _client);
                });
        }

        #endregion
    }
}
