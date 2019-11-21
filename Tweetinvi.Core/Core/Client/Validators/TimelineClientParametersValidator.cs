using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITimelineClientParametersValidator
    {
        void Validate(IGetHomeTimelineParameters parameters);
        void Validate(IGetUserTimelineParameters parameters);
        void Validate(IGetRetweetsOfMeTimelineParameters parameters);
    }

    public interface IInternalTimelineClientParametersValidator : ITimelineClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class TimelineClientParametersValidator : IInternalTimelineClientParametersValidator
    {
        private readonly ITimelineClientRequiredParametersValidator _timelineClientRequiredParametersValidator;
        private ITwitterClient _client;
        public TimelineClientParametersValidator(ITimelineClientRequiredParametersValidator timelineClientRequiredParametersValidator)
        {
            _timelineClientRequiredParametersValidator = timelineClientRequiredParametersValidator;
        }
        
        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }
        
        private TwitterLimits Limits => _client.Config.Limits;

        public void Validate(IGetHomeTimelineParameters parameters)
        {
            _timelineClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.TIMELINE_HOME_PAGE_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TIMELINE_HOME_PAGE_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IGetUserTimelineParameters parameters)
        {
            _timelineClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.TIMELINE_USER_PAGE_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TIMELINE_USER_PAGE_MAX_PAGE_SIZE), "page size");
            }
        }

        public void Validate(IGetRetweetsOfMeTimelineParameters parameters)
        {
            _timelineClientRequiredParametersValidator.Validate(parameters);
            
            var maxPageSize = Limits.TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TIMELINE_RETWEETS_OF_ME_MAX_PAGE_SIZE), "page size");
            }
        }
    }
}