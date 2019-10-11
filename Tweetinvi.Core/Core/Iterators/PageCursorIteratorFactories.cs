using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Iterators
{
    public interface IPageCursorIteratorFactories
    {
        ITwitterPageIterator<ITwitterResult<T[]>, long?> Create<T>(IMinMaxQueryParameters parameters, Func<long?, Task<ITwitterResult<T[]>>> getNext) where T : ITwitterIdentifier;
    }
    
    public class PageCursorIteratorFactories : IPageCursorIteratorFactories
    {
        public ITwitterPageIterator<ITwitterResult<T[]>, long?> Create<T>(IMinMaxQueryParameters parameters, Func<long?, Task<ITwitterResult<T[]>>> getNext) where T : ITwitterIdentifier
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<T[]>, long?>(
                parameters.MaxId,
                getNext,
                page =>
                {
                    return page.DataTransferObject.Min(x => x.Id) - 1;
                },
                page => page.DataTransferObject.Length < parameters.PageSize);

            return twitterCursorResult;
        }
    }
}