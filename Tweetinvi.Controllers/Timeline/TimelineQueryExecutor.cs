using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryExecutor
    {
        // Home Timeline
        Task<IEnumerable<ITweetDTO>> GetHomeTimeline(IHomeTimelineParameters timelineParameters);

        // User Timeline
        Task<IEnumerable<ITweetDTO>> GetUserTimeline(IUserTimelineQueryParameters timelineParameters);

        // Mention Timeline
        Task<IEnumerable<ITweetDTO>> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters);

        // Retweets Of Me Timeline
        Task<IEnumerable<ITweetDTO>> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters timelineParameters);
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
        public Task<IEnumerable<ITweetDTO>> GetHomeTimeline(IHomeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetHomeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // User Timeline
        public Task<IEnumerable<ITweetDTO>> GetUserTimeline(IUserTimelineQueryParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetUserTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Mention Timeline
        public Task<IEnumerable<ITweetDTO>> GetMentionsTimeline(IMentionsTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetMentionsTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }

        // Retweets of Me Timeline
        public Task<IEnumerable<ITweetDTO>> GetRetweetsOfMeTimeline(IRetweetsOfMeTimelineParameters timelineParameters)
        {
            string query = _timelineQueryGenerator.GetRetweetsOfMeTimelineQuery(timelineParameters);
            return _twitterAccessor.ExecuteGETQuery<IEnumerable<ITweetDTO>>(query);
        }
    }
}