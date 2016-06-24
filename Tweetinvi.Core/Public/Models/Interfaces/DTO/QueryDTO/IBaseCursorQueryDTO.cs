using System.Collections.Generic;

namespace Tweetinvi.Models.DTO.QueryDTO
{
    public interface IBaseCursorQueryDTO
    {
        long PreviousCursor { get; set; }
        long NextCursor { get; set; }

        string PreviousCursorStr { get; set; }
        string NextCursorStr { get; set; }

        string RawJson { get; set; }

        int GetNumberOfObjectRetrieved();
    }

    public interface IBaseCursorQueryDTO<T> : IBaseCursorQueryDTO
    {
        IEnumerable<T> Results { get; set; }
    }
}