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
            Assert.AreEqual(6, UnicodeHelper.UTF32Length("sa🚒osa"));
            Assert.AreEqual(11, UnicodeHelper.UTF32Length("ab 🎅🏼⛄️🎅🏼 yz"));
            Assert.AreEqual(5, UnicodeHelper.UTF32Length("🎅🏼⛄️🎅🏼"));
            Assert.AreEqual(5, UnicodeHelper.UTF32Length("\ud83c\udf85\ud83c\udffc\u26c4\ufe0f\ud83c\udf85\ud83c\udffc"));
            Assert.AreEqual(2, UnicodeHelper.UTF32Length("\ud83c\udfb8\u203c\ufe0f"));
            Assert.AreEqual(13, UnicodeHelper.UTF32Length("Cooper`s Hawk"));
        }
    }

    [TestClass]
    public class UnicodeHelperSubstringByTextElementsTests
    {
        [TestMethod]
        public void When_Input_Is_Null_Then_Result_Is_Null()
        {
            Assert.IsNull(UnicodeHelper.SubstringByTextElements(null, 0));
            Assert.IsNull(UnicodeHelper.SubstringByTextElements(null, 0, 1));
        }

        [TestMethod]
        public void When_StartIndex_Equals_Zero_Then_Result_Equals_Input()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 0);
            Assert.AreEqual(str, substr);
        }

        [TestMethod]
        public void When_StartIndex_GreaterThan_Zero_Then_Result_Contains_Part_Of_Input()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 1);
            Assert.AreEqual("bcd", substr);
        }

        [TestMethod]
        public void When_StartIndex_Equals_InputLength_Then_Result_Is_Empty()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, str.Length);
            Assert.AreEqual("", substr);
        }

        [TestMethod]
        public void When_StartIndex_GreaterThan_InputLength_Then_Result_Is_Empty()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, str.Length + 1);
            Assert.AreEqual("", substr);
        }

        [TestMethod]
        public void When_Input_Contains_Emojis_And_StartIndex_Equals_Zero_Then_Result_Equals_Input()
        {
            var str = "🎅🏼⛄️🎅🏼";
            var substr = UnicodeHelper.SubstringByTextElements(str, 0);
            Assert.AreEqual(str, substr);
        }

        [TestMethod]
        public void When_Input_Contains_Emojis_And_StartIndex_Is_After_Emojis_Then_Result_Contains_No_Emojis()
        {
            var str = "🎅🏼⛄️🎅🏼abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 5);
            Assert.AreEqual("abcd", substr);
        }

        [TestMethod]
        public void When_Input_Contains_Emojis_And_StartIndex_Is_Between_Emojis_Then_Result_Contains_Emojis()
        {
            var str = "🎅🏼⛄️🎅🏼abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 3);
            Assert.AreEqual("🎅🏼abcd", substr);
        }

        [TestMethod]
        public void When_StartIndex_Equals_Zero_And_Length_Equals_Zero_Then_Result_Is_Empty()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 0, 0);
            Assert.AreEqual("", substr);
        }

        [TestMethod]
        public void When_StartIndex_Equals_Zero_And_Length_Equals_Input_Length_Then_Result_Equals_Input()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 0, str.Length);
            Assert.AreEqual(str, substr);
        }

        [TestMethod]
        public void When_StartIndex_Equals_Zero_And_Length_Is_GreaterThan_InputLength_Then_Result_Equals_Input()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 0, str.Length + 1);
            Assert.AreEqual(str, substr);
        }

        [TestMethod]
        public void When_StartIndex_GreaterThan_Zero_And_Length_LessThan_Input_Length_Then_Result_Contains_Part_Of_Input()
        {
            var str = "abcd";
            var substr = UnicodeHelper.SubstringByTextElements(str, 1, 2);
            Assert.AreEqual("bc", substr);
        }

        [TestMethod]
        public void When_StartIndex_Equals_Zero_And_Length_LessThan_InputLength_Then_Result_Does_Not_Include_End_Of_Input()
        {
            var str = "What better way to celebrate Christmas than supporting your team in the Greg Shupe Christmas tourney @ Kent! (We play where it says 1)🎅🏼⛄️🎅🏼 https://t.co/oUeMIkyb5G";

            var substr = UnicodeHelper.SubstringByTextElements(str, 0, 140);

            var expectedSubstr = "What better way to celebrate Christmas than supporting your team in the Greg Shupe Christmas tourney @ Kent! (We play where it says 1)🎅🏼⛄️🎅🏼 ";
            Assert.AreEqual(expectedSubstr, substr);
        }

        [TestMethod]
        public void When_StartIndex_GreaterThan_Zero_And_Length_LessThan_InputLength_Then_Result_Does_Not_Include_Beginning_Or_End_Of_Input()
        {
            var str = "@sam @aileen Check out this photo of @YellowstoneNPS! It makes me want to go camping there this summer. Have you visited before?? nps.gov/yell/index.htm pic.twitter.com/e8bDiL6LI4";

            var substr = UnicodeHelper.SubstringByTextElements(str, 13, 140);

            var expectedSubstr = "Check out this photo of @YellowstoneNPS! It makes me want to go camping there this summer. Have you visited before?? nps.gov/yell/index.htm ";
            Assert.AreEqual(expectedSubstr, substr);
        }
    }
}
