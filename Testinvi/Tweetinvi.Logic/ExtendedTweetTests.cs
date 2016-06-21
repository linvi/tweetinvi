using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Extensions;

namespace Testinvi.Tweetinvi.Core
{
    public class TweetParts
    {
        public TweetParts(string text)
        {
            var stringMatches = Regex.Match(text, @"^(?<prefix>(?:(?:@[a-z]+)\s)+)?(?<content>.+)()?");

            var prefix = stringMatches.Groups["prefix"];
            var content = stringMatches.Groups["content"];

            Prefix = prefix.Value;
            Content = content.Value;
            Suffix = string.Empty;
        }

        public string Content { get; private set; }
        public string Prefix { get; private set; }
        public string Suffix { get; private set; }
    }

    [TestClass]
    public class ExtendedTweetTests
    {
        [TestMethod]
        public void TestWithSimpleContent()
        {
            var content = "Tweetinvi I love it!";
            var parts = new TweetParts(content);

            Assert.AreEqual(parts.Content, content);
            Assert.AreEqual(parts.Prefix, "");
            Assert.AreEqual(parts.Suffix, "");
        }

        [TestMethod]
        public void TestWithPrefixAndContent()
        {
            var text = "@tweetinviapi Tweetinvi I love it!";
            var parts = new TweetParts(text);

            Assert.AreEqual(parts.Content, "Tweetinvi I love it!");
            Assert.AreEqual(parts.Prefix, "@tweetinviapi ");
            Assert.AreEqual(parts.Suffix, "");
        }

        [TestMethod]
        public void TestWithPrefixAndContentAndSuffix()
        {
            var text = "@sam @aileen " +
                       "Check out this photo of @YellowstoneNPS! " +
                       "It makes me want to go camping there this summer. " +
                       "Have you visited before?? nps.gov/yell/index.htm ";

            var parts = new TweetParts(text);

            Assert.AreEqual(parts.Prefix, "@sam @aileen ");
            Assert.AreEqual(parts.Prefix.Length, 13);
            
            Assert.AreEqual(parts.Content.TweetLength(), 140);

            Assert.AreEqual(parts.Suffix, "");
        }
    }
}
