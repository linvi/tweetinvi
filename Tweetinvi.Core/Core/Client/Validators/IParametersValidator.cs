using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Client.Validators
{
    public interface IParametersValidator : IUsersClientParametersValidator
    {
    }
    
    public interface IInternalParametersValidator : IParametersValidator
    {
        void Initialize(ITwitterClient client);
    }
    
    public class ParametersValidator : IInternalParametersValidator
    {
        private readonly IInternalUsersClientParametersValidator _usersClientParametersValidator;

        public ParametersValidator(IInternalUsersClientParametersValidator usersClientParametersValidator)
        {
            _usersClientParametersValidator = usersClientParametersValidator;
        }
        
        public void Initialize(ITwitterClient client)
        {
            _usersClientParametersValidator.Initialize(client);
        }

        public void Validate(IGetUserParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetUsersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFollowerIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFollowersParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFriendIdsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetFriendsParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetRelationshipBetweenParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }

        public void Validate(IGetProfileImageParameters parameters)
        {
            _usersClientParametersValidator.Validate(parameters);
        }
    }
}