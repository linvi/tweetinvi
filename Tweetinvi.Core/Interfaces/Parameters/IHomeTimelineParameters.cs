namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IHomeTimelineParameters : ITimelineRequestParameters
    {
        bool IncludeContributorDetails { get; set; }
        bool ExcludeReplies { get; set; }
    }
}