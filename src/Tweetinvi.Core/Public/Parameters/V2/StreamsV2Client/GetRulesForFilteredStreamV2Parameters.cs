namespace Tweetinvi.Parameters.V2
{
    public interface IGetRulesForFilteredStreamV2Parameters : ICustomRequestParameters
    {
        string[] RuleIds { get; set; }
    }

    public class GetRulesForFilteredStreamV2Parameters : CustomRequestParameters, IGetRulesForFilteredStreamV2Parameters
    {
        public GetRulesForFilteredStreamV2Parameters()
        {
        }

        public GetRulesForFilteredStreamV2Parameters(string[] ruleIds)
        {
            RuleIds = ruleIds;
        }

        public string[] RuleIds { get; set; }
    }
}