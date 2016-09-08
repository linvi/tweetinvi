using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tweetinvi.Core.Attributes;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;

namespace Testinvi.Tweetinvi.Core
{
    [TestClass]
    public class LanguageTests
    {
        [TestMethod]
        public void AllCodesAreUniqueToOneLanguage()
        {
            // Test that each language code only appears against one enum entry.
            //  If more than one entry had the same code, the behaviour of which one will be returned
            //  is undefined, so lets avoid it.
            Dictionary<string, Language> taken = new Dictionary<string, Language>();

            foreach (Language lang in Enum.GetValues(typeof(Language)))
            {
                LanguageAttribute attr = lang.GetAttributeOfType<LanguageAttribute>();

                foreach (string code in attr.Codes)
                {
                    // Rather than using assert, conditional & write details to console, making fixing any problem easier
                    if (taken.ContainsKey(code))
                    {
                        Console.WriteLine("Code {0} is against multiple languages: {1} & {2}", code, taken[code], lang);
                        Assert.Fail();
                    }
                    else
                    {
                        taken.Add(code, lang);
                    }
                }
            }
        }
    }
}
