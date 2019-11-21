using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class TimelineClient : ITimelineClient
    {
        private readonly TwitterClient _client;
        private readonly ITweetFactory _tweetFactory;
        private readonly ITimelineRequester _timelineRequester;

        public TimelineClient(TwitterClient client)
        {
            _client = client;
            _timelineRequester = _client.RequestExecutor.Timeline;
            _tweetFactory = TweetinviContainer.Resolve<ITweetFactory>();
        }

        public ITimelineClientParametersValidator ParametersValidator => _client.ParametersValidator;

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator()
        {
            return GetHomeTimelineIterator(new GetGetHomeTimelineParameters());
        }

        public ITwitterIterator<ITweet, long?> GetHomeTimelineIterator(IGetHomeTimelineParameters parameters)
        {
            var tweetMode = _client.Config.TweetMode;

            var pageIterator = _timelineRequester.GetHomeTimelineIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult =>
                {
                    return _tweetFactory.GenerateTweetsFromDTO(twitterResult.DataTransferObject, tweetMode, _client);
                });
        }

        public Task<ITweet[]> GetRetweetsOfMeTimeline()
        {
            return GetRetweetsOfMeTimeline(new GetRetweetsOfMeTimelineParameters());
        }

        public async Task<ITweet[]> GetRetweetsOfMeTimeline(IGetRetweetsOfMeTimelineParameters parameters)
        {
            var iterator = GetRetweetsOfMeTimelineIterator(parameters);
            var firstResults = await iterator.MoveToNextPage().ConfigureAwait(false);
            return firstResults?.ToArray();
        }

        public ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator()
        {
            return GetRetweetsOfMeTimelineIterator(new GetRetweetsOfMeTimelineParameters());
        }

        public ITwitterIterator<ITweet, long?> GetRetweetsOfMeTimelineIterator(IGetRetweetsOfMeTimelineParameters parameters)
        {
            var tweetMode = _client.Config.TweetMode;

            var pageIterator = _timelineRequester.GetRetweetsOfMeTimelineIterator(parameters);
            return new TwitterIteratorProxy<ITwitterResult<ITweetDTO[]>, ITweet, long?>(pageIterator,
                twitterResult =>
                {
                    return _tweetFactory.GenerateTweetsFromDTO(twitterResult.DataTransferObject, tweetMode, _client);
                });
        }
    }
}