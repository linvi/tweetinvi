using System;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.Geo;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.GeoTests
{
    [TestClass]
    public class GeoQueryExecutorTests
    {
        private FakeClassBuilder<GeoQueryExecutor> _fakeBuilder;
        private IGeoQueryGenerator _geoQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<GeoQueryExecutor>();
            _geoQueryGenerator = _fakeBuilder.GetFake<IGeoQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
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
            _twitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = controller.GetPlaceFromId(placeId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        private void ArrangeGetPlaceFromIdQueryGenerator(string placeId, string result)
        {
            A.CallTo(() => _geoQueryGenerator.GetPlaceFromIdQuery(placeId)).Returns(result);
        }

        public GeoQueryExecutor CreateGeoQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}