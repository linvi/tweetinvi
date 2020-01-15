using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Help
{
    public class HelpQueryGenerator : IHelpQueryGenerator
    {
        public string GetRateLimitsQuery(IGetRateLimitsParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetRateLimit);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetTwitterConfigurationQuery(IGetTwitterConfigurationParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetTwitterConfiguration);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetSupportedLanguagesQuery(IGetSupportedLanguagesParameters parameters)
        {
            var query = new StringBuilder(Resources.Help_GetSupportedLanguages);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }
    }
}