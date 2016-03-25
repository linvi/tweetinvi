using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public class Coordinates : ICoordinates
    {
        public Coordinates(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}