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
        public Task<ITwitterResult<ITweetDTO>> GetTweetAsync(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.GetTweetAsync(parameters, request));
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetTweetsAsync(IGetTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.GetTweetsAsync(parameters, request));
        }

        // Tweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishTweetAsync(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.PublishTweetAsync(parameters, request));
        }

        // Tweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyTweetAsync(IDestroyTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.DestroyTweetAsync(parameters, request));
        }

        // Retweets
        public Task<ITwitterResult<ITweetDTO[]>> GetRetweetsAsync(IGetRetweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.GetRetweetsAsync(parameters, request));
        }

        // Retweets - Publish
        public Task<ITwitterResult<ITweetDTO>> PublishRetweetAsync(IPublishRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.PublishRetweetAsync(parameters, request));
        }

        // Retweets - Destroy
        public Task<ITwitterResult<ITweetDTO>> DestroyRetweetAsync(IDestroyRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.DestroyRetweetAsync(parameters, request));
        }

        public ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> GetRetweeterIdsIterator(IGetRetweeterIdsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            var request = TwitterClient.CreateRequest();
            request.ExecutionContext.Converters = JsonQueryConverterRepository.Converters;
            return _tweetController.GetRetweeterIdsIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetUserFavoriteTweetsIterator(IGetUserFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            var request = TwitterClient.CreateRequest();
            return _tweetController.GetFavoriteTweetsIterator(parameters, request);
        }

        public Task<ITwitterResult<ITweetDTO>> FavoriteTweetAsync(IFavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.FavoriteTweetAsync(parameters, request));
        }

        public Task<ITwitterResult<ITweetDTO>> UnfavoriteTweetAsync(IUnfavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.UnfavoriteTweetAsync(parameters, request));
        }

        public Task<ITwitterResult<IOEmbedTweetDTO>> GetOEmbedTweetAsync(IGetOEmbedTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            return ExecuteRequestAsync(request => _tweetController.GetOEmbedTweetAsync(parameters, request));
        }
    }
}