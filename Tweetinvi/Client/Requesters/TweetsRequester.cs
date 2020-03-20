using System.Threading.Tasks;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.JsonConverters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class TweetsRequester : BaseRequester, ITweetsRequester
    {
        private readonly ITweetController _tweetController;
        private readonly ITweetsClientRequiredParametersValidator _tweetsClientRequiredParametersValidator;

        public TweetsRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITweetController tweetController,
            ITweetsClientRequiredParametersValidator tweetsClientRequiredParametersValidator)
        : base(client, clientEvents)
        {
            _tweetController = tweetController;
            _tweetsClientRequiredParametersValidator = tweetsClientRequiredParametersValidator;
        }

        // Tweets
        public Task<ITwitterResult<ITweetDTO>> GetTweet(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.GetTweet(parameters, request));
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweets(IGetTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.GetTweets(parameters, request));
        }

        // Tweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishTweet(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.PublishTweet(parameters, request));
        }

        // Tweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyTweet(IDestroyTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.DestroyTweet(parameters, request));
        }

        // Retweets
        public Task<ITwitterResult<ITweetDTO[]>> GetRetweets(IGetRetweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.GetRetweets(parameters, request));
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishRetweet(IPublishRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.PublishRetweet(parameters, request));
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

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetFavoriteTweetsIterator(IGetFavoriteTweetsParameters parameters)
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

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweet(IUnfavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.UnfavoriteTweet(parameters, request));
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweet(IGetOEmbedTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequest(request => _tweetController.GetOEmbedTweet(parameters, request));
        }
    }
}