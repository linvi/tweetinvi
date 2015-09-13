namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IRetweetsOfMeTimelineRequestParameters : ITimelineRequestParameters
    {
        bool IncludeUserEntities { get; set; }
    }
}
