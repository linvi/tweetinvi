namespace Tweetinvi.Core.Parameters
{
    public interface ITimelineRequestParameters : ICustomRequestParameters
    {
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        long SinceId { get; set; }
        long MaxId { get; set; }

        bool TrimUser { get; set; }
        bool IncludeEntities { get; set; }
    }
}