using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface ITweetsRequester
    {
        // Tweets
        Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId);
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds);

        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(string text);
        Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters);

        Task<ITwitterResult> DestroyTweet(long tweetId);
        Task<ITwitterResult> DestroyTweet(ITweetDTO tweet);

        // Retweets
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(ITweetIdentifier tweet, int? maxRetweetsToRetrieve);

        Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(ITweetIdentifier tweet);
        Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweetId);
    }

    public interface IInternalTweetsRequester : ITweetsRequester
    {
        void Initialize(ITwitterClient client);
    }

    public class TweetsRequester : BaseRequester, IInternalTweetsRequester
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITweetController _tweetController;
        private ITwitterClient _twitterClient;

        public TweetsRequester(
            ITweetFactory tweetFactory,
            ITweetController tweetController)
        {
            _tweetFactory = tweetFactory;
            _tweetController = tweetController;
        }

        public void Initialize(ITwitterClient client)
        {
            if (_twitterClient != null)
            {
                throw new InvalidOperationException("createRequest cannot be changed");
            }

            _twitterClient = client;
        }

        // Tweets
        public Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweet(tweetId, request), request);
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweets(tweetIds, request), request);
        }

        // Tweets - Publish
        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(string text)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.PublishTweet(text, request), request);
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.PublishTweet(parameters, request), request);
        }

        // Tweets - Destroy
        public Task<ITwitterResult> DestroyTweet(long tweetId)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyTweet(tweetId, request), request);
        }

        public Task<ITwitterResult> DestroyTweet(ITweetDTO tweet)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyTweet(tweet, request), request);
        }

        // Retweets
        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(ITweetIdentifier tweet, int? maxRetweetsToRetrieve)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.GetRetweets(tweet, maxRetweetsToRetrieve, request), request);
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(ITweetIdentifier tweet)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.PublishRetweet(tweet, request), request);
        }

        // Retweets - Destroy
        public Task<ITwitterResult> DestroyRetweet(ITweetIdentifier retweetId)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyRetweet(retweetId, request), request);
        }

        // Factories
    }
}
