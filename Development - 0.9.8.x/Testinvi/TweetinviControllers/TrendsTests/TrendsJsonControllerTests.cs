using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.TrendsTests
{
    [TestClass]
    public class TrendsJsonControllerTests
    {
        private FakeClassBuilder<TrendsJsonController> _fakeBuilder;

        private Fake<ITrendsQueryGenerator> _fakeTrendsQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsJsonController>();
            _fakeTrendsQueryGenerator = _fakeBuilder.GetFake<ITrendsQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithLocationId_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsJsonController();
            var query = TestHelper.GenerateString();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = TestHelper.GenerateString();

            _fakeTrendsQueryGenerator.CallsTo(x => x.GetPlaceTrendsAtQuery(locationId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetPlaceTrendsAt(locationId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithwoeIdLocation_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsJsonController();
            var query = TestHelper.GenerateString();
            var woeIdLocation = A.Fake<IWoeIdLocation>();
            var expectedResult = TestHelper.GenerateString();

            _fakeTrendsQueryGenerator.CallsTo(x => x.GetPlaceTrendsAtQuery(woeIdLocation)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = queryExecutor.GetPlaceTrendsAt(woeIdLocation);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        public TrendsJsonController CreateTrendsJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}