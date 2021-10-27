using System;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Parameters.V2
{
    public interface IGetTimelinesV2Parameters : IBaseTweetsV2Parameters
    {
        string UserId { get; set; }
        string Exclude { get; set; }
        int? MaxResults { get; set; }
        string PaginationToken { get; set; }
        string SinceId { get; set; }
        DateTime? StartTime { get; set; }
        DateTime? EndTime { get; set; }
        string UntilId { get; set; }
    }

    public class GetTimelinesV2Parameters : BaseTweetsV2Parameters, IGetTimelinesV2Parameters
    {
        public GetTimelinesV2Parameters(string userId)
        {
            UserId = userId;
        }
        public GetTimelinesV2Parameters(IGetTimelinesV2Parameters parameters)
        {
            UserId = parameters.UserId;
            Exclude = parameters?.Exclude;
            MaxResults = parameters?.MaxResults;
            PaginationToken = parameters?.PaginationToken;
            SinceId = parameters?.SinceId;
            StartTime = parameters?.StartTime;
            EndTime = parameters?.EndTime;
            UntilId = parameters?.UntilId;

            Expansions = parameters.Expansions;
            MediaFields = parameters.MediaFields;
            PlaceFields = parameters.PlaceFields;
            PollFields = parameters.PollFields;
            TweetFields = parameters.TweetFields;
            UserFields = parameters.UserFields;

        }

        public string UserId { get; set; }
        public string Exclude { get; set; }
        public int? MaxResults { get; set; }
        public string PaginationToken { get; set; }
        public string SinceId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string UntilId { get; set; }
    }
}
