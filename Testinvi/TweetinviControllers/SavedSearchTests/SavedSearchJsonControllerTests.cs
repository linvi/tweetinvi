using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Testinvi.SetupHelpers;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchJsonControllerTests
    {
        private FakeClassBuilder<SavedSearchJsonController> _fakeBuilder;

        private ISavedSearchQueryGenerator _savedSearchQueryGenerator;
        private ITwitterAccessor _twitterAccessor;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchJsonController>();
            _savedSearchQueryGenerator = _fakeBuilder.GetFake<ISavedSearchQueryGenerator>().FakedObject;
            _twitterAccessor = _fakeBuilder.GetFake<ITwitterAccessor>().FakedObject;
        }

        [TestMethod]
        public void GetSavedSearches_ReturnsTwitterAccessorResult()
        {
            // Arrange
            var controller = CreateSavedSearchJsonController();
            string query = TestHelper.GenerateString();
            var expectedResult = TestHelper.GenerateString();

            A.CallTo(() => _savedSearchQueryGenerator.GetSavedSearchesQuery()).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonGETQuery(query, expectedResult);

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

            A.CallTo(() => _savedSearchQueryGenerator.GetDestroySavedSearchQuery(savedSearch)).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

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

            A.CallTo(() => _savedSearchQueryGenerator.GetDestroySavedSearchQuery(searchId)).Returns(query);
            _twitterAccessor.ArrangeExecuteJsonPOSTQuery(query, expectedResult);

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