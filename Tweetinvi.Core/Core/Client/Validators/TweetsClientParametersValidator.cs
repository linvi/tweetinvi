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

        void Validate(IGetUserFavoriteTweetsParameters parameters);

        void Validate(IGetRetweetsParameters parameters);
        void Validate(IPublishRetweetParameters parameters);
        void Validate(IDestroyRetweetParameters parameters);
        void Validate(IGetRetweeterIdsParameters parameters);

        void Validate(IFavoriteTweetParameters parameters);
        void Validate(IUnfavoriteTweetParameters parameters);
        void Validate(IGetOEmbedTweetParameters parameters);
    }

    public class TweetsClientParametersValidator : ITweetsClientParametersValidator
    {
        private readonly ITweetsClientRequiredParametersValidator _tweetsClientRequiredParametersValidator;
        private readonly ITwitterClient _client;

        public TweetsClientParametersValidator(ITwitterClient client, ITweetsClientRequiredParametersValidator tweetsClientRequiredParametersValidator)
        {
            _client = client;
            _tweetsClientRequiredParametersValidator = tweetsClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.ClientSettings.Limits;

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

        public void Validate(IGetUserFavoriteTweetsParameters parameters)
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

        public void Validate(IUnfavoriteTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetOEmbedTweetParameters parameters)
        {
            _tweetsClientRequiredParametersValidator.Validate(parameters);
        }
    }
}