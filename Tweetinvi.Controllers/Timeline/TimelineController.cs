using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelineController : ITimelineController
    {
        private readonly ITweetFactory _tweetFactory;
        private readonly ITimelineQueryExecutor _timelineQueryExecutor;
        private readonly IUserFactory _userFactory;
        private readonly IPageCursorIteratorFactories _pageCursorIteratorFactories;
        private readonly ITimelineQueryParameterGenerator _timelineQueryParameterGenerator;

        public TimelineController(
            ITweetFactory tweetFactory,
            ITimelineQueryExecutor timelineQueryExecutor,
            IUserFactory userFactory,
            IPageCursorIteratorFactories pageCursorIteratorFactories,
            ITimelineQueryParameterGenerator timelineQueryParameterGenerator)
        {
            _tweetFactory = tweetFactory;
            _timelineQueryExecutor = timelineQueryExecutor;
            _userFactory = userFactory;
            _pageCursorIteratorFactories = pageCursorIteratorFactories;
            _timelineQueryParameterGenerator = timelineQueryParameterGenerator;
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

        // Mention Timeline
        public Task<IEnumerable<IMention>> GetMentionsTimeline(int maximumNumberOfTweets = 40)
        {
            var timelineRequestParameter = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            timelineRequestParameter.PageSize = maximumNumberOfTweets;

            return GetMentionsTimeline(timelineRequestParameter);
        }

        public async Task<IEnumerable<IMention>> GetMentionsTimeline(IMentionsTimelineParameters parameters)
        {
            if (parameters == null)
            {
                parameters = _timelineQueryParameterGenerator.CreateMentionsTimelineParameters();
            }

            var timelineDTO = await _timelineQueryExecutor.GetMentionsTimeline(parameters);
            return _tweetFactory.GenerateMentionsFromDTO(timelineDTO);
        }

        // Retweets Of Me Timeline
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