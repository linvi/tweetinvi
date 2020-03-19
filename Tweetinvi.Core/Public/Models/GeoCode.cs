namespace Tweetinvi.Models
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

        public GeoCode(double latitude, double longitude, double radius, DistanceMeasure distanceMeasure)
        {
            Coordinates = new Coordinates(latitude, longitude);
            Radius = radius;
            DistanceMeasure = distanceMeasure;
        }

        public GeoCode(IGeoCode source)
        {
            if (source == null)
            {
                return;
            }

            Coordinates = new Coordinates(source.Coordinates.Latitude, source.Coordinates.Longitude);
            Radius = source.Radius;
            DistanceMeasure = source.DistanceMeasure;
        }

        public ICoordinates Coordinates { get; set; }
        public double Radius { get; set; }
        public DistanceMeasure DistanceMeasure { get; set; }
    }
}