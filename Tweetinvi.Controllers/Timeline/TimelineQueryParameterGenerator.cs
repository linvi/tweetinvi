using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryParameterGenerator
    {
        string GenerateExcludeRepliesParameter(bool excludeReplies);
        string GenerateIncludeContributorDetailsParameter(bool includeContributorDetails);
        string GenerateIncludeRTSParameter(bool includeRTS);
        string GenerateIncludeUserEntitiesParameter(bool includeUserEntities);

        IMentionsTimelineParameters CreateMentionsTimelineParameters();
        IGetRetweetsOfMeTimelineParameters CreateRetweetsOfMeTimelineParameters();
    }

    public class TimelineQueryParameterGenerator : ITimelineQueryParameterGenerator
    {
        private readonly IFactory<IGetHomeTimelineParameters> _homeTimelineRequestParameterFactory;
        private readonly IFactory<IMentionsTimelineParameters> _mentionsTimelineRequestParameterFactory;
        private readonly IFactory<IGetRetweetsOfMeTimelineParameters> _retweetsOfMeTimelineRequestParameterFactory;

        public TimelineQueryParameterGenerator(
            IFactory<IGetHomeTimelineParameters> homeTimelineRequestParameterFactory,
            IFactory<IMentionsTimelineParameters> mentionsTimelineRequestParameterFactory,
            IFactory<IGetRetweetsOfMeTimelineParameters> retweetsOfMeTimelineRequestParameterFactory)
        {
            _homeTimelineRequestParameterFactory = homeTimelineRequestParameterFactory;
            _mentionsTimelineRequestParameterFactory = mentionsTimelineRequestParameterFactory;
            _retweetsOfMeTimelineRequestParameterFactory = retweetsOfMeTimelineRequestParameterFactory;
        }

        public string GenerateExcludeRepliesParameter(bool excludeReplies)
        {
            return string.Format(Resources.TimelineParameter_ExcludeReplies, excludeReplies);
        }

        public string GenerateIncludeContributorDetailsParameter(bool includeContributorDetails)
        {
            return string.Format(Resources.TimelineParameter_IncludeContributorDetails, includeContributorDetails);
        }

        public string GenerateIncludeRTSParameter(bool includeRTS)
        {
            return string.Format(Resources.QueryParameter_IncludeRetweets, includeRTS);
        }

        public string GenerateIncludeUserEntitiesParameter(bool includeUserEntities)
        {
            return string.Format(Resources.TimelineParameter_IncludeUserEntities, includeUserEntities);
        }

        // User Parameters

        public IMentionsTimelineParameters CreateMentionsTimelineParameters()
        {
            return _mentionsTimelineRequestParameterFactory.Create();
        }

        public IGetRetweetsOfMeTimelineParameters CreateRetweetsOfMeTimelineParameters()
        {
            return _retweetsOfMeTimelineRequestParameterFactory.Create();
        }
    }
}