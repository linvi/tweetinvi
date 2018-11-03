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
    public class TrendsQueryExecutorTests
    {
        private FakeClassBuilder<TrendsQueryExecutor> _fakeBuilder;

        private ITrendsQueryGenerator _trendsQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsQueryExecutor>();
            _trendsQueryGenerator = _fakeBuilder.GetFake<ITrendsQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithLocationId_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsQueryExecutor();
            var query = TestHelper.GenerateString();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = A.Fake<IPlaceTrends>();
            var expectedTwitterAccessorResults = new[] { expectedResult };

            A.CallTo(() => _trendsQueryGenerator.GetPlaceTrendsAtQuery(locationId)).Returns(query);
            _twitterAccessor.ArrangeExecuteGETQuery(query, expectedTwitterAccessorResults);

            // Act
            var result = queryExecutor.GetPlaceTrendsAt(locationId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetPlaceTrendsAt_WithwoeIdLocation_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsQueryExecutor();
            var query = TestHelper.GenerateString();
            var woeIdLocation = A.Fake<IWoeIdLocation>();
            var expectedResult = A.Fake<IPlaceTrends>();
            var expectedTwitterAccessorResults = new[] { expectedResult };

            A.CallTo(() => _trendsQueryGenerator.GetPlaceTrendsAtQuery(woeIdLocation)).Returns(query);
            _twitterAccessor.ArrangeExecuteGETQuery(query, expectedTwitterAccessorResults);

            // Act
            var result = queryExecutor.GetPlaceTrendsAt(woeIdLocation);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        public TrendsQueryExecutor CreateTrendsQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}