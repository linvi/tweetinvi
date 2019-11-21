using System;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITimelineClientRequiredParametersValidator : ITimelineClientParametersValidator
    {
    }
    
    public class TimelineClientRequiredParametersValidator : ITimelineClientRequiredParametersValidator
    {
        public void Validate(IGetHomeTimelineParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
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