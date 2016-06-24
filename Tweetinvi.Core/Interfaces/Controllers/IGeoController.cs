using System.Collections.Generic;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IGeoController
    {
        IPlace GetPlaceFromId(string placeId);
        IEnumerable<IPlace> SearchGeo(IGeoSearchParameters parameters);
        IEnumerable<IPlace> SearchGeoReverse(IGeoSearchReverseParameters parameters);
    }
}