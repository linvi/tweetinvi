using System.Collections.Generic;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryExecutor
    {
        // Home Timeline
        IEnumerable<ITweetDTO> GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        IEnumerable<ITweetDTO> GetUserTimeline(IUserTimelineQueryParameters timelineParameters);

        // Mention Timeline
        IEnumerable<ITweetDTO> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters);

        // Retweets Of Me Timeline
        IEnumerable<ITweetDTO> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters timelineParameters);
    }

    public class TimelineQueryExecutor : ITimelineQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly ITimelineQueryGenerator _timelineQueryGenerator;

        public TimelineQueryExecutor(
            ITwitterAccessor twitterAccessor,
            ITimelineQueryGenerator timelineQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _timelineQueryGenerator = timelineQueryGenerator;
        }

        // Home Timeline
        public IEnumerable<ITweetDTO> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // User Timeline
        public IEnumerable<ITweetDTO> GetUserTimeline(IUserTimelineQueryParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetUserTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Mention Timeline
        public IEnumerable<ITweetDTO> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Retweets of Me Timeline
        public IEnumerable<ITweetDTO> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetRetweetsOfMeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }
    }
}