using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Factories;
using Tweetinvi.Core.Interfaces.Models;

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
        public void GetSavedSearches_ReturnsQueryExecutor()
        {
            // Arrange
            var controller = CreateSavedSearchController();
            IEnumerable<ISavedSearchDTO> expectedDTOResult = new List<ISavedSearchDTO>();
            IEnumerable<ISavedSearch> expectedResult = new List<ISavedSearch>();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.GetSavedSearches()).Returns(expectedDTOResult);
            _fakeSavedSearchFactory.CallsTo(x => x.GenerateSavedSearchesFromDTOs(expectedDTOResult)).Returns(expectedResult);

            // Act
            var result = controller.GetSavedSearches();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchObject_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var result1 = DestroySavedSearch_WithSavedSearchObject(true);
            var result2 = DestroySavedSearch_WithSavedSearchObject(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private bool DestroySavedSearch_WithSavedSearchObject(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchController();
            var savedSearch = A.Fake<ISavedSearch>();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.DestroySavedSearch(savedSearch)).Returns(expectedResult);

            // Act
            return controller.DestroySavedSearch(savedSearch);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchId_ReturnsQueryExecutor()
        {
            // Arrange - Act
            var result1 = DestroySavedSearch_WithSavedSearchId(true);
            var result2 = DestroySavedSearch_WithSavedSearchId(false);

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        private bool DestroySavedSearch_WithSavedSearchId(bool expectedResult)
        {
            // Arrange
            var controller = CreateSavedSearchController();
            var searchId = TestHelper.GenerateRandomLong();

            _fakeSavedSearchQueryExecutor.CallsTo(x => x.DestroySavedSearch(searchId)).Returns(expectedResult);

            // Act
            return controller.DestroySavedSearch(searchId);
        }

        public SavedSearchController CreateSavedSearchController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}