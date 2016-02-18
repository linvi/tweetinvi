using System;
using Tweetinvi.Core.Enum;
using Tweetinvi.Core.Interfaces.Models;
using Tweetinvi.Core.Interfaces.QueryGenerators;
using Tweetinvi.Core.Interfaces.QueryValidators;
using Tweetinvi.Factories.Properties;

namespace Tweetinvi.Factories.Lists
{
    public interface ITwitterListFactoryQueryGenerator
    {
        string GetCreateListQuery(string name, PrivacyMode privacyMode, string description);
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

        public string GetCreateListQuery(string name, PrivacyMode privacyMode, string description)
        {
            var baseQuery = string.Format(Resources.List_Create, name, privacyMode.ToString().ToLower());

            if (_listsQueryValidator.IsDescriptionParameterValid(description))
            {
                baseQuery += string.Format(Resources.List_Create_DescriptionParameter, description);
            }

            return baseQuery;
        }

        public string GetListByIdQuery(ITwitterListIdentifier twitterListIdentifier)
        {
            if (!_listsQueryValidator.IsListIdentifierValid(twitterListIdentifier))
            {
                return null;
            }

            var identifierParameter = _listQueryParameterGenerator.GenerateIdentifierParameter(twitterListIdentifier);
            return string.Format(Resources.List_GetExistingList, identifierParameter);
        }
    }
}