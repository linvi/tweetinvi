using System;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAccountActivityClientRequiredParametersValidator : IAccountActivityClientParametersValidator
    {
    }

    public class AccountActivityClientRequiredParametersValidator : IAccountActivityClientRequiredParametersValidator
    {
        public void Validate(ICreateAccountActivityWebhookParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }

            if (parameters.WebhookUrl == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.WebhookUrl)}");
            }
        }

        public void Validate(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }
        }

        public void Validate(IDeleteAccountActivityWebhookParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }

            if (parameters.WebhookId == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.WebhookId)}");
            }
        }

        public void Validate(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }

            if (parameters.WebhookId == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.WebhookId)}");
            }
        }

        public void Validate(ISubscribeToAccountActivityParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }
        }

        public void Validate(ICountAccountActivitySubscriptionsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
        }

        public void Validate(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }
        }

        public void Validate(IGetAccountActivitySubscriptionsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }
        }

        public void Validate(IUnsubscribeFromAccountActivityParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Environment == null)
            {
                throw new ArgumentNullException($"${nameof(parameters.Environment)}");
            }

            if (parameters.UserId <= 0)
            {
                throw new ArgumentException($"${nameof(parameters.UserId)}");
            }
        }
    }
}