using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi.Core.Extensions;

namespace Tweetinvi.Core.Helpers
{
    public interface IPagedOperationsHelper
    {
        Task<TResult[]> IterateOverWithLimit<TInput, TResult>(TInput[] input, Func<TInput[], Task<TResult[]>> transform, int maxItemsPerRequest);
    }

    public class PagedOperationsHelper : IPagedOperationsHelper
    {
        public async Task<TResult[]> IterateOverWithLimit<TInput, TResult>(TInput[] input, Func<TInput[], Task<TResult[]>> transform, int maxItemsPerRequest)
        {
            var result = new List<TResult>();

            for (var i = 0; i < input.Length; i += maxItemsPerRequest)
            {
                var pageItemsInput = input.Skip(i).Take(maxItemsPerRequest).ToArray();
                var pageResults = await transform(pageItemsInput);
                result.AddRangeSafely(pageResults);

                if (pageResults == null)
                {
                    throw new Exception("An error occurred in the middle of the iterations");
                }
            }

            return result.ToArray();
        }
    }
}
