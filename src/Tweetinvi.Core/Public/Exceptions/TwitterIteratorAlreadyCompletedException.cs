using System;

namespace Tweetinvi.Exceptions
{
    /// <summary>
    /// Exception raised when attempting to move to a next page of an iterator that already completed its lifecyle.
    /// </summary>
    public class TwitterIteratorAlreadyCompletedException : Exception
    {
        public TwitterIteratorAlreadyCompletedException(string message) : base(message)
        {

        }

        public TwitterIteratorAlreadyCompletedException() : this("Iterator already completed. Create another iterator if you want to make another search")
        {
        }
    }
}