using System.Globalization;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.Parameters;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListQueryParameterGenerator : ITwitterListQueryParameterGenerator
    {
        private readonly IUserQueryValidator _userQueryValidator;
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;

        private readonly IFactory<ITwitterListUpdateParameters> _updateTwitterListParametersFactory;
        private readonly IFactory<ITwitterListUpdateQueryParameters> _updateTwitterListQueryParametersFactory;
        private readonly IFactory<IGetTweetsFromListParameters> _getTweetsFromListParametersFactory;
        private readonly IFactory<IGetTweetsFromListQueryParameters> _tweetsFromListQueryParametersFactory;

        public TwitterListQueryParameterGenerator(
            IUserQueryValidator userQueryValidator,
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IFactory<ITwitterListUpdateParameters> updateTwitterListParametersFactory,
            IFactory<ITwitterListUpdateQueryParameters> updateTwitterListQueryParametersFactory,
            IFactory<IGetTweetsFromListParameters> getTweetsFromListParametersFactory,
            IFactory<IGetTweetsFromListQueryParameters> tweetsFromListQueryParametersFactory)
        {
            _userQueryValidator = userQueryValidator;
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _updateTwitterListParametersFactory = updateTwitterListParametersFactory;
            _updateTwitterListQueryParametersFactory = updateTwitterListQueryParametersFactory;
            _getTweetsFromListParametersFactory = getTweetsFromListParametersFactory;
            _tweetsFromListQueryParametersFactory = tweetsFromListQueryParametersFactory;
        }

        public string GenerateIdentifierParameter(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier.Id != null)
            {
                return $"list_id={twitterListIdentifier.Id}";
            }

            string ownerIdentifier;
            if (_userQueryValidator.IsUserIdValid(twitterListIdentifier.OwnerId))
            {
                ownerIdentifier = string.Format(Resources.List_OwnerIdParameter, twitterListIdentifier.OwnerId?.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                ownerIdentifier = string.Format(Resources.List_OwnerScreenNameParameter, twitterListIdentifier.OwnerScreenName);
            }

            var slugParameter = string.Format(Resources.List_SlugParameter, twitterListIdentifier.Slug);

            return string.Format("{0}{1}", slugParameter, ownerIdentifier);
        }

        public void AppendListIdentifierParameter(StringBuilder query, ITwitterListIdentifier listIdentifier)
        {
            var owner = new UserIdentifier(listIdentifier.OwnerId)
            {
                ScreenName = listIdentifier.OwnerScreenName
            };

            if (listIdentifier.Id != null)
            {
                query.AddParameterToQuery("list_id", listIdentifier.Id);
            }
            else
            {
                query.AddParameterToQuery("slug", listIdentifier.Slug);

                var ownerParameter = _userQueryParameterGenerator.GenerateIdOrScreenNameParameter(owner, "owner_id", "owner_screen_name");
                query.AddFormattedParameterToQuery(ownerParameter);
            }
        }

        // Tweets From List
        public IGetTweetsFromListParameters CreateTweetsFromListParameters()
        {
            return _getTweetsFromListParametersFactory.Create();
        }

        public IGetTweetsFromListQueryParameters CreateTweetsFromListQueryParameters(
            ITwitterListIdentifier listIdentifier,
            IGetTweetsFromListParameters getTweetsFromListParameters)
        {
            var identifierParameter = TweetinviFactory.CreateConstructorParameter("listIdentifier", listIdentifier);
            var queryParameter = TweetinviFactory.CreateConstructorParameter("parameters", getTweetsFromListParameters);

            return _tweetsFromListQueryParametersFactory.Create(identifierParameter, queryParameter);
        }

        // List Update
        public ITwitterListUpdateParameters CreateUpdateListParameters()
        {
            return _updateTwitterListParametersFactory.Create();
        }

        public ITwitterListUpdateQueryParameters CreateTwitterListUpdateQueryParameters(
            ITwitterListIdentifier listIdentifier,
            ITwitterListUpdateParameters listUpdateParameters)
        {
            var identifierParameter = TweetinviFactory.CreateConstructorParameter("listIdentifier", listIdentifier);
            var queryParameter = TweetinviFactory.CreateConstructorParameter("parameters", listUpdateParameters);

            return _updateTwitterListQueryParametersFactory.Create(identifierParameter, queryParameter);
        }
    }
}