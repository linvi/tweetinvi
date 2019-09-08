using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace xUnitinvi.TestHelpers
{
    [ExcludeFromCodeCoverage]
    public class TestHelper
    {
        public static string GenerateString()
        {
            return Guid.NewGuid().ToString();
        }

        public static long GenerateRandomLong()
        {
            long result = Int64.MaxValue - new Random().Next();

            while (result == DefaultId())
            {
                result = Int64.MaxValue - new Random().Next();
            }

            return result;
        }

        public static int GenerateRandomInt(int? maxValue = null)
        {
            if (maxValue == null)
            {
                maxValue = Int32.MaxValue;
            }

            int result = Math.Min(new Random().Next(), maxValue.Value);

            while (result == DefaultId())
            {
                result = Math.Min(new Random().Next(), maxValue.Value);
            }

            return result;
        }

        public static long DefaultId()
        {
            return TweetinviSettings.DEFAULT_ID;
        }

        public static IUser GenerateUser(IUserDTO userDTO)
        {
            var user = A.Fake<IUser>();
            A.CallTo(() => user.UserDTO).Returns(userDTO);
            return user;
        }

        public static IUser GenerateUser(string screenName)
        {
            var user = A.Fake<IUser>();
            A.CallTo(() => user.ScreenName).Returns(screenName);
            return user;
        }

        public static ITweet GenerateTweet(ITweetDTO tweetDTO)
        {
            var tweet = A.Fake<ITweet>();
            A.CallTo(() => tweet.TweetDTO).Returns(tweetDTO);
            return tweet;
        }
    }
}