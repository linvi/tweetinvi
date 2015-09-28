using Tweetinvi.Core.Interfaces.Models;

namespace Tweetinvi.Core.Parameters
{
    public class Location : ILocation
    {
        public ICoordinates Coordinate1 { get; set; }
        public ICoordinates Coordinate2 { get; set; }

        public Location(ICoordinates coordinates1, ICoordinates coordinates2)
        {
            Coordinate1 = coordinates1;
            Coordinate2 = coordinates2;
        }

        public Location(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            Coordinate1 = new Coordinates(longitude1, latitude1);
            Coordinate2 = new Coordinates(longitude2, latitude2);
        }

        public static bool CoordinatesLocatedIn(ICoordinates coordinates, ILocation location)
        {
            return CoordinatesLocatedIn(coordinates, location.Coordinate1, location.Coordinate2);
        }

        public static bool CoordinatesLocatedIn(ICoordinates coordinates, ICoordinates boxCoordinates1, ICoordinates boxCoordinates2)
        {
            bool xIsBelowCoord1AndAboveCoord2 = boxCoordinates1.Longitude >= coordinates.Longitude &&
                                                    coordinates.Longitude >= boxCoordinates2.Longitude;

            bool xIsAboveCoord1AndBelowCoord2 = boxCoordinates1.Longitude <= coordinates.Longitude &&
                                                coordinates.Longitude <= boxCoordinates2.Longitude;

            bool yIsRightCoord1AndLeftCoord2 = boxCoordinates1.Latitude >= coordinates.Latitude &&
                                               coordinates.Latitude >= boxCoordinates2.Latitude;

            bool yIsLeftCoord1AndRightCoord2 = boxCoordinates1.Latitude <= coordinates.Latitude &&
                                               coordinates.Latitude <= boxCoordinates2.Latitude;

            return (xIsAboveCoord1AndBelowCoord2 || xIsBelowCoord1AndAboveCoord2) &&
                   (yIsLeftCoord1AndRightCoord2 || yIsRightCoord1AndLeftCoord2);
        }
    }
}