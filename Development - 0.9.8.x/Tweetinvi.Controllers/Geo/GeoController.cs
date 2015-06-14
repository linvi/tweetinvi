using Tweetinvi.Core.Interfaces.Controllers;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Geo
{
    public class GeoController : IGeoController
    {
        private readonly IGeoQueryExecutor _geoQueryExecutor;

        public GeoController(IGeoQueryExecutor geoQueryExecutor)
        {
            _geoQueryExecutor = geoQueryExecutor;
        }

        public IPlace GetPlaceFromId(string placeId)
        {
            return _geoQueryExecutor.GetPlaceFromId(placeId);
        }
    }
}