using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testinvi.Helpers;
using Tweetinvi;
using Tweetinvi.Controllers.Shared;

namespace Testinvi.TweetinviControllers.Shared
{
    public class QueryParameterGeneratorTests
    {
        private FakeClassBuilder<QueryParameterGenerator> _fakeBuilder;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeBuilder = new FakeClassBuilder<QueryParameterGenerator>();
        }

        #region IsSinceIdDefined

        [TestMethod]
        public void IsSinceIdDefined_SinceIdIsDefault_False()
        {
            // Arrange
            var queryParameterGenerator = CreateQueryParameterGenerator();

            // Act
            var result = queryParameterGenerator.GenerateSinceIdParameter(TweetinviSettings.DEFAULT_ID);

            // Assert
            Assert.AreEqual(result, string.Empty);
        }

        [TestMethod]
        public void IsSinceIdDefined_SinceIdRandom_True()
        {
            // Arrange
            var queryParameterGenerator = CreateQueryParameterGenerator();

            // Act
            var result = queryParameterGenerator.GenerateSinceIdParameter(TestHelper.GenerateRandomLong());

            // Assert
            Assert.AreEqual(result, string.Format(""));
        }

        #endregion 

        public QueryParameterGenerator CreateQueryParameterGenerator()
        {
            return _fakeBuilder.GenerateClass();
        }
    }
}