using Tweetinvi.Core.Web;

namespace Tweetinvi.Core.Iterators
{
    public interface ITwitterIteratorPageResult<out TPageContent, out TCursor> : IIteratorPageResult<TPageContent, TCursor>
    {
        string RawResult { get; }
    }

    public class TwitterIteratorPageResult<TPageContent, TCursor> : IteratorPageResult<TPageContent, TCursor>, ITwitterIteratorPageResult<TPageContent, TCursor>
        where TPageContent : ITwitterResult
    {
        public TwitterIteratorPageResult(
            TPageContent content,
            TCursor nextCursor,
            bool isLastPage) : base(content, nextCursor, isLastPage)
        {
            RawResult = content.RawResult;
        }

        public string RawResult { get; }
    }
}