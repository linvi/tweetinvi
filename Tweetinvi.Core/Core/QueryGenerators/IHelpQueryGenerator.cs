using Tweetinvi.Parameters;

namespace Tweetinvi.Core.QueryGenerators
{
    public interface IHelpQueryGenerator
    {
        string GetRateLimitsQuery(IGetRateLimitsParameters parameters);
        string GetTwitterConfigurationQuery(IGetTwitterConfigurationParameters parameters);
        string GetSupportedLanguagesQuery(IGetSupportedLanguagesParameters parameters);

        string GetPlaceQuery(IGetPlaceParameters parameters);
        // string GenerateGeoParameter(ICoordinates coordinates);
        string GetSearchGeoQuery(IGeoSearchParameters parameters);
        string GetSearchGeoReverseQuery(IGeoSearchReverseParameters parameters);
    }
}