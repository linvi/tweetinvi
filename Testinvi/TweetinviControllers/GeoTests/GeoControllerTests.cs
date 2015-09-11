using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoControllerTests
    {
        private FakeClassBuilder<GeoController> _fakeBuilder;
        private Fake<IGeoQueryExecutor> _fakeGeoQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoController>();
            _fakeGeoQueryExecutor = _fakeBuilder.GetFake<IGeoQueryExecutor>();
        }

        [TestMethod]
        public void GetPlaceFromId_ReturnsGeoQueryExecutorResult()
        {
            string placeId = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateGeoController();

            var expectedResult = A.Fake<IPlace>();
            _fakeGeoQueryExecutor
                .CallsTo(x => x.GetPlaceFromId(placeId))
                .Returns(expectedResult);

            // Act
            var result = controller.GetPlaceFromId(placeId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        public GeoController CreateGeoController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}