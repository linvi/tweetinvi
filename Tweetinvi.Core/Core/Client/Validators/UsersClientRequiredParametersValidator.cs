using System;
using System.Text;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IUsersClientRequiredParametersValidator : IUsersClientParametersValidator
    {
    }

    public class UsersClientRequiredParametersValidator : IUsersClientRequiredParametersValidator
    {
        private readonly IUserQueryValidator _userQueryValidator;

        public UsersClientRequiredParametersValidator(IUserQueryValidator userQueryValidator)
        {
            _userQueryValidator = userQueryValidator;
        }

        public void Validate(IGetUserParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User);
        }

        public void Validate(IGetUsersParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.Users == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}.{nameof(parameters.Users)}");
            }
        }

        public void Validate(IGetFollowerIdsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"{nameof(parameters)}.{nameof(parameters.User)}");
        }

        public void Validate(IGetFollowersParameters parameters)
        {
            Validate(parameters as IGetFollowerIdsParameters);
        }

        public void Validate(IGetFriendIdsParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.User, $"{nameof(parameters)}.{nameof(parameters.User)}");
        }

        public void Validate(IGetFriendsParameters parameters)
        {
            Validate(parameters as IGetFriendIdsParameters);
        }

        public void Validate(IGetRelationshipBetweenParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }
            
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.SourceUser, $"{nameof(parameters)}.{nameof(parameters.SourceUser)}");
            _userQueryValidator.ThrowIfUserCannotBeIdentified(parameters.TargetUser, $"{nameof(parameters)}.{nameof(parameters.TargetUser)}");
        }

        public void Validate(IGetProfileImageParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.ImageUrl == null)
            {
                throw new ArgumentNullException($"{nameof(parameters)}.{nameof(parameters.ImageUrl)}");
            }

            if (!Uri.IsWellFormedUriString(parameters.ImageUrl, UriKind.Absolute))
            {
                throw new ArgumentException("ImageUrl has to be valid absolute url", $"{nameof(parameters)}.{nameof(parameters.ImageUrl)}");
            }
        }
    }
}