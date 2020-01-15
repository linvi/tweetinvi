using System.Threading.Tasks;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public interface IHelpClient
    {
        /// <inheritdoc cref="GetTwitterConfiguration(IGetTwitterConfigurationParameters)"/>
        Task<ITwitterConfiguration> GetTwitterConfiguration();

        /// <summary>
        /// Get the Twitter API configuration
        /// </summary>
        /// <para> https://developer.twitter.com/en/docs/developer-utilities/configuration/api-reference/get-help-configuration </para>
        /// <returns>Twitter official configuration</returns>
        Task<ITwitterConfiguration> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters);
    }
}