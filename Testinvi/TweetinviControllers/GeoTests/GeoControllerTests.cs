using System;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Models;

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
        public async Task GetPlaceFromId_ReturnsGeoQueryExecutorResult()
        {
            string placeId = Guid.NewGuid().ToString();

            // Arrange
            var controller = CreateGeoController();

            var expectedResult = A.Fake<IPlace>();
            _fakeGeoQueryExecutor
                .CallsTo(x => x.GetPlaceFromId(placeId))
                .Returns(expectedResult);

            // Act
            var result = await controller.GetPlaceFromId(placeId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private GeoController CreateGeoController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}