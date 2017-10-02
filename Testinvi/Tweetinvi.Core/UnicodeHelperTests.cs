using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Core.Helpers;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class UnicodeHelperTests
    {
        [TestMethod]
        public void UnicodeLength()
        {
            Assert.AreEqual(UnicodeHelper.UTF32Length("sa🚒osa"), 6);
            Assert.AreEqual(UnicodeHelper.UTF32Length("ab 🎅🏼⛄️🎅🏼 yz"), 12);
            Assert.AreEqual(UnicodeHelper.UTF32Length("🎅🏼⛄️🎅🏼"), 6);
            Assert.AreEqual(UnicodeHelper.UTF32Length("Cooper`s Hawk"), 13);
        }

        [TestMethod]
        public void UnicodeSubstring()
        {
            // Arrange
            var str = "What better way to celebrate Christmas than supporting your team in the Greg Shupe Christmas tourney @ Kent! (We play where it says 1)🎅🏼⛄️🎅🏼 https://t.co/oUeMIkyb5G";

            // Act
            var substr = UnicodeHelper.UnicodeSubstring(str, 141);

            // Assert
            Assert.AreEqual(substr, " https://t.co/oUeMIkyb5G");
        }
    }
}
