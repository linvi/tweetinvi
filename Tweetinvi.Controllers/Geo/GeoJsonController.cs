using Tweetinvi.Core.Web;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoJsonController
    {
        string GetPlaceFromId(string placeId);
    }

    public class GeoJsonController : IGeoJsonController
    {
        private readonly IGeoQueryGenerator _geoQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public GeoJsonController(
            IGeoQueryGenerator geoQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _geoQueryGenerator = geoQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public string GetPlaceFromId(string placeId)
        {
            string query = _geoQueryGenerator.GetPlaceFromIdQuery(placeId);
            return _twitterAccessor.ExecuteGETQueryReturningJson(query);
        }
    }
}