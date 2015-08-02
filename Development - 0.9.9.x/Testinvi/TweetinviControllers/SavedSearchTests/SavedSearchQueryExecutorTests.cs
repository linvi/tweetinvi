using System.Collections.Generic;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchQueryExecutorTests
    {
        private FakeClassBuilder<SavedSearchQueryExecutor> _fakeBuilder;
        private Fake<ISavedSearchQueryGenerator> _fakeSavedSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchQueryExecutor>();
            _fakeSavedSearchQueryGenerator = _fakeBuilder.GetFake<ISavedSearchQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public void GetSavedSearches_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchQueryExecutor();
            string query = TestHelper.GenerateString();
            IEnumerable<ISavedSearchDTO> expectedResult = new List<ISavedSearchDTO>();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetSavedSearchesQuery()).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteGETQuery(query, expectedResult);

            // Act
            var result = controller.GetSavedSearches();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchObject_ReturnsTwitterAccessorResult()
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
            var controller = CreateSavedSearchQueryExecutor();
            string query = TestHelper.GenerateString();
            var savedSearch = A.Fake<ISavedSearch>();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(savedSearch)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return controller.DestroySavedSearch(savedSearch);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchId_ReturnsTwitterAccessorResult()
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
            var controller = CreateSavedSearchQueryExecutor();
            string query = TestHelper.GenerateString();
            var searchId = TestHelper.GenerateRandomLong();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(searchId)).Returns(query);
            _fakeTwitterAccessor.ArrangeTryExecutePOSTQuery(query, expectedResult);

            // Act
            return controller.DestroySavedSearch(searchId);
        }

        public SavedSearchQueryExecutor CreateSavedSearchQueryExecutor()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}