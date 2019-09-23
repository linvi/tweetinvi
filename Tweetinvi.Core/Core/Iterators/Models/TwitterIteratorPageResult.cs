namespace Tweetinvi.Core.Iterators
{
    public interface ITwitterIteratorPageResult<out TPageContent, out TCursor>
    {
        TPageContent Content { get; }
        TCursor NextCursor { get; }
        bool IsLastPage { get; }
    }
    
    public class TwitterIteratorPageResult<TPageContent, TCursor> : ITwitterIteratorPageResult<TPageContent, TCursor>
    {
        public TwitterIteratorPageResult(
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