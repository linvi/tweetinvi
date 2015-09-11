namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IMentionsTimelineParameters : ITimelineRequestParameters
    {
        bool IncludeContributorDetails { get; set; }
    }
}