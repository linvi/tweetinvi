namespace Tweetinvi.Core.Interfaces.Parameters
{
    public interface IGetTweetsFromListParameters : ICustomRequestParameters
    {
        int MaximumNumberOfTweetsToRetrieve { get; set; }

        long? SinceId { get; set; }
        long? MaxId { get; set; }
        bool IncludeEntities { get; set; }
        bool IncludeRetweets { get; set; }
    }
}
