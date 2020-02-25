using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Tweetinvi
{
    /// <summary>
    /// Access Twitter live feeds.
    /// </summary>
    public static class Stream
    {
        private static readonly IFactory<IAccountActivityStream> _accountActivityStreamFactory;

        static Stream()
        {
            _accountActivityStreamFactory = TweetinviContainer.Resolve<IFactory<IAccountActivityStream>>();
        }

        /// <summary>
        /// Create a stream notifying the client about everything that can happen to a user.
        /// </summary>
        public static IAccountActivityStream CreateAccountActivityStream(long accountUserId)
        {
            var client = new TwitterClient(null as IReadOnlyTwitterCredentials);
            var factories = _accountActivityStreamFactory.GenerateParameterOverrideWrapper("factories", client.Factories);
            var stream = _accountActivityStreamFactory.Create(factories);

            stream.AccountUserId = accountUserId;

            return stream;
        }

        public static IAccountActivityStream CreateAccountActivityStream(string userId)
        {
            var longUserId = long.Parse(userId);

            return CreateAccountActivityStream(longUserId);
        }
    }
}