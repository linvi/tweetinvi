namespace Tweetinvi.Models
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

        public Location(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            Coordinate1 = new Coordinates(latitude1, longitude1);
            Coordinate2 = new Coordinates(latitude2, longitude2);
        }

        public static bool CoordinatesLocatedIn(ICoordinates coordinates, ILocation location)
        {
            return CoordinatesLocatedIn(coordinates, location.Coordinate1, location.Coordinate2);
        }

		public static bool CoordinatesLocatedIn(ICoordinates testPoint, ICoordinates boxNorthwestCoordinates, ICoordinates boxSoutheastCoordinates)
		{
			bool longitudeIsBetweenCoordinates = boxNorthwestCoordinates.Longitude <= testPoint.Longitude &&
			testPoint.Longitude <= boxSoutheastCoordinates.Longitude;

			bool latitudeIsBetweenCoordinates = boxNorthwestCoordinates.Latitude >= testPoint.Latitude &&
			testPoint.Latitude >= boxSoutheastCoordinates.Latitude;

			//TODO: Add a case where the box crosses the antimeridian for our friends in Fiji and Togo

			return (longitudeIsBetweenCoordinates && latitudeIsBetweenCoordinates);
		}

	}
}