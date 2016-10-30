using Microsoft.Extensions.Configuration;

namespace Examplinvi.NETCore
{
    /// <summary>
    /// Provides access to Twitter API credentials
    /// </summary>
    public class APICredentials
    {
        public string ConsumerKey { get; private set; }
        public string ConsumerSecret { get; private set; }
        public string AccessToken { get; private set; }
        public string AccessTokenSecret { get; private set; }

        public APICredentials()
        {
            GetAPICredentials();
        }

        /// <summary>
        /// Uses .NET Core configuration API for extracting credentials from environment variables.
        /// </summary>
        private void GetAPICredentials()
        {
            var builder = new ConfigurationBuilder();
            builder.AddEnvironmentVariables();

            var configuration = builder.Build();

            ConsumerKey = configuration["ConsumerKey"];
            ConsumerSecret = configuration["ConsumerSecret"];
            AccessToken = configuration["AccessToken"];
            AccessTokenSecret = configuration["AccessTokenSecret"];
        }
    }
}
