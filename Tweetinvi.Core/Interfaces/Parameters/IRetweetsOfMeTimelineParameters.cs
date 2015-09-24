namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IRetweetsOfMeTimelineParameters : ITimelineRequestParameters
    {
        bool IncludeUserEntities { get; set; }
    }
}
