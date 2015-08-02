using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoQueryExecutorTests
    {
        private FakeClassBuilder<GeoQueryExecutor> _fakeBuilder;
        private Fake<IGeoQueryGenerator> _fakeGeoQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoQueryExecutor>();
            _fakeGeoQueryGenerator = _fakeBuilder.GetFake<IGeoQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public void GetPlaceFromId_ReturnsGeoQueryExecutorResult()
        {
            var expectedResult = A.Fake<IPlace>();

            // Arrange
            var controller = CreateGeoQueryExecutor();

            string placeId = Guid.NewGuid().ToString();
            string query = Guid.NewGuid().ToString();

            ArrangeGetPlaceFromIdQueryGenerator(placeId, query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

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

        public GeoQueryExecutor CreateGeoQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}