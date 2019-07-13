using System.Threading.Tasks;
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
        private Fake<ITrendsQueryGenerator> _fakeTrendsQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsQueryExecutor>();
            _fakeTrendsQueryGenerator = _fakeBuilder.GetFake<ITrendsQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public async Task GetPlaceTrendsAt_WithLocationId_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsQueryExecutor();
            var query = TestHelper.GenerateString();
            var locationId = TestHelper.GenerateRandomLong();
            var expectedResult = A.Fake<IPlaceTrends>();
            var expectedTwitterAccessorResults = new[] { expectedResult };

            _fakeTrendsQueryGenerator.CallsTo(x => x.GetPlaceTrendsAtQuery(locationId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedTwitterAccessorResults);

            // Act
            var result = await queryExecutor.GetPlaceTrendsAt(locationId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task GetPlaceTrendsAt_WithWoeIdLocation_ReturnsFirstObjectFromTheResults()
        {
            // Arrange
            var queryExecutor = CreateTrendsQueryExecutor();
            var query = TestHelper.GenerateString();
            var woeIdLocation = A.Fake<IWoeIdLocation>();
            var expectedResult = A.Fake<IPlaceTrends>();
            var expectedTwitterAccessorResults = new[] { expectedResult };

            _fakeTrendsQueryGenerator.CallsTo(x => x.GetPlaceTrendsAtQuery(woeIdLocation)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedTwitterAccessorResults);

            // Act
            var result = await queryExecutor.GetPlaceTrendsAt(woeIdLocation);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private TrendsQueryExecutor CreateTrendsQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}