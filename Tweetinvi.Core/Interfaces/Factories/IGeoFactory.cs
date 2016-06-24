using Tweetinvi.Core.Enum;
using Tweetinvi.Models;

namespace Tweetinvi.Core.Interfaces.Factories
{
    public interface IGeoFactory
    {
        ICoordinates GenerateCoordinates(double latitude, double longitude);
        ILocation GenerateLocation(ICoordinates coordinates1, ICoordinates coordinates2);
        ILocation GenerateLocation(double latitude1, double longitude1, 
                                   double latitude2, double longitude2);

        IGeoCode GenerateGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure);
    }
}