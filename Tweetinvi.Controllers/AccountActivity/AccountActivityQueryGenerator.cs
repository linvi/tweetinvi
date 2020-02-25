using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers
{
    public interface IWebhooksQueryGenerator
    {
        string GetRegisterAccountActivityWebhookQuery(IRegisterAccountActivityWebhookParameters parameters);
        string GetAccountActivityWebhookEnvironmentsQuery(IGetAccountActivityWebhookEnvironmentsParameters parameters);
        string GetRemoveAccountActivityWebhookQuery(IRemoveAccountActivityWebhookParameters parameters);
    }

    public class WebhooksQueryGenerator : IWebhooksQueryGenerator
    {
        public string GetRegisterAccountActivityWebhookQuery(IRegisterAccountActivityWebhookParameters parameters)
        {
            var query = new StringBuilder($"{Resources.Webhooks_AccountActivity_All}/{parameters.Environment}/webhooks.json?");

            query.AddParameterToQuery("url", parameters.WebhookUrl);
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetAccountActivityWebhookEnvironmentsQuery(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            var query = new StringBuilder(Resources.Webhooks_AccountActivity_GetAllWebhooks);

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }

        public string GetRemoveAccountActivityWebhookQuery(IRemoveAccountActivityWebhookParameters parameters)
        {
            var query = new StringBuilder($"{Resources.Webhooks_AccountActivity_All}/{parameters.Environment}/webhooks/{parameters.WebhookId}.json?");

            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);

            return query.ToString();
        }
    }
}