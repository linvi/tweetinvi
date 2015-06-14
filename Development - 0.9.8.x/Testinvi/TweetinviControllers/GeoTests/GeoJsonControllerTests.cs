using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Core.Interfaces.Credentials;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoJsonControllerTests
    {
        private FakeClassBuilder<GeoJsonController> _fakeBuilder;
        private Fake<IGeoQueryGenerator> _fakeGeoQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoJsonController>();
            _fakeGeoQueryGenerator = _fakeBuilder.GetFake<IGeoQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
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
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = controller.GetPlaceFromId(placeId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetPlaceFromIdQueryGenerator(string placeId, string result)
        {
            _fakeGeoQueryGenerator
                .CallsTo(x => x.GetPlaceFromIdQuery(placeId))
                .Returns(result);
        }

        public GeoJsonController CreateGeoJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}