using FakeItEasy;
using Tweetinvi.Controllers.Shared;
using Tweetinvi.Models;

namespace Testinvi.Helpers
{
    public class QueryParameterGeneratorTestHelper
    {
        public static string TweetIdentifier = "TWEET_ID";

        public static void InitializeQueryParameterGenerator(Fake<IQueryParameterGenerator> queryParameterGenerator)
        {
            queryParameterGenerator
                .CallsTo(x => x.GenerateTweetIdentifier(It.IsAny<ITweetIdentifier>()))
                .ReturnsLazily((ITweetIdentifier tweetId) => tweetId.Id.ToString());
        }
    }
}
