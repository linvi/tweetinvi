using Tweetinvi.Exceptions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface ITimelineClientParametersValidator
    {
        void Validate(IGetHomeTimelineParameters parameters);
        void Validate(IGetUserTimelineParameters parameters);
        void Validate(IGetMentionsTimelineParameters parameters);
        void Validate(IGetRetweetsOfMeTimelineParameters parameters);
    }

    public class TimelineClientParametersValidator : ITimelineClientParametersValidator
    {
        private readonly ITimelineClientRequiredParametersValidator _timelineClientRequiredParametersValidator;
        private readonly ITwitterClient _client;
        public TimelineClientParametersValidator(ITwitterClient client, ITimelineClientRequiredParametersValidator timelineClientRequiredParametersValidator)
        {
            _client = client;
            _timelineClientRequiredParametersValidator = timelineClientRequiredParametersValidator;
        }

        private TwitterLimits Limits => _client.ClientSettings.Limits;

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

        public void Validate(IGetMentionsTimelineParameters parameters)
        {
            _timelineClientRequiredParametersValidator.Validate(parameters);

            var maxPageSize = Limits.TIMELINE_MENTIONS_PAGE_MAX_PAGE_SIZE;
            if (parameters.PageSize > maxPageSize)
            {
                throw new TwitterArgumentLimitException($"{nameof(parameters)}.{nameof(parameters.PageSize)}", maxPageSize, nameof(Limits.TIMELINE_MENTIONS_PAGE_MAX_PAGE_SIZE), "page size");
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