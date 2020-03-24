using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelineController : ITimelineController
    {
        private readonly ITimelineQueryExecutor _timelineQueryExecutor;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;

        public TimelineController(ITimelineQueryExecutor timelineQueryExecutor,
            IPageCursorIteratorFactories pageCursorIteratorFactories)
        {
            _timelineQueryExecutor = timelineQueryExecutor;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
        }

        // Home Timeline

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetHomeTimelineParameters(parameters)
                {
                    MaxId = cursor
                };

                return _timelineQueryExecutor.GetHomeTimeline(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetUserTimelineIterator(IGetUserTimelineParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetUserTimelineParameters(parameters)
                {
                    MaxId = cursor
                };

                return _timelineQueryExecutor.GetUserTimeline(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetMentionsTimelineIterator(IGetMentionsTimelineParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetMentionsTimelineParameters(parameters)
                {
                    MaxId = cursor
                };

                return _timelineQueryExecutor.GetMentionsTimeline(cursoredParameters, new TwitterRequest(request));
            });
        }

        public ITwitterPageIterator<ITwitterResult<ITweetDTO[]>, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters, ITwitterRequest request)
        {
            return _pageCursorIteratorFactories.Create(parameters, cursor =>
            {
                var cursoredParameters = new GetRetweetsOfMeTimelineParameters(parameters)
                {
                    MaxId = cursor
                };

                return _timelineQueryExecutor.GetRetweetsOfMeTimeline(cursoredParameters, new TwitterRequest(request));
            });
        }
    }
}