using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;
using Tweetinvi.Parameters;

namespace Tweetinvi.Core.Iterators
{
    public interface IPageCursorIteratorFactories
    {
        ITwitterPageIterator<ITwitterResult<T[]>, long?> Create<T>(IMinMaxQueryParameters parameters, Func<long?, Task<ITwitterResult<T[]>>> getNext) where T : ITwitterIdentifier;
        ITwitterPageIterator<ITwitterResult<T>> Create<T>(ICursorQueryParameters parameters, Func<string, Task<ITwitterResult<T>>> getNext) where T : IBaseCursorQueryDTO;
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
                    if (page.DataTransferObject.Length == 0)
                    {
                        return null;
                    }

                    return page.DataTransferObject?.Min(x => x.Id) - 1;
                },
                page => page.DataTransferObject.Length < parameters.PageSize);

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<T>> Create<T>(ICursorQueryParameters parameters, Func<string, Task<ITwitterResult<T>>> getNext) where T : IBaseCursorQueryDTO
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<T>>(
                parameters.Cursor,
                getNext,
                page => page.DataTransferObject.NextCursorStr,
                page => page.DataTransferObject.NextCursorStr == "0");

            return twitterCursorResult;
        }
    }
}