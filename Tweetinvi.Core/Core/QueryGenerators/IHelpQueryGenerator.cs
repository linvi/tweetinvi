using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IHelpQueryGenerator
    {
        string GetRateLimitsQuery(IGetRateLimitsParameters parameters);
        string GetTwitterConfigurationQuery(IGetTwitterConfigurationParameters parameters);
    }
}