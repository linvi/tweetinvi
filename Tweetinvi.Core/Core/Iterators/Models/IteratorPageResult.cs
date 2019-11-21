namespace Tweetinvi.Core.Iterators
{
    public interface IIteratorPageResult<out TPageContent, out TCursor>
    {
        TPageContent Content { get; }
        TCursor NextCursor { get; }
        bool IsLastPage { get; }
    }
    
    public class IteratorPageResult<TPageContent, TCursor>: IIteratorPageResult<TPageContent, TCursor>
    {
        public IteratorPageResult(
            TPageContent content,
            TCursor nextCursor,
            bool isLastPage)
        {
            Content = content;
            NextCursor = nextCursor;
            IsLastPage = isLastPage;
        }
        
        public TPageContent Content { get; }
        public TCursor NextCursor { get; }
        public bool IsLastPage { get; }
    }
}