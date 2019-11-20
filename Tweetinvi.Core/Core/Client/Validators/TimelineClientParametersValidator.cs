using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITimelineClientParametersValidator
    {
        void Validate(IGetRetweetsOfMeTimelineParameters parameters);
    }

    public interface IInternalTimelineClientParametersValidator : ITimelineClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class TimelineClientParametersValidator : IInternalTimelineClientParametersValidator
    {
        public void Initialize(ITwitterClient client)
        {
        }

        public void Validate(IGetRetweetsOfMeTimelineParameters parameters)
        {
        }
    }
}