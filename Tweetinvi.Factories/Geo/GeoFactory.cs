using Tweetinvi.Core.Factories;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;

namespace Tweetinvi.Factories.Geo
{
    public class GeoFactory : IGeoFactory
    {
        private readonly IFactory<ICoordinates> _coordinatesUnityFactory;
        private readonly IFactory<ILocation> _locationUnityFactory;

        public GeoFactory(
            IFactory<ICoordinates> coordinatesUnityFactory, 
            IFactory<ILocation> locationUnityFactory)
        {
            _coordinatesUnityFactory = coordinatesUnityFactory;
            _locationUnityFactory = locationUnityFactory;
        }

        public ICoordinates GenerateCoordinates(double latitude, double longitude)
        {
            var longitudeParameter = _locationUnityFactory.GenerateParameterOverrideWrapper("longitude", longitude);
            var latitudeParameter = _locationUnityFactory.GenerateParameterOverrideWrapper("latitude", latitude);

            return _coordinatesUnityFactory.Create(longitudeParameter, latitudeParameter);
        }

        public ILocation GenerateLocation(ICoordinates coordinates1, ICoordinates coordinates2)
        {
            if (coordinates1 == null || coordinates2 == null)
            {
                return null;
            }

            var coordinates1Parameter = _locationUnityFactory.GenerateParameterOverrideWrapper("coordinates1", coordinates1);
            var coordinates2Parameter = _locationUnityFactory.GenerateParameterOverrideWrapper("coordinates2", coordinates2);

            return _locationUnityFactory.Create(coordinates1Parameter, coordinates2Parameter);
        }

        public ILocation GenerateLocation(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            var coordinates1 = GenerateCoordinates(latitude1, longitude1);
            var coordinates2 = GenerateCoordinates(latitude2, longitude2);

            return GenerateLocation(coordinates1, coordinates2);
        }

        public IGeoCode GenerateGeoCode(ICoordinates coordinates, double radius, DistanceMeasure measure)
        {
            return new GeoCode(coordinates, radius, measure);
        }
    }
}