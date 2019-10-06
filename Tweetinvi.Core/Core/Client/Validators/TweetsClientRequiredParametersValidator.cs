using System;
using System.Text;
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

        public void Validate(IGetFavoriteTweetsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"${nameof(parameters)}.{nameof(parameters.User)}");
        }
    }
}