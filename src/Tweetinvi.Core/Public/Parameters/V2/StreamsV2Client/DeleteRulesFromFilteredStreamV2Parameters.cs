using System.Linq;
using Tweetinvi.Models;

namespace Tweetinvi.Parameters.V2
{
    public interface IDeleteRulesFromFilteredStreamV2Parameters : ICustomRequestParameters
    {
        string[] RuleIds { get; set; }
    }

    public class DeleteRulesFromFilteredStreamV2Parameters : CustomRequestParameters, IDeleteRulesFromFilteredStreamV2Parameters
    {
        public DeleteRulesFromFilteredStreamV2Parameters(string[] ruleIds)
        {
            RuleIds = ruleIds;
        }

        public DeleteRulesFromFilteredStreamV2Parameters(FilteredStreamRuleV2[] rules)
        {
            RuleIds = rules.Select(x => x.Id).ToArray();
        }

        public string[] RuleIds { get; set; }
    }
}