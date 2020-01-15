using System.Threading.Tasks;
using Tweetinvi.Client.Requesters;
using Tweetinvi.Models.DTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    public class HelpClient : IHelpClient
    {
        private readonly IHelpRequester _helpRequester;

        public HelpClient(IInternalHelpRequester helpRequester)
        {
            _helpRequester = helpRequester;
        }

        public Task<ITwitterConfiguration> GetTwitterConfiguration()
        {
            return GetTwitterConfiguration(new GetTwitterConfigurationParameters());
        }

        public async Task<ITwitterConfiguration> GetTwitterConfiguration(IGetTwitterConfigurationParameters parameters)
        {
            var twitterResult = await _helpRequester.GetTwitterConfiguration(parameters).ConfigureAwait(false);
            return twitterResult?.DataTransferObject;
        }
    }
}