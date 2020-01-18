using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class TimelinesRequester : BaseRequester, ITimelinesRequester
    {
        private readonly ITimelineController _timelineController;
        private readonly ITimelineClientRequiredParametersValidator _validator;

        public TimelinesRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            ITimelineController timelineController,
            ITimelineClientRequiredParametersValidator validator)
        : base(client, clientEvents)
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