using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Controllers
{
    public interface IGeoController
    {
        IPlace GetPlaceFromId(string placeId);
    }
}