using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Timeline
{
    public class TimelinesV2QueryGenerator : ITimelinesV2QueryGenerator
    {
        private readonly ITweetsV2QueryGenerator _tweetsV2QueryGenerator;

        public TimelinesV2QueryGenerator(ITweetsV2QueryGenerator tweetsV2QueryGenerator)
        {
            _tweetsV2QueryGenerator = tweetsV2QueryGenerator;
        }

        public string GetTimelineQuery(IGetTimelinesV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.UserV2_Get}/{parameters.UserId}/tweets");
            AddTimelineFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetMentionTimelineQuery(IGetTimelinesV2Parameters parameters)
        {
            var query = new StringBuilder($"{Resources.UserV2_Get}/{parameters.UserId}/mentions");
            AddTimelineFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public void AddTimelineFieldsParameters(IGetTimelinesV2Parameters parameters, StringBuilder query)
        {
            _tweetsV2QueryGenerator.AddTweetFieldsParameters(parameters, query);

            // specific timeline parameters
            query.AddParameterToQuery("exclude", parameters.Exclude);
            query.AddParameterToQuery("max_results", parameters.MaxResults);
            query.AddParameterToQuery("pagination_token", parameters.PaginationToken);
            query.AddParameterToQuery("since_id", parameters.SinceId);
            query.AddParameterToQuery("start_time", parameters.StartTime?.ToString("yyy-MM-ddThh:mm:ssZ"));
            query.AddParameterToQuery("end_time", parameters.EndTime?.ToString("yyy-MM-ddThh:mm:ssZ"));
            query.AddParameterToQuery("until_id", parameters.UntilId);
        }
    }
}
