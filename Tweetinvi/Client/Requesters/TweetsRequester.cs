using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
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
            var twitterResult = await ExecuteRequest(() => _tweetController.GetTweet(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(twitterResult, dto => _tweetFactory.GenerateTweetFromDTO(dto, request.ExecutionContext.TweetMode, _twitterClient));
        }

        public async Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(IGetTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _tweetController.GetTweets(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(twitterResult, dtos => _tweetFactory.GenerateTweetsFromDTO(dtos, request.ExecutionContext.TweetMode, _twitterClient).ToArray());
        }

        // Tweets - Publish
        public async Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _tweetController.PublishTweet(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(twitterResult, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, _twitterClient));
        }

        // Tweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyTweet(parameters, request), request);
        }

        // Retweets
        public async Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(IGetRetweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var retweetsDTO = await ExecuteRequest(() => _tweetController.GetRetweets(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(retweetsDTO, tweetDTOs => _tweetFactory.GenerateTweetsFromDTO(tweetDTOs, request.ExecutionContext.TweetMode, _twitterClient));
        }

        // Retweets - Publish
        public async Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(IPublishRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            var twitterResult = await ExecuteRequest(() => _tweetController.PublishRetweet(parameters, request), request).ConfigureAwait(false);
            return _twitterResultFactory.Create(twitterResult, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, _twitterClient));
        }

        // Retweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return ExecuteRequest(() => _tweetController.DestroyRetweet(parameters, request), request);
        }
        
        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIds(IGetRetweeterIdsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _tweetController.GetRetweeterIds(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var request = _twitterClient.CreateRequest();
            return _tweetController.GetFavoriteTweets(parameters, request);
        }
    }
}
