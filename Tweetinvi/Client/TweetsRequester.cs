using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Web;
using Tweetinvi.Exceptions;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Interfaces;

namespace Tweetinvi.Client
{
    public interface ITweetsRequester
    {
        Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId);
        Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds);
    }

    public interface IInternalTweetsRequester : ITweetsRequester
    {
        void Initialize(TwitterClient client);
    }

    public class TweetsRequester : IInternalTweetsRequester
    {
        private readonly ITweetFactory _tweetFactory;
        private TwitterClient _client;

        public TweetsRequester(ITweetFactory tweetFactory)
        {
            _tweetFactory = tweetFactory;
        }

        public void Initialize(TwitterClient client)
        {
            if (_client != null)
            {
                throw new InvalidOperationException("Client cannot be changed");
            }

            _client = client;
        }

        public Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(long tweetId)
        {
            var request = _client.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweet(tweetId, request), request);
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds)
        {
            var request = _client.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweets(tweetIds, request), request);
        }

        private async Task<T> ExecuteRequest<T>(Func<Task<T>> action, ITwitterRequest request)
        {
            try
            {
                return await action();
            }
            catch (Exception e)
            {
                throw new TwitterRequestException(request, e);
            }
        }
    }
}
