using System;
using System.Threading.Tasks;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Controllers.V2;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelinesV2Controller : ITimelinesV2Controller
    {
        private readonly ITimelinesV2QueryExecutor _queryExecutor;

        public TimelinesV2Controller(ITimelinesV2QueryExecutor queryExecutor)
        {
            _queryExecutor = queryExecutor;
        }

        private ITwitterPageIterator<ITwitterResult<TimelinesV2Response>, string> GetIterator(
            Func<IGetTimelinesV2Parameters, ITwitterRequest, Task<ITwitterResult<TimelinesV2Response>>> Method,
            IGetTimelinesV2Parameters parameters,
            ITwitterRequest request)
        {
            Func<string, Task<ITwitterResult<TimelinesV2Response>>> getNext = nextToken =>
            {
                var cursoredParameters = new GetTimelinesV2Parameters(parameters)
                {
                    PaginationToken = nextToken
                };

                return Method(cursoredParameters, request);
            };

            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<TimelinesV2Response>, string>(
                parameters.PaginationToken,
                getNext,
                page =>
                {
                    if (page.Model.Tweets.Length == 0)
                    {
                        return null;
                    }

                    return page.Model.Meta.NextToken;
                },
                page =>
                {
                    if (page.Model.Tweets.Length == 0)
                    {
                        return true;
                    }

                    return page.Model.Meta.NextToken == null;
                });

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<TimelinesV2Response>, string> GetUserTweetsTimelineIterator(IGetTimelinesV2Parameters parameters, ITwitterRequest request)
        {
            return GetIterator(_queryExecutor.GetUserTweetsTimelineAsync, parameters, request);
        }

        public ITwitterPageIterator<ITwitterResult<TimelinesV2Response>, string> GetUserMentionedTimelineIterator(IGetTimelinesV2Parameters parameters, ITwitterRequest request)
        {
            return GetIterator(_queryExecutor.GetUserMentionedTimelineAsync, parameters, request);
        }
    }
}
