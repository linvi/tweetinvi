using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IGeoFactory
    {
        ICoordinates GenerateCoordinates(double longitude, double latitude);
        ILocation GenerateLocation(ICoordinates coordinates1, ICoordinates coordinates2);
        ILocation GenerateLocation(double longitude1, double latitude1,
                                   double longitude2, double latitude2);

        IGeoCode GenerateGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure);
    }
}