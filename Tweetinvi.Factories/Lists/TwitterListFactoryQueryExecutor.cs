using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO;

namespace Tweetinvi.Factories.Lists
{
    public interface ITwitterListFactoryQueryExecutor
    {
        Task<ITwitterListDTO> GetExistingList(ITwitterListIdentifier identifier);
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

        // Get existing list
        public Task<ITwitterListDTO> GetExistingList(ITwitterListIdentifier identifier)
        {
            string query = _twitterListFactoryQueryGenerator.GetListByIdQuery(identifier);
            return _twitterAccessor.ExecuteGETQuery<ITwitterListDTO>(query);
        }
    }
}