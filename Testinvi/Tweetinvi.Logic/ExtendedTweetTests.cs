using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Extensions;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class ExtendedTweetTests
    {
        [TestMethod]
        public void TestWithSimpleContent()
        {
            var content = "Tweetinvi I love it!";
            var parts = StringExtension.TweetParts(content);

            Assert.AreEqual(parts.Content, content);
            Assert.AreEqual(parts.Prefix, "");
            Assert.AreEqual(parts.Mentions.Length, 0);
        }

        [TestMethod]
        public void TestWithPrefixAndContent()
        {
            var text = "@tweetinviapi Tweetinvi I love it!";
            var parts = StringExtension.TweetParts(text);

            Assert.AreEqual(parts.Content, "Tweetinvi I love it!");
            Assert.AreEqual(parts.Prefix, "@tweetinviapi ");
            Assert.AreEqual(parts.Mentions.Length, 1);
        }

        [TestMethod]
        public void TestWithPrefixAndContentAndSuffix()
        {
            var text = "@sam @aileen " +
                       "Check out this photo of @YellowstoneNPS! " +
                       "It makes me want to go camping there this summer. " +
                       "Have you visited before?? nps.gov/yell/index.htm";

            var parts = StringExtension.TweetParts(text);

            Assert.AreEqual(parts.Prefix, "@sam @aileen ");
            Assert.AreEqual(parts.Prefix.Length, 13);
            
            Assert.AreEqual(parts.Content.TweetLength(), 140);
            Assert.AreEqual(parts.Mentions.Length, 2);
        }
    }
}
