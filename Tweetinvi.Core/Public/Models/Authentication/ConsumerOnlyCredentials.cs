using Tweetinvi.Models;

namespace Tweetinvi.Core.Public.Models.Authentication
{
    public interface IConsumerOnlyCredentials : IConsumerCredentials
    {

    }

    public class ConsumerOnlyCredentials : ConsumerCredentials, IConsumerOnlyCredentials
    {
        public ConsumerOnlyCredentials(string consumerKey, string consumerSecret) : base(consumerKey, consumerSecret)
        {
        }
    }
}
