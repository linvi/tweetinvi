using System.Collections.Generic;

namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface ICursorQueryHelper
    {
        IEnumerable<T> GetResultsFromCursorQuery<T>(IEnumerable<IBaseCursorQueryDTO<T>> cursorQueryResult, int maxNumberOfResults);
    }
}