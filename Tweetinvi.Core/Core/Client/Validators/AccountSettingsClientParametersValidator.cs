using System;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IAccountSettingsClientParametersValidator
    {
        void Validate(IGetAccountSettingsParameters parameters);
        void Validate(IUpdateAccountSettingsParameters parameters);
        void Validate(IUpdateProfileParameters parameters);
        void Validate(IUpdateProfileImageParameters parameters);
        void Validate(IUpdateProfileBannerParameters parameters);
        void Validate(IRemoveProfileBannerParameters parameters);
    }

    public interface IInternalAccountSettingsClientParametersValidator : IAccountSettingsClientParametersValidator
    {
        void Initialize(ITwitterClient client);
    }

    public class AccountSettingsClientParametersValidator : IInternalAccountSettingsClientParametersValidator
    {
        private readonly IAccountSettingsClientRequiredParametersValidator _accountSettingsClientRequiredParametersValidator;
        private ITwitterClient _client;

        public AccountSettingsClientParametersValidator(IAccountSettingsClientRequiredParametersValidator accountSettingsClientRequiredParametersValidator)
        {
            _accountSettingsClientRequiredParametersValidator = accountSettingsClientRequiredParametersValidator;
        }
        
        public void Initialize(ITwitterClient client)
        {
            _client = client;
        }
        
        private TwitterLimits Limits => _client.Config.Limits;

        public void Validate(IGetAccountSettingsParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void Validate(IUpdateAccountSettingsParameters parameters)
        {
            _accountSettingsClientRequiredParametersValidator.Validate(parameters);
            
            if (!parameters.DisplayLanguage.IsADisplayLanguage())
            {
                throw new ArgumentException("As of 2019-10-06 this language is not supported by Twitter", $"{nameof(parameters)}.{nameof(parameters.DisplayLanguage)}");
            }
        }

        public void Validate(IUpdateProfileParameters parameters)
        {
            _accountSettingsClientRequiredParametersValidator.Validate(parameters);

            ThrowIfParameterSizeIsInvalid(parameters.Name, $"{nameof(parameters)}.{nameof(parameters.Name)}", Limits.ACCOUNT_SETTINGS_PROFILE_NAME_MAX_LENGTH);
            ThrowIfParameterSizeIsInvalid(parameters.Description, $"{nameof(parameters)}.{nameof(parameters.Description)}", Limits.ACCOUNT_SETTINGS_PROFILE_DESCRIPTION_MAX_LENGTH);
            ThrowIfParameterSizeIsInvalid(parameters.Location, $"{nameof(parameters)}.{nameof(parameters.Location)}", Limits.ACCOUNT_SETTINGS_PROFILE_LOCATION_MAX_LENGTH);
            ThrowIfParameterSizeIsInvalid(parameters.WebsiteUrl, $"{nameof(parameters)}.{nameof(parameters.WebsiteUrl)}", Limits.ACCOUNT_SETTINGS_PROFILE_WEBSITE_URL_MAX_LENGTH);
        }

        public void Validate(IUpdateProfileImageParameters parameters)
        {
            _accountSettingsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IUpdateProfileBannerParameters parameters)
        {
            _accountSettingsClientRequiredParametersValidator.Validate(parameters);
        }

        public void Validate(IRemoveProfileBannerParameters parameters)
        {
            _accountSettingsClientRequiredParametersValidator.Validate(parameters);
        }
        
        // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
        private static void ThrowIfParameterSizeIsInvalid(string value, string parameterName, int maxSize)
        {
            if (value != null && value.Length > maxSize)
            {
                throw new ArgumentException($"{parameterName} cannot contain more than {maxSize} characters.", parameterName);
            }
        }
    }
}