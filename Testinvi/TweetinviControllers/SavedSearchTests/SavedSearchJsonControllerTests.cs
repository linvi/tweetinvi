using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchJsonControllerTests
    {
        private FakeClassBuilder<SavedSearchJsonController> _fakeBuilder;
        private Fake<ISavedSearchQueryGenerator> _fakeSavedSearchQueryGenerator;
        private Fake<ITwitterAccessor> _fakeTwitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchJsonController>();
            _fakeSavedSearchQueryGenerator = _fakeBuilder.GetFake<ISavedSearchQueryGenerator>();
            _fakeTwitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>();
        }

        [TestMethod]
        public void GetSavedSearches_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchJsonController();
            string query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetSavedSearchesQuery()).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

            // Act
            var result = controller.GetSavedSearches();

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchObject_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchJsonController();
            string query = TestHelper.GenerateString();
            string expectedResult = TestHelper.GenerateString();
            var savedSearch = A.Fake<ISavedSearch>();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(savedSearch)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = controller.DestroySavedSearch(savedSearch);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void DestroySavedSearch_WithSavedSearchId_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchJsonController();
            string query = TestHelper.GenerateString();
            string expectedResult = TestHelper.GenerateString();
            var searchId = TestHelper.GenerateRandomLong();

            _fakeSavedSearchQueryGenerator.CallsTo(x => x.GetDestroySavedSearchQuery(searchId)).Returns(query);
            _fakeTwitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

            // Act
            var result = controller.DestroySavedSearch(searchId);

            // Assert
            Assert.AreEqual(result, expectedResult);
        }

        public SavedSearchJsonController CreateSavedSearchJsonController()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}