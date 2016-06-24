using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Tweetinvi.Credentials.QueryDTO
{
    public class CursorQueryHelper : ICursorQueryHelper
    {
        public IEnumerable<T> GetResultsFromCursorQuery<T>(IEnumerable<IBaseCursorQueryDTO<T>> cursorQueryResult, int maxNumberOfResults)
        {
            if (cursorQueryResult == null)
            {
                return null;
            }

            var allResults = cursorQueryResult.SelectMany(x => x.Results).ToArray();
            return allResults.Take(Math.Min(allResults.Length, maxNumberOfResults)).ToArray();
        }
    }
}