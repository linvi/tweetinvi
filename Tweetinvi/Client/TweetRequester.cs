using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Web;
using Tweetinvi.Logic.DTO;
using Tweetinvi.Models;

namespace Tweetinvi.Client
{
    public interface ITweetRequester
    {
        Task<ITwitterResult<TweetDTO, ITweet>> GetTweet(long tweetId);
    }

    public interface IInternalTweetRequester : ITweetRequester
    {
        void Initialize(TwitterClient client);
    }

    public class TweetRequester : IInternalTweetRequester
    {
        private readonly ITweetFactory _tweetFactory;
        private TwitterClient _client;

        public TweetRequester(ITweetFactory tweetFactory)
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

        public Task<ITwitterResult<TweetDTO, ITweet>> GetTweet(long tweetId)
        {
            var request = _client.CreateRequest();
            return _tweetFactory.GetTweet(tweetId, _client.Config.TweetMode, request);
        }
    }
}
