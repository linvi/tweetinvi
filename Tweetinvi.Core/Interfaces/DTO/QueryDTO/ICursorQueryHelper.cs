using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.DTO.QueryDTO
{
    public interface ICursorQueryHelper
    {
        IEnumerable<T> GetResultsFromCursorQuery<T>(IEnumerable<IBaseCursorQueryDTO<T>> cursorQueryResult, int maxNumberOfResults);
    }
}