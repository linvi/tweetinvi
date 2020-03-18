using Tweetinvi.Core.Helpers;

namespace Tweetinvi.Models
{
    public interface IConsumerCredentials : IReadOnlyConsumerCredentials
    {
        /// <summary>
        /// Key identifying a specific consumer application
        /// </summary>
        new string ConsumerKey { get; set; }

        /// <summary>
        /// Secret Key identifying a specific consumer application
        /// </summary>
        new string ConsumerSecret { get; set; }

        /// <summary>
        /// Token required for Application Only Authentication
        /// </summary>
        new string BearerToken { get; set; }

        /// <summary>
        /// Are credentials correctly set up for application only authentication.
        /// </summary>
        bool AreSetupForApplicationAuthentication();
    }

    public class ConsumerCredentials : IConsumerCredentials
    {
        public ConsumerCredentials() { }

        public ConsumerCredentials(string consumerKey, string consumerSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
        }

        public ConsumerCredentials(IReadOnlyConsumerCredentials source)
        {
            if (source == null)
            {
                return;
            }

            ConsumerKey = source.ConsumerKey;
            ConsumerSecret = source.ConsumerSecret;
            BearerToken = source.BearerToken;
        }

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }

        public string BearerToken { get; set; }

        public bool AreSetupForApplicationAuthentication()
        {
            return !string.IsNullOrEmpty(ConsumerKey) &&
                   !string.IsNullOrEmpty(ConsumerSecret);
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj?.GetHashCode();
        }

        public override int GetHashCode()
        {
            return CredentialsHashCodeGenerator.CreateHash(this).GetHashCode();
        }
    }
}