using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers
{
    public interface IAccountActivityQueryGenerator
    {
        string GetCreateAccountActivityWebhookQuery(ICreateAccountActivityWebhookParameters parameters);
        string GetAccountActivityWebhookEnvironmentsQuery(IGetAccountActivityWebhookEnvironmentsParameters parameters);
        string GetAccountActivityEnvironmentWebhooksQuery(IGetAccountActivityEnvironmentWebhooksParameters parameters);
        string GetDeleteAccountActivityWebhookQuery(IDeleteAccountActivityWebhookParameters parameters);
        string GetTriggerAccountActivityWebhookCRCQuery(ITriggerAccountActivityWebhookCRCParameters parameters);
        string GetSubscribeToAccountActivityQuery(ISubscribeToAccountActivityParameters parameters);
        string GetUnsubscribeToAccountActivityQuery(IUnsubscribeFromAccountActivityParameters parameters);
        string GetCountAccountActivitySubscriptionsQuery(ICountAccountActivitySubscriptionsParameters parameters);
        string GetIsAccountSubscribedToAccountActivityQuery(IIsAccountSubscribedToAccountActivityParameters parameters);
        string GetAccountActivitySubscriptionsQuery(IGetAccountActivitySubscriptionsParameters parameters);
    }

    public class AccountActivityQueryGenerator : IAccountActivityQueryGenerator
    {
        public string GetCreateAccountActivityWebhookQuery(ICreateAccountActivityWebhookParameters parameters)
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

        public string GetAccountActivityEnvironmentWebhooksQuery(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            var query = new StringBuilder($"{Resources.Webhooks_AccountActivity_All}/{parameters.Environment}/webhooks.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetDeleteAccountActivityWebhookQuery(IDeleteAccountActivityWebhookParameters parameters)
        {
            var query = new StringBuilder($"{Resources.Webhooks_AccountActivity_All}/{parameters.Environment}/webhooks/{parameters.WebhookId}.json?");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetTriggerAccountActivityWebhookCRCQuery(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            var query = new StringBuilder($"{Resources.Webhooks_AccountActivity_All}/{parameters.Environment}/webhooks/{parameters.WebhookId}.json?");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetSubscribeToAccountActivityQuery(ISubscribeToAccountActivityParameters parameters)
        {
            var query = new StringBuilder($"https://api.twitter.com/1.1/account_activity/all/{parameters.Environment}/subscriptions.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetUnsubscribeToAccountActivityQuery(IUnsubscribeFromAccountActivityParameters parameters)
        {
            var query = new StringBuilder($"https://api.twitter.com/1.1/account_activity/all/{parameters.Environment}/subscriptions/{parameters.UserId}.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetCountAccountActivitySubscriptionsQuery(ICountAccountActivitySubscriptionsParameters parameters)
        {
            var query = new StringBuilder("https://api.twitter.com/1.1/account_activity/all/subscriptions/count.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetIsAccountSubscribedToAccountActivityQuery(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            var query = new StringBuilder($"https://api.twitter.com/1.1/account_activity/all/{parameters.Environment}/subscriptions.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }

        public string GetAccountActivitySubscriptionsQuery(IGetAccountActivitySubscriptionsParameters parameters)
        {
            var query = new StringBuilder($"https://api.twitter.com/1.1/account_activity/all/{parameters.Environment}/subscriptions/list.json");
            query.AddFormattedParameterToQuery(parameters.FormattedCustomQueryParameters);
            return query.ToString();
        }
    }
}