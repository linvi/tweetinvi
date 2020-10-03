using System.Text;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators.V2;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Search
{
    public interface ISearchV2QueryGenerator
    {
        string GetSearchTweetsV2Query(ISearchTweetsV2Parameters parameters);
    }

    public class SearchV2QueryGenerator : ISearchV2QueryGenerator
    {
        private readonly ITweetsV2QueryGenerator _tweetsV2QueryGenerator;

        public SearchV2QueryGenerator(ITweetsV2QueryGenerator tweetsV2QueryGenerator)
        {
            _tweetsV2QueryGenerator = tweetsV2QueryGenerator;
        }

        public string GetSearchTweetsV2Query(ISearchTweetsV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/search/recent");
            query.AddParameterToQuery("query", parameters.Query);
            query.AddParameterToQuery("end_time", parameters.EndTime?.ToString("yyy-MM-ddThh:mm:ssZ"));
            query.AddParameterToQuery("max_results", parameters.PageSize);
            query.AddParameterToQuery("next_token", parameters.NextToken);
            query.AddParameterToQuery("since_id", parameters.SinceId);
            query.AddParameterToQuery("start_time", parameters.StartTime?.ToString("yyy-MM-ddThh:mm:ssZ"));
            query.AddParameterToQuery("until_id", parameters.UntilId);
            _tweetsV2QueryGenerator.AddTweetFieldsParameters(parameters, query);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }
    }
}