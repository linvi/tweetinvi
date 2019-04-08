using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Controllers
{
    public interface IGeoController
    {
        Task<IPlace> GetPlaceFromId(string placeId);
        Task<IEnumerable<IPlace>> SearchGeo(IGeoSearchParameters parameters);
        Task<IEnumerable<IPlace>> SearchGeoReverse(IGeoSearchReverseParameters parameters);
    }
}