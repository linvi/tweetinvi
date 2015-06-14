using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public class GeoCode : IGeoCode
    {
        public GeoCode()
        {
        }

        public GeoCode(ICoordinates coordinates, double radius, DistanceMeasure distanceMeasure)
        {
            Coordinates = coordinates;
            Radius = radius;
            DistanceMeasure = distanceMeasure;
        }

        public GeoCode(double longitude, double latitude, double radius, DistanceMeasure distanceMeasure)
        {
            Coordinates = new Coordinates(longitude, latitude);
            Radius = radius;
            DistanceMeasure = distanceMeasure;
        }

        public ICoordinates Coordinates { get; set; }
        public double Radius { get; set; }
        public DistanceMeasure DistanceMeasure { get; set; }
    }
}