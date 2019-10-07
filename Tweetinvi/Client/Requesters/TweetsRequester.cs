using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalTweetsRequester : ITweetsRequester, IBaseRequester
    {
    }

    public class TweetsRequester : BaseRequester, IInternalTweetsRequester
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITweetController _tweetController;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly ITweetsClientRequiredParametersValidator _tweetsClientRequiredParametersValidator;

        public TweetsRequester(
            ITweetFactory tweetFactory,
            ITweetController tweetController,
            ITwitterResultFactory twitterResultFactory,
            ITweetsClientRequiredParametersValidator tweetsClientRequiredParametersValidator)
        {
            _tweetFactory = tweetFactory;
            _tweetController = tweetController;
            _twitterResultFactory = twitterResultFactory;
            _tweetsClientRequiredParametersValidator = tweetsClientRequiredParametersValidator;
        }

        // Tweets
        public async Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _tweetController.GetTweet(parameters, request), request);
            return _twitterResultFactory.Create(twitterResult, dto => _tweetFactory.GenerateTweetFromDTO(dto, request.ExecutionContext));
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(long[] tweetIds)
        {
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetFactory.GetTweets(tweetIds, request), request);
        }

        // Tweets - Publish
        public async Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _tweetController.PublishTweet(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(twitterResult, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, request.ExecutionContext));
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

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return _tweetController.GetFavoriteTweets(parameters, request);
        }
    }
}
