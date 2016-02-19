using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.Trends;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.TrendsTests
{
    [TestClass]
    public class TrendsQueryGeneratorTests
    {
        private FakeClassBuilder<TrendsQueryGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<TrendsQueryGenerator>();
        }

        [TestMethod]
        public void GetPlaceTrendsAtQuery_WithLocationId_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();
            var locationId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetPlaceTrendsAtQuery(locationId);

            // Assert
            var expectedQuery = string.Format(Resources.Trends_GetTrendsFromWoeId, locationId);
            Assert.AreEqual(result, expectedQuery);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetPlaceTrendsAtQuery_WithNullWoeIdLocation_ThrowsArgumentException()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();

            // Act
            queryGenerator.GetPlaceTrendsAtQuery(null);
        }

        [TestMethod]
        public void GetPlaceTrendsAtQuery_WithWoeIdLocation_ReturnsExpectedQuery()
        {
            // Arrange
            var queryGenerator = CreateTrendsQueryGenerator();
            var locationId = TestHelper.GenerateRandomLong();

            var woeIdLocation = A.Fake<IWoeIdLocation>();
            woeIdLocation.CallsTo(x => x.WoeId).Returns(locationId);

            // Act
            var result = queryGenerator.GetPlaceTrendsAtQuery(woeIdLocation);

            // Assert
            var expectedQuery = string.Format(Resources.Trends_GetTrendsFromWoeId, locationId);
            Assert.AreEqual(result, expectedQuery);
        }

        public TrendsQueryGenerator CreateTrendsQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}