using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalTimelinesRequester : ITimelinesRequester, IBaseRequester
    {
    }

    public class TimelinesRequester : BaseRequester, IInternalTimelinesRequester
    {
        private readonly ITimelineController _timelineController;
        private readonly ITimelineClientRequiredParametersValidator _validator;

        public TimelinesRequester(
            ITimelineController timelineController,
            ITimelineClientRequiredParametersValidator validator)
        {
            _timelineController = timelineController;
            _validator = validator;
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetUserTimelineIterator(IGetUserTimelineParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _timelineController.GetUserTimelineIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _timelineController.GetHomeTimelineIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _timelineController.GetRetweetsOfMeTimelineIterator(parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters)
        {
            _validator.Validate(parameters);

            var request = TwitterClient.CreateRequest();
            return _timelineController.GetMentionsTimelineIterator(parameters, request);
        }
    }
}