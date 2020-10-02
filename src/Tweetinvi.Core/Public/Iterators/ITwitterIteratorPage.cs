namespace Tweetinvi.Iterators
{
    /// <summary>
    /// An iterator page containing the values of a specific page.
    /// It also give access to the next page as well as informing if a next page exists.
    /// </summary>
    public interface ITwitterIteratorPage<out TCursor>
    {
        TCursor NextCursor { get; }
        bool IsLastPage { get; }
    }

    /// <summary>
    /// An iterator page containing the values of a specific page.
    /// It also give access to the next page as well as informing if a next page exists.
    /// </summary>
    public interface ITwitterIteratorPage<out TItem, out TCursor>
    {
        TCursor NextCursor { get; }
        bool IsLastPage { get; }
        TItem Response { get; }
    }
}