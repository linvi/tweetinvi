using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Web;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Core.Iterators
{
    public interface IMultiLevelCursorIteratorFactory
    {
        IMultiLevelCursorIterator<TInput, TOutput> Create<TDTO, TInput, TOutput>(
            ITwitterPageIterator<ITwitterResult<TDTO>> pageIterator,
            Func<TDTO, TInput[]> transform,
            Func<TInput[], Task<TOutput[]>> getChildItems,
            int maxPageSize);

        IMultiLevelCursorIterator<long, IUser> CreateUserMultiLevelIterator(
            ITwitterClient client,
            ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> iterator,
            int maxPageSize);
    }

    public class MultiLevelCursorIteratorFactory : IMultiLevelCursorIteratorFactory
    {
        public IMultiLevelCursorIterator<TInput, TOutput> Create<TDTO, TInput, TOutput>(
            ITwitterPageIterator<ITwitterResult<TDTO>> pageIterator,
            Func<TDTO, TInput[]> transform,
            Func<TInput[], Task<TOutput[]>> getChildItems,
            int maxPageSize)
        {
            var iterator = new MultiLevelCursorIterator<TInput, TOutput>(
                async () =>
                {
                    var userIdsPage = await pageIterator.NextPageAsync().ConfigureAwait(false);

                    return new CursorPageResult<TInput, string>
                    {
                        Items = transform(userIdsPage.Content.DataTransferObject),
                        NextCursor = userIdsPage.NextCursor,
                        IsLastPage = userIdsPage.IsLastPage
                    };
                }, async inputs =>
                {
                    var userIdsToAnalyze = inputs.Take(maxPageSize).ToArray();
                    var friends = await getChildItems(userIdsToAnalyze).ConfigureAwait(false);

                    return new MultiLevelPageProcessingResult<TInput, TOutput>
                    {
                        Items = friends,
                        AssociatedParentItems = userIdsToAnalyze,
                    };
                });

            return iterator;
        }

        public IMultiLevelCursorIterator<long, IUser> CreateUserMultiLevelIterator(
            ITwitterClient client,
            ITwitterPageIterator<ITwitterResult<IIdsCursorQueryResultDTO>> iterator,
            int maxPageSize)
        {
            return Create(iterator, dtoIds => dtoIds.Ids, client.Users.GetUsersAsync, maxPageSize);
        }
    }
}