using System.Collections;
using System.Collections.Generic;

namespace Tweetinvi.Core.Models
{
    public interface IPageResult<TItem> : IEnumerable<TItem>
    {
        /// <summary>
        /// Items returned during for a specific cursor iteration
        /// </summary>
        TItem[] Items { get; set; }
        
        /// <summary>
        /// Whether all the data have been returned
        /// </summary>
        bool IsLastPage { get; }
    }

    public class PageResult<TItem> : IPageResult<TItem>
    {
        public PageResult()
        {
            Items = new TItem[0];
        }
        
        public TItem[] Items { get; set; }
        public bool IsLastPage { get; set; }

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