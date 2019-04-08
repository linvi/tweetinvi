using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Geo
{
    public interface IGeoQueryExecutor
    {
        Task<IPlace> GetPlaceFromId(string placeId);
        Task<IEnumerable<IPlace>> SearchGeo(IGeoSearchParameters parameters);
        Task<IEnumerable<IPlace>> SearchGeoReverse(IGeoSearchReverseParameters parameters);
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

        public Task<IPlace> GetPlaceFromId(string placeId)
        {
            string query = _geoQueryGenerator.GetPlaceFromIdQuery(placeId);
            return _twitterAccessor.ExecuteGETQuery<IPlace>(query);
        }

        public Task<IEnumerable<IPlace>> SearchGeo(IGeoSearchParameters parameters)
        {
            var query = _geoQueryGenerator.GetSearchGeoQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IPlace>>(query, "result", "places");
        }

        public Task<IEnumerable<IPlace>> SearchGeoReverse(IGeoSearchReverseParameters parameters)
        {
            var query = _geoQueryGenerator.GetSearchGeoReverseQuery(parameters);
            return _twitterAccessor.ExecuteGETQueryWithPath<IEnumerable<IPlace>>(query, "result", "places");
        }
    }
}