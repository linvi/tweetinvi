using Tweetinvi.Core.QueryGenerators;
using Tweetinvi.Core.QueryValidators;
using Tweetinvi.Factories.Properties;
using Tweetinvi.Models;

namespace Tweetinvi.Factories.Lists
{
    public interface ITwitterListFactoryQueryGenerator
    {
        string GetListByIdQuery(ITwitterListIdentifier twitterListIdentifier);
    }

    public class TwitterListFactoryQueryGenerator : ITwitterListFactoryQueryGenerator
    {
        private readonly ITwitterListQueryValidator _listsQueryValidator;
        private readonly ITwitterListQueryParameterGenerator _listQueryParameterGenerator;

        public TwitterListFactoryQueryGenerator(
            ITwitterListQueryValidator listsQueryValidator,
            ITwitterListQueryParameterGenerator listQueryParameterGenerator)
        {
            _listsQueryValidator = listsQueryValidator;
            _listQueryParameterGenerator = listQueryParameterGenerator;
        }

        public string GetListByIdQuery(ITwitterListIdentifier twitterListIdentifier)
        {
            _listsQueryValidator.ThrowIfListIdentifierIsNotValid(twitterListIdentifier);

            var identifierParameter = _listQueryParameterGenerator.GenerateIdentifierParameter(twitterListIdentifier);
            return string.Format(Resources.List_GetExistingList, identifierParameter);
        }
    }
}