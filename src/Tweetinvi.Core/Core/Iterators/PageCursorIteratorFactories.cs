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
                    if (page.Model.Length == 0)
                    {
                        return null;
                    }

                    return page.Model?.Min(x => x.Id) - 1;
                },
                page =>
                {
                    if (parameters.ContinueMinMaxCursor == ContinueMinMaxCursor.UntilPageSizeIsDifferentFromRequested)
                    {
                        return page.Model.Length < parameters.PageSize;
                    }

                    return page.Model.Length == 0;
                });

            return twitterCursorResult;
        }

        public ITwitterPageIterator<ITwitterResult<T>> Create<T>(ICursorQueryParameters parameters, Func<string, Task<ITwitterResult<T>>> getNext) where T : IBaseCursorQueryDTO
        {
            var twitterCursorResult = new TwitterPageIterator<ITwitterResult<T>>(
                parameters.Cursor,
                getNext,
                page => page.Model.NextCursorStr,
                page => page.Model.NextCursorStr == "0");

            return twitterCursorResult;
        }
    }
}