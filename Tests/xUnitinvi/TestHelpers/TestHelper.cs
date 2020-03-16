using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace xUnitinvi.TestHelpers
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        public static string GenerateString()
        {
            return Guid.NewGuid().ToString();
        }

        public static int GenerateRandomInt(int? maxValue)
        {
            if (maxValue == null)
            {
                maxValue = int.MaxValue;
            }

            var result = Math.Min(new Random().Next(), maxValue.Value);

            while (result == 0)
            {
                result = Math.Min(new Random().Next(), maxValue.Value);
            }

            return result;
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