using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.TrendsTests
{
    [TestClass]
    public class TrendsControllerTests
    {
        private FakeClassBuilder<TrendsController> _fakeBuilder;
        private Fake<ITrendsQueryExecutor> _fakeTrendsQueryExecutor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsController>();
            _fakeTrendsQueryExecutor = _fakeBuilder.GetFake<ITrendsQueryExecutor>();
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithLocationId_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTrendsController();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = A.Fake<IPlaceTrends>();

            _fakeTrendsQueryExecutor.CallsTo(x => x.GetPlaceTrendsAt(locationId)).Returns(expectedResult);

            // Act
            var result = controller.GetPlaceTrendsAt(locationId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithWoeIdLocation_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateTrendsController();
            var woeIdLocation = A.Fake<IWoeIdLocation>();
            var expectedResult = A.Fake<IPlaceTrends>();

            _fakeTrendsQueryExecutor.CallsTo(x => x.GetPlaceTrendsAt(woeIdLocation)).Returns(expectedResult);

            // Act
            var result = controller.GetPlaceTrendsAt(woeIdLocation);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        public TrendsController CreateTrendsController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}