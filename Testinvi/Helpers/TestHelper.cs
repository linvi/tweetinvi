using System;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using FakeItEasy.ExtensionSyntax.Full;
using Tweetinvi.Core;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO;

namespace Testinvi.Helpers
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

        public static int GenerateRandomInt()
        {
            int result = new Random().Next();

            while (result == DefaultId())
            {
                result = new Random().Next();
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
            user.CallsTo(x => x.UserDTO).Returns(userDTO);
            return user;
        }

        public static ITweet GenerateTweet(ITweetDTO tweetDTO)
        {
            var tweet = A.Fake<ITweet>();
            tweet.CallsTo(x => x.TweetDTO).Returns(tweetDTO);
            return tweet;
        }
    }
}