using System;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITimelineClientRequiredParametersValidator : ITimelineClientParametersValidator
    {
    }

    public class TimelineClientRequiredParametersValidator : ITimelineClientRequiredParametersValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public TimelineClientRequiredParametersValidator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public void Validate(IGetHomeTimelineParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetUserTimelineParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"{nameof(parameters)}.{nameof(parameters.User)}");
        }

        public void Validate(IGetRetweetsOfMeTimelineParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }
    }
}