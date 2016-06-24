namespace Tweetinvi.Models
{
    public class Coordinates : ICoordinates
    {
        public Coordinates(double latitude, double longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}