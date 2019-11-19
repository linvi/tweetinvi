using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public interface IInternalTimelineRequester : ITimelineRequester, IBaseRequester
    {
    }
    
    public class TimelineRequester : BaseRequester, IInternalTimelineRequester
    {
        private readonly ITimelineController _timelineController;
        private readonly ITimelineClientRequiredParametersValidator _validator;

        public TimelineRequester(
            ITimelineController timelineController,
            ITimelineClientRequiredParametersValidator validator)
        {
            _timelineController = timelineController;
            _validator = validator;
        }
        
        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters)
        {
            _validator.Validate(parameters);
            
            var request = TwitterClient.CreateRequest();
            return _timelineController.GetRetweetsOfMeTimeline(parameters, request);
        }
    }
}