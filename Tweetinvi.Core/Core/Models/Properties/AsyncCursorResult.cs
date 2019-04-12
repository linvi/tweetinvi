namespace Tweetinvi.Core.Models
{
    public class AsyncCursorResult<T>
    {
        public string Cursor { get; set; }
        public T Result { get; set; }
    }
}
