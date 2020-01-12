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
        public Task<ITwitterResult<ITweetDTO, ITweet>> GetTweet(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _tweetController.GetTweet(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dto => _tweetFactory.GenerateTweetFromDTO(dto, request.ExecutionContext.TweetMode, TwitterClient));
            });
        }

        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetTweets(IGetTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _tweetController.GetTweets(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, dtos => _tweetFactory.GenerateTweetsFromDTO(dtos, request.ExecutionContext.TweetMode, TwitterClient).ToArray());
            });
        }

        // Tweets - Publish
        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishTweet(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _tweetController.PublishTweet(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, TwitterClient));
            });
        }

        // Tweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.DestroyTweet(parameters, request));
        }

        // Retweets
        public Task<ITwitterResult<ITweetDTO[], ITweet[]>> GetRetweets(IGetRetweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var retweetsDTO = await _tweetController.GetRetweets(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(retweetsDTO, tweetDTOs => _tweetFactory.GenerateTweetsFromDTO(tweetDTOs, request.ExecutionContext.TweetMode, TwitterClient));
            });
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO, ITweet>> PublishRetweet(IPublishRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(async request =>
            {
                var twitterResult = await _tweetController.PublishRetweet(parameters, request).ConfigureAwait(false);
                return _twitterResultFactory.Create(twitterResult, tweetDTO => _tweetFactory.GenerateTweetFromDTO(tweetDTO, request.ExecutionContext.TweetMode, TwitterClient));
            });
        }

        // Retweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyRetweet(IDestroyRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            return ExecuteRequest(request => _tweetController.DestroyRetweet(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _tweetController.GetRetweeterIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweets(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _tweetController.GetFavoriteTweetsIterator(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweet(IFavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.FavoriteTweet(parameters, request));
        }

        public Task<ITwitterResult<ITweetDTO>> UnFavoriteTweet(IUnFavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.UnFavoriteTweet(parameters, request));
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.GetOEmbedTweet(parameters, request));
        }
    }
}