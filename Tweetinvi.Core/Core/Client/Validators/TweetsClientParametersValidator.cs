using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITweetsClientParametersValidator
    {
        void Validate(IGetTweetParameters parameters);
        void Validate(IPublishTweetParameters parameters);
        void Validate(IGetFavoriteTweetsParameters parameters);
    }
    
    public interface IInternalTweetsClientParametersValidator : ITweetsClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class TweetsClientParametersValidator : IInternalTweetsClientParametersValidator
    {
        private readonly ITweetsClientRequiredParametersValidator _tweetsClientRequiredParametersValidator;
        private ITwitterClient _client;

        public TweetsClientParametersValidator(ITweetsClientRequiredParametersValidator tweetsClientRequiredParametersValidator)
        {
            _tweetsClientRequiredParametersValidator = tweetsClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.Config.Limits;

        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }

        public void Validate(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = parameters.PageSize;
            if (maxPageSize > Limits.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"${nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE), "page size");
            }
        }
    }
}