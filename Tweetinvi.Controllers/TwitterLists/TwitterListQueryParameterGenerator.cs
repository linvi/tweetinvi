using System.Globalization;
using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Injectinvi;
using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.TwitterLists
{
    public class TwitterListQueryParameterGenerator : ITwitterListQueryParameterGenerator
    {
        private readonly IUserQueryParameterGenerator _userQueryParameterGenerator;

        private readonly IFactory<IGetTweetsFromListParameters> _getTweetsFromListParametersFactory;

        public TwitterListQueryParameterGenerator(
            IUserQueryParameterGenerator userQueryParameterGenerator,
            IFactory<IGetTweetsFromListParameters> getTweetsFromListParametersFactory)
        {
            _userQueryParameterGenerator = userQueryParameterGenerator;
            _getTweetsFromListParametersFactory = getTweetsFromListParametersFactory;
        }

        public string GenerateIdentifierParameter(ITwitterListIdentifier twitterListIdentifier)
        {
            if (twitterListIdentifier.Id > 0)
            {
                return $"list_id={twitterListIdentifier.Id}";
            }

            string ownerIdentifier;
            if (twitterListIdentifier.OwnerId > 0)
            {
                ownerIdentifier = string.Format(Resources.List_OwnerIdParameter, twitterListIdentifier.OwnerId.ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                ownerIdentifier = string.Format(Resources.List_OwnerScreenNameParameter, twitterListIdentifier.OwnerScreenName);
            }

            var slugParameter = string.Format(Resources.List_SlugParameter, twitterListIdentifier.Slug);

            return $"{slugParameter}{ownerIdentifier}";
        }

        public void AppendListIdentifierParameter(StringBuilder query, ITwitterListIdentifier listIdentifier)
        {
            var owner = new UserIdentifier(listIdentifier.OwnerScreenName);
            if (listIdentifier.OwnerId > 0)
            {
                owner.Id = listIdentifier.OwnerId;
            }

            if (listIdentifier.Id > 0)
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

        public void AppendListIdentifierParameter(StringBuilder query, IListParameters parameters)
        {
            AppendListIdentifierParameter(query, parameters.List);
        }

        // Tweets From List
        public IGetTweetsFromListParameters CreateTweetsFromListParameters()
        {
            return _getTweetsFromListParametersFactory.Create();
        }
    }
}