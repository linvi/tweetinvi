using System;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface ISearchTweetsV2Parameters : IBaseTweetsV2Parameters
    {
        DateTime? EndTime { get; set; }
        string Query { get; set; }
        int? PageSize { get; set; }
        string NextToken { get; set; }
        string SinceId { get; set; }
        DateTime? StartTime { get; set; }
        string UntilId { get; set; }
    }

    public class SearchTweetsV2Parameters : BaseTweetsV2Parameters, ISearchTweetsV2Parameters
    {
        public SearchTweetsV2Parameters(string query)
        {
            Query = query;
            PageSize = 100;
        }

        public SearchTweetsV2Parameters(ISearchTweetsV2Parameters parameters)
        {
            EndTime = parameters?.EndTime;
            Query = parameters?.Query;
            PageSize = parameters?.PageSize;
            NextToken = parameters?.NextToken;
            SinceId = parameters?.SinceId;
            StartTime = parameters?.StartTime;
            UntilId = parameters?.UntilId;
        }

        public DateTime? EndTime { get; set; }
        public string Query { get; set; }
        public int? PageSize { get; set; }
        public string NextToken { get; set; }
        public string SinceId { get; set; }
        public DateTime? StartTime { get; set; }
        public string UntilId { get; set; }
    }
}