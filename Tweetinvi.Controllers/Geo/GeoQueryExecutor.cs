using System.Collections.Generic;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Parameters;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoQueryExecutor
    {
        IPlace GetPlaceFromId(string placeId);
        IEnumerable<IPlace> SearchGeo(IGeoSearchParameters parameters);
        IEnumerable<IPlace> SearchGeoReverse(IGeoSearchReverseParameters parameters);
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

        public IEnumerable<IPlace> SearchGeo(IGeoSearchParameters parameters)
        {
            var query = _geoQueryGenerator.GetSearchGeoQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IPlace>>(query, "result", "places");
        }

        public IEnumerable<IPlace> SearchGeoReverse(IGeoSearchReverseParameters parameters)
        {
            var query = _geoQueryGenerator.GetSearchGeoReverseQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IPlace>>(query, "result", "places");
        }
    }
}