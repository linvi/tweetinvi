using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITweetsClientParametersValidator
    {
        void Validate(IGetTweetParameters parameters);
        void Validate(IGetTweetsParameters parameters);
        void Validate(IPublishTweetParameters parameters);
        void Validate(IDestroyTweetParameters parameters);

        void Validate(IGetFavoriteTweetsParameters parameters);

        void Validate(IGetRetweetsParameters parameters);
        void Validate(IPublishRetweetParameters parameters);
        void Validate(IDestroyRetweetParameters parameters);
        void Validate(IGetRetweeterIdsParameters parameters);

        void Validate(IFavoriteTweetParameters parameters);
        void Validate(IUnFavoriteTweetParameters parameters);
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

        private TwitterLimits Limits => _client.ClientSettings.Limits;

        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }

        public void Validate(IGetTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IPublishTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = parameters.PageSize;
            if (maxPageSize > Limits.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TWEETS_GET_FAVORITE_TWEETS_MAX_SIZE), "page size");
            }
        }

        public void Validate(IGetRetweetsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = parameters.PageSize;
            if (maxPageSize > Limits.TWEETS_GET_RETWEETS_MAX_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TWEETS_GET_RETWEETS_MAX_SIZE), "page size");
            }
        }

        public void Validate(IPublishRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IDestroyRetweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRetweeterIdsParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = parameters.PageSize;
            if (maxPageSize > Limits.TWEETS_GET_RETWEETER_IDS_MAX_PAGE_SIZE)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TWEETS_GET_RETWEETS_MAX_SIZE), "page size");
            }
        }

        public void Validate(IFavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUnFavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }
    }
}