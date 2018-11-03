using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.TrendsTests
{
    [TestClass]
    public class TrendsJsonControllerTests
    {
        private FakeClassBuilder<TrendsJsonController> _fakeBuilder;

        private ITrendsQueryGenerator _trendsQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsJsonController>();
            _trendsQueryGenerator = _fakeBuilder.GetFake<ITrendsQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithLocationId_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsJsonController();
            var query = TestHelper.GenerateString();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = TestHelper.GenerateString();

            A.CallTo(() => _trendsQueryGenerator.GetPlaceTrendsAtQuery(locationId)).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

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

            A.CallTo(() => _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation)).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

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