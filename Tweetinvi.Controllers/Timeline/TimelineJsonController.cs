using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineJsonController
    {
        // Mention Timeline
        Task<string> GetMentionsTimeline(int maximumNumberOfTweets = 40);
        Task<string> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters);
    }

    public class TimelineJsonController : ITimelineJsonController
    {
        private readonly ITimelineQueryGenerator _timelineQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        public TimelineJsonController(
            ITimelineQueryGenerator timelineQueryGenerator,
            ITwitterAccessor twitterAccessor,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator)
        {
            _timelineQueryGenerator = timelineQueryGenerator;
            _twitterAccessor = twitterAccessor;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
        }

        // Mentions Timeline
        public Task<string> GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var requestParameters = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            requestParameters.PageSize = maximumNumberOfTweets;

            return GetMentionsTimeline(requestParameters);
        }

        public Task<string> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            var query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}