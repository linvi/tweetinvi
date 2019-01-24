using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Controllers.Search;

namespace Testinvi.TweetinviControllers.SearchTests
{
    [TestClass]
    public class SearchQueryParameterTests
    {
        [TestMethod]
        public void TestGenerateSearchQueryParameterStringDoesNotUrlEncode()
        {
            // Arrange
            const string expected = "Something that will get URL encoded";
            ISearchQueryValidator queryValidator = A.Fake<ISearchQueryValidator>();
            SearchQueryParameterGenerator paramGenerator = new SearchQueryParameterGenerator(queryValidator);

            // Act
            string actual = paramGenerator.GenerateSearchQueryParameter(expected);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
