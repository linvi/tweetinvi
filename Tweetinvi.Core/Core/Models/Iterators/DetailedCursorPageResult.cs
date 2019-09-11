using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.DTO.Cursor;
using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Models
{
    public interface IDetailedCursorPageResult<TItem, TDTO> : IPageResult<TItem>
    {
        ITwitterResult<TDTO> TwitterResult { get; set; }
        TItem[] Items { get; set; }
        string NextCursor { get; set; }
        string PreviousCursor { get; set; }
    }

    public class DetailedCursorPageResult<TItem, TDTO> : IDetailedCursorPageResult<TItem, TDTO>
    {
        public DetailedCursorPageResult(ITwitterResult<TDTO> result, TItem[] items, string nextCursor, string previousCursor)
        {
            TwitterResult = result;
            Items = items;
            NextCursor = nextCursor;
            PreviousCursor = previousCursor;
            IsLastPage = nextCursor == "0";
        }
        
        public DetailedCursorPageResult(ITwitterResult<TDTO> result, BaseCursorQueryDTO<TItem> cursorResult)
        {
            TwitterResult = result;
            Items = cursorResult.Results.ToArray();
            NextCursor = cursorResult.NextCursorStr;
            PreviousCursor = cursorResult.NextCursorStr;
            IsLastPage = NextCursor == "0";
        }

        public ITwitterResult<TDTO> TwitterResult { get; set; }
        public TItem[] Items { get; set; }
        public string NextCursor { get; set; }
        public string PreviousCursor { get; set; }
        public bool IsLastPage { get; }

        
        public IEnumerator<TItem> GetEnumerator()
        {
            var items = Items ?? new TItem[0];
            return ((IEnumerable<TItem>) items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
