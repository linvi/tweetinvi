using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAccountActivityClientParametersValidator
    {
        void Validate(ICreateAccountActivityWebhookParameters parameters);
        void Validate(IGetAccountActivityWebhookEnvironmentsParameters parameters);
        void Validate(IGetAccountActivityEnvironmentWebhooksParameters parameters);
        void Validate(IDeleteAccountActivityWebhookParameters parameters);
        void Validate(ITriggerAccountActivityWebhookCRCParameters parameters);
        void Validate(ISubscribeToAccountActivityParameters parameters);
        void Validate(ICountAccountActivitySubscriptionsParameters parameters);
        void Validate(IIsAccountSubscribedToAccountActivityParameters parameters);
        void Validate(IGetAccountActivitySubscriptionsParameters parameters);
        void Validate(IUnsubscribeFromAccountActivityParameters parameters);
    }

    public class AccountActivityClientParametersValidator : IAccountActivityClientParametersValidator
    {
        private readonly IAccountActivityClientRequiredParametersValidator _activityClientRequiredParametersValidator;

        public AccountActivityClientParametersValidator(IAccountActivityClientRequiredParametersValidator activityClientRequiredParametersValidator)
        {
            _activityClientRequiredParametersValidator = activityClientRequiredParametersValidator;
        }

        public void Validate(ICreateAccountActivityWebhookParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivityWebhookEnvironmentsParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivityEnvironmentWebhooksParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IDeleteAccountActivityWebhookParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(ITriggerAccountActivityWebhookCRCParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(ISubscribeToAccountActivityParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(ICountAccountActivitySubscriptionsParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IIsAccountSubscribedToAccountActivityParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IGetAccountActivitySubscriptionsParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUnsubscribeFromAccountActivityParameters parameters)
        {
            _activityClientRequiredParametersValidator.Validate(parameters);
        }
    }
}