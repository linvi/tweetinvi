using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryExecutor
    {
        // Home Timeline
        Task<ITwitterResult<ITweetDTO[]>> GetHomeTimeline(IGetHomeTimelineParameters parameters, ITwitterRequest request);

        // User Timeline
        Task<ITwitterResult<ITweetDTO[]>> GetUserTimeline(IGetUserTimelineParameters parameters, ITwitterRequest request);

        // Mention Timeline
        Task<ITwitterResult<ITweetDTO[]>> GetMentionsTimeline(IGetMentionsTimelineParameters parameters, ITwitterRequest request);

        // Retweets Of Me Timeline
        Task<ITwitterResult<ITweetDTO[]>> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters, ITwitterRequest request);
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
        public Task<ITwitterResult<ITweetDTO[]>> GetHomeTimeline(IGetHomeTimelineParameters parameters, ITwitterRequest request)
        {
            var query = _timelineQueryGenerator.GetHomeTimelineQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        public Task<ITwitterResult<ITweetDTO[]>> GetUserTimeline(IGetUserTimelineParameters parameters, ITwitterRequest request)
        {
            var query = _timelineQueryGenerator.GetUserTimelineQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        // Mention Timeline
        public Task<ITwitterResult<ITweetDTO[]>> GetMentionsTimeline(IGetMentionsTimelineParameters parameters, ITwitterRequest request)
        {
            var query = _timelineQueryGenerator.GetMentionsTimelineQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }

        // Retweets of Me Timeline
        public Task<ITwitterResult<ITweetDTO[]>> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters, ITwitterRequest request)
        {
            var query = _timelineQueryGenerator.GetRetweetsOfMeTimelineQuery(parameters, request.ExecutionContext.TweetMode);
            request.Query.Url = query;
            request.Query.HttpMethod = HttpMethod.GET;
            return _twitterAccessor.ExecuteRequest<ITweetDTO[]>(request);
        }
    }
}