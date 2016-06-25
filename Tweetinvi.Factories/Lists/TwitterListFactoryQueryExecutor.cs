using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Lists
{
    public interface ITwitterListFactoryQueryExecutor
    {
        ITwitterListDTO CreateList(string name, PrivacyMode privacyMode, string description);
        ITwitterListDTO GetExistingList(ITwitterListIdentifier identifier);
    }

    public class TwitterListFactoryQueryExecutor : ITwitterListFactoryQueryExecutor
    {
        private readonly ITwitterListFactoryQueryGenerator _twitterListFactoryQueryGenerator;
        private readonly ITwitterAccessor _twitterAccessor;

        public TwitterListFactoryQueryExecutor(
            ITwitterListFactoryQueryGenerator twitterListFactoryQueryGenerator,
            ITwitterAccessor twitterAccessor)
        {
            _twitterListFactoryQueryGenerator = twitterListFactoryQueryGenerator;
            _twitterAccessor = twitterAccessor;
        }

        public ITwitterListDTO CreateList(string name, PrivacyMode privacyMode, string description)
        {
            var query = _twitterListFactoryQueryGenerator.GetCreateListQuery(name, privacyMode, description);
            return _twitterAccessor.ExecutePOSTQuery<ITwitterListDTO>(query);
        }

        // Get existing list
        public ITwitterListDTO GetExistingList(ITwitterListIdentifier identifier)
        {
            string query = _twitterListFactoryQueryGenerator.GetListByIdQuery(identifier);
            return _twitterAccessor.ExecuteGETQuery<ITwitterListDTO>(query);
        }
    }
}