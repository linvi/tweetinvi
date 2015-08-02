using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Extensions;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class EnumerableExtensionsTests
    {
        private IEnumerable<TestClass> _emptyCollection;
        private IEnumerable<TestClass> _notMatchingCollection;
        private IEnumerable<TestClass> _singleMatchCollection;
        private IEnumerable<TestClass> _multipleMatchesCollection;

        [TestInitialize]
        public void TestInitialize()
        {
            _emptyCollection = Enumerable.Empty<TestClass>();
            _notMatchingCollection = new List<TestClass> { new TestClass { IsTrue = false } };
            _singleMatchCollection = new List<TestClass>
            {
                new TestClass { IsTrue = true },
                new TestClass { IsTrue = false }
            };

            _multipleMatchesCollection = new List<TestClass>
            {
                new TestClass { IsTrue = true },
                new TestClass { IsTrue = true }
            };
        }

        [TestMethod]
        public void JustOneOrDefault_IsEmpty_ReturnsNull()
        {
            // Arrange - Act
            var result = _emptyCollection.JustOneOrDefault(x => x.IsTrue);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void JustOneOrDefault_ConditionNeverMatched_ReturnsNull()
        {
            // Arrange - Act
            var result = _notMatchingCollection.JustOneOrDefault(x => x.IsTrue);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void JustOneOrDefault_OneMatch_ReturnsMatch()
        {
            // Arrange - Act
            var result = _singleMatchCollection.JustOneOrDefault(x => x.IsTrue);

            // Assert
            Assert.AreEqual(result, _singleMatchCollection.First());
        }

        [TestMethod]
        public void JustOneOrDefault_MultipleMatches_ReturnsNull()
        {
            // Arrange - Act
            var result = _multipleMatchesCollection.JustOneOrDefault(x => x.IsTrue);

            // Assert
            Assert.IsNull(result);
        }

        private class TestClass
        {
            public bool IsTrue { get; set; }
        }
    }
}