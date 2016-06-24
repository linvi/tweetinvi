using System;
using System.Globalization;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoQueryGeneratorTests
    {
        private FakeClassBuilder<GeoQueryGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoQueryGenerator>();
        }

        #region GetPlaceFromIdQuery

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GenerateGeoParameter_CoordinatesIsNull_ReturnsNull()
        {
            // Arrange
            var controller = CreateGeoQueryGenerator();

            // Act
            controller.GenerateGeoParameter(null);
        }

        [TestMethod]
        public void GenerateGeoParameter_Coordinates_ReturnsValidQuery()
        {
            var coordinates = A.Fake<ICoordinates>();
            coordinates.Longitude = new Random().Next();
            coordinates.Latitude = new Random().Next();

            // Arrange
            var controller = CreateGeoQueryGenerator();

            // Act
            var result = controller.GenerateGeoParameter(coordinates);

            // Assert
            string expectedLongitude = coordinates.Longitude.ToString(CultureInfo.InvariantCulture);
            string expectedLatitude = coordinates.Latitude.ToString(CultureInfo.InvariantCulture);
            Assert.AreEqual(result, string.Format(Resources.Geo_CoordinatesParameter, expectedLongitude, expectedLatitude));
        }

        #endregion

        #region GetPlaceFromIdQuery

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetPlaceFromIdQuery_PlaceIdIsNull_ReturnsNull()
        {
            // Arrange
            var controller = CreateGeoQueryGenerator();

            // Act
            var result = controller.GetPlaceFromIdQuery(null);
            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetPlaceFromIdQuery_PlaceId_ReturnsValidQuery()
        {
            string placeId = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateGeoQueryGenerator();

            // Act
            var result = controller.GetPlaceFromIdQuery(placeId);

            // Assert
            Assert.AreEqual(result, string.Format(Resources.Geo_GetPlaceFromId, placeId));
        }

        #endregion

        public GeoQueryGenerator CreateGeoQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}