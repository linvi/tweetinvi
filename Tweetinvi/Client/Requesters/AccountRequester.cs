using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Controllers;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Iterators;
using Tweetinvi.Core.Web;
using Tweetinvi.Credentials.QueryJsonConverters;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client.Requesters
{
    public class AccountRequester : BaseRequester
    {
        private readonly IUserController _userController;
        private readonly ITwitterClientFactories _factories;
        private readonly ITwitterResultFactory _twitterResultFactory;
        private readonly IUsersClientParametersValidator _validator;

        public AccountRequester(
            ITwitterClient client,
            ITwitterClientEvents clientEvents,
            IUserController userController,
            ITwitterClientFactories factories,
            ITwitterResultFactory twitterResultFactory,
            IUsersClientParametersValidator validator)
            : base(client, clientEvents)
        {
            _userController = userController;
            _factories = factories;
            _twitterResultFactory = twitterResultFactory;
            _validator = validator;
        }




    }
}