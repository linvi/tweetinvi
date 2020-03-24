using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Timeline
{
    public interface ITimelineQueryGenerator
    {
        string GetHomeTimelineQuery(IGetHomeTimelineParameters parameters, TweetMode? tweetMode);
        string GetUserTimelineQuery(IGetUserTimelineParameters parameters, TweetMode? tweetMode);

        // Mention Timeline
        string GetMentionsTimelineQuery(IGetMentionsTimelineParameters getMentionsTimelineParameters, TweetMode? tweetMode);

        // Retweets of Me Timeline
        string GetRetweetsOfMeTimelineQuery(IGetRetweetsOfMeTimelineParameters parameters, TweetMode? tweetMode);
    }

    public class TimelineQueryGenerator : ITimelineQueryGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;
        private readonly IQueryParameterGenerator _queryParameterGenerator;

        public TimelineQueryGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IQueryParameterGenerator queryParameterGenerator)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _queryParameterGenerator = queryParameterGenerator;
        }

        // Home Timeline
        public string GetHomeTimelineQuery(IGetHomeTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetHomeTimeline);

            _queryParameterGenerator.AddTimelineParameters(query, parameters);

            query.AddParameterToQuery("exclude_replies", parameters.ExcludeReplies);
            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // User Timeline
        public string GetUserTimelineQuery(IGetUserTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetUserTimeline);

            query.AddFormattedParameterToQuery(_userQueryParameterGenerator.GenerateIdOrScreenNameParameter(parameters.User));

            _queryParameterGenerator.AddTimelineParameters(query, parameters);

            query.AddParameterToQuery("exclude_replies", parameters.ExcludeReplies);
            query.AddParameterToQuery("include_rts", parameters.IncludeRetweets);
            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Mentions Timeline
        public string GetMentionsTimelineQuery(IGetMentionsTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetMentionsTimeline);

            _queryParameterGenerator.AddTimelineParameters(query, parameters);

            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        // Retweets of Me Timeline
        public string GetRetweetsOfMeTimelineQuery(IGetRetweetsOfMeTimelineParameters parameters, TweetMode? tweetMode)
        {
            var query = new StringBuilder(Resources.Timeline_GetRetweetsOfMeTimeline);

            _queryParameterGenerator.AddTimelineParameters(query, parameters);
            query.AddParameterToQuery("include_user_entities", parameters.IncludeUserEntities);

            query.AddParameterToQuery("tweet_mode", tweetMode?.ToString().ToLowerInvariant());
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }


    }
}