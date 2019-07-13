using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Factories;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchControllerTests
    {
        private FakeClassBuilder<SavedSearchController> _fakeBuilder;
        private Fake<ISavedSearchQueryExecutor> _fakeSavedSearchQueryExecutor;
        private Fake<ISavedSearchFactory> _fakeSavedSearchFactory;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchController>();
            _fakeSavedSearchQueryExecutor = _fakeBuilder.GetFake<ISavedSearchQueryExecutor>();
            _fakeSavedSearchFactory = _fakeBuilder.GetFake<ISavedSearchFactory>();
        }

        [TestMethod]
        public async Task GetSavedSearches_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateSavedSearchController();
            IEnumerable<ISavedSearchDTO> expectedDTOResult = new List<ISavedSearchDTO>();
            IEnumerable<ISavedSearch> expectedResult = new List<ISavedSearch>();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.GetSavedSearches()).Returns(expectedDTOResult);
            _fakeSavedSearchFactory.CallsTo(x => x.GenerateSavedSearchesFromDTOs(expectedDTOResult)).Returns(expectedResult);

            // Act
            var result = await controller.GetSavedSearches();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public async Task DestroySavedSearch_WithSavedSearchObject_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var result1 = await DestroySavedSearch_WithSavedSearchObject(true);
            var result2 = await DestroySavedSearch_WithSavedSearchObject(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroySavedSearch_WithSavedSearchObject(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchController();
            var savedSearch = A.Fake<ISavedSearch>();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.DestroySavedSearch(savedSearch)).Returns(expectedResult);

            // Act
            return await controller.DestroySavedSearch(savedSearch);
        }

        [TestMethod]
        public async Task DestroySavedSearch_WithSavedSearchId_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var result1 = await DestroySavedSearch_WithSavedSearchId(true);
            var result2 = await DestroySavedSearch_WithSavedSearchId(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private async Task<bool> DestroySavedSearch_WithSavedSearchId(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchController();
            var searchId = TestHelper.GenerateRandomLong();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.DestroySavedSearch(searchId)).Returns(expectedResult);

            // Act
            return await controller.DestroySavedSearch(searchId);
        }

        private SavedSearchController CreateSavedSearchController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}