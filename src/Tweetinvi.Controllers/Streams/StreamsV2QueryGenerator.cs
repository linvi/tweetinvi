using System.Text;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters.V2;

namespace Tweetinvi.Controllers.Streams
{
    public interface IStreamsV2QueryGenerator
    {
        string GetRulesForFilteredStreamV2Query(IGetRulesForFilteredStreamV2Parameters parameters);
        string GetAddRulesToFilteredStreamQuery(IAddRulesToFilteredStreamV2Parameters parameters);
        string GetDeleteRulesFromFilteredStreamQuery(IDeleteRulesFromFilteredStreamV2Parameters parameters);
        string GetTestFilteredStreamRulesV2Query(IAddRulesToFilteredStreamV2Parameters parameters);
    }

    public class StreamsV2QueryGenerator : IStreamsV2QueryGenerator
    {
        public string GetRulesForFilteredStreamV2Query(IGetRulesForFilteredStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/search/stream/rules");
            query.AddParameterToQuery("ids", parameters.RuleIds);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetAddRulesToFilteredStreamQuery(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/search/stream/rules");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetDeleteRulesFromFilteredStreamQuery(IDeleteRulesFromFilteredStreamV2Parameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/2/tweets/search/stream/rules");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetTestFilteredStreamRulesV2Query(IAddRulesToFilteredStreamV2Parameters parameters)
        {
            var query = new StringBuilder(GetAddRulesToFilteredStreamQuery(parameters));
            query.AddParameterToQuery("dry_run", true);
            return query.ToString();
        }
    }
}