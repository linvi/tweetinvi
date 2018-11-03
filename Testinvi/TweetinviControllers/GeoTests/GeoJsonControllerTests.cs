using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Core.Web;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoJsonControllerTests
    {
        private FakeClassBuilder<GeoJsonController> _fakeBuilder;
        private IGeoQueryGenerator _geoQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoJsonController>();
            _geoQueryGenerator = _fakeBuilder.GetFake<IGeoQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        [TestMethod]
        public void GetPlaceFromId_ReturnsGeoQueryExecutorResult()
        {
            string expectedResult = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateGeoJsonController();

            string placeId = Guid.NewGuid().ToString();
            string query = Guid.NewGuid().ToString();

            ArrangeGetPlaceFromIdQueryGenerator(placeId, query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = controller.GetPlaceFromId(placeId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetPlaceFromIdQueryGenerator(string placeId, string result)
        {
            A.CallTo(() => _geoQueryGenerator.GetPlaceFromIdQuery(placeId)).Returns(result);
        }

        public GeoJsonController CreateGeoJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}