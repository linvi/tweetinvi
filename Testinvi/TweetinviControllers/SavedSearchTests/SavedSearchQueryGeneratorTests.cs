using System;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Controllers.SavedSearch;
using Tweetinvi.Core.Interfaces.Models;

namespace Testinvi.TweetinviControllers.SavedSearchTests
{
    [TestClass]
    public class SavedSearchQueryGeneratorTests
    {
        private FakeClassBuilder<SavedSearchQueryGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<SavedSearchQueryGenerator>();
        }

        [TestMethod]
        public void GetSavedSearchesQuery_ReturnsResource()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();

            // Act
            var result = queryGenerator.GetSavedSearchesQuery();

            // Assert
            Assert.AreEqual(result, Resources.SavedSearches_GetList);
        }

        [TestMethod]
        public void GetDestroySavedSearchQuery_WithSavedSearch_ReturnsFormattedResource()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();
            var searchId = TestHelper.GenerateRandomLong();
            var savedSearch = A.Fake<ISavedSearch>();
            savedSearch.CallsTo(x => x.Id).Returns(searchId);

            // Act
            var result = queryGenerator.GetDestroySavedSearchQuery(savedSearch);

            // Assert
            string expectedResult = string.Format(Resources.SavedSearch_Destroy, searchId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetDestroySavedSearchQuery_WithNullSavedSearch_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();

            // Act
            var result = queryGenerator.GetDestroySavedSearchQuery(null);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetDestroySavedSearchQuery_WithSavedSearchAndIdBeingDefault_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();
            var searchId = TestHelper.DefaultId();
            var savedSearch = A.Fake<ISavedSearch>();
            savedSearch.CallsTo(x => x.Id).Returns(searchId);

            // Act
            var result = queryGenerator.GetDestroySavedSearchQuery(savedSearch);

            // Assert
            Assert.AreEqual(result, null);
        }

        [TestMethod]
        public void GetDestroySavedSearchQuery_WithSearchId_ReturnsFormattedResource()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();
            var searchId = TestHelper.GenerateRandomLong();

            // Act
            var result = queryGenerator.GetDestroySavedSearchQuery(searchId);

            // Assert
            string expectedResult = string.Format(Resources.SavedSearch_Destroy, searchId);
            Assert.AreEqual(result, expectedResult);
        }

        [TestMethod]
        public void GetDestroySavedSearchQuery_WithSearchIdBeingDefault_ReturnsNull()
        {
            // Arrange
            var queryGenerator = CreateSavedSearchQueryGenerator();
            var searchId = TestHelper.DefaultId();

            // Act
            var result = queryGenerator.GetDestroySavedSearchQuery(searchId);

            // Assert
            Assert.AreEqual(result, null);
        }

        public SavedSearchQueryGenerator CreateSavedSearchQueryGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}