using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoQueryExecutor
    {
        IPlace GetPlaceFromId(string placeId);
    }

    public class GeoQueryExecutor : IGeoQueryExecutor
    {
        private readonly ITwitterAccessor _twitterAccessor;
        private readonly IGeoQueryGenerator _geoQueryGenerator;

        public GeoQueryExecutor(
            ITwitterAccessor twitterAccessor,
            IGeoQueryGenerator geoQueryGenerator)
        {
            _twitterAccessor = twitterAccessor;
            _geoQueryGenerator = geoQueryGenerator;
        }

        public IPlace GetPlaceFromId(string placeId)
        {
            string query = _geoQueryGenerator.GetPlaceFromIdQuery(placeId);
            return _twitterAccessor.ExecuteGETQuery<IPlace>(query);
        }
    }
}