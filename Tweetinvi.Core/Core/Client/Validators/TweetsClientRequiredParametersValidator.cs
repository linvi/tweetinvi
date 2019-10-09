using System;
using System.Linq;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITweetsClientRequiredParametersValidator : ITweetsClientParametersValidator
    {
    }

    public class TweetsClientRequiredParametersValidator : ITweetsClientRequiredParametersValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly ITweetQueryValidator _tweetQueryValidator;

        public TweetsClientRequiredParametersValidator(
            IUserQueryValidator userQueryValidator, 
            ITweetQueryValidator tweetQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
            _tweetQueryValidator = tweetQueryValidator;
        }

        public void Validate(IGetTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(parameters.Tweet, $"{nameof(parameters)}.{nameof(parameters.Tweet)}");
        }

        public void Validate(IGetTweetsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Tweets == null)
            {
                throw new ArgumentNullException();
            }

            if (parameters.Tweets.Length == 0)
            {
                throw new ArgumentException("You need at least 1 tweet id", $"{nameof(parameters)}.{nameof(parameters.Tweets)}");
            }
            
            var validTweetIdentifiers = parameters.Tweets.Where(x => x?.Id != null || !string.IsNullOrEmpty(x?.IdStr));

            if (!validTweetIdentifiers.Any())
            {
                throw new ArgumentException("There are no valid tweet identifiers", $"{nameof(parameters)}.{nameof(parameters.Tweets)}");
            }
        }

        public void Validate(IPublishTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.InReplyToTweet != null)
            {
                _tweetQueryValidator.ThrowIfTweetCannotBeUsed(parameters.InReplyToTweet);
            }

            if (parameters.QuotedTweet != null)
            {
                _tweetQueryValidator.ThrowIfTweetCannotBeUsed(parameters.QuotedTweet);
            }
        }

        public void Validate(IDestroyTweetParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(parameters.Tweet, $"{nameof(parameters)}.{nameof(parameters.Tweet)}");
        }

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"{nameof(parameters)}.{nameof(parameters.User)}");
        }

        public void Validate(IGetRetweetsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _tweetQueryValidator.ThrowIfTweetCannotBeUsed(parameters.Tweet, $"{nameof(parameters)}.{nameof(parameters.Tweet)}");
        }
    }
}