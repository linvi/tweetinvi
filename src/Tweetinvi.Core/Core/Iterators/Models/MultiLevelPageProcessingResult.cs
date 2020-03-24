namespace Tweetinvi.Core.Iterators
{
    public interface IPageProcessingResult<TParent, TItem>
    {
        TItem[] Items { get; set; }
        TParent[] AssociatedParentItems { get; set; }
    }
    
    public class MultiLevelPageProcessingResult<TParent, TItem> : IPageProcessingResult<TParent, TItem>
    {
        public TItem[] Items { get; set; }
        public TParent[] AssociatedParentItems { get; set; }
    }
}