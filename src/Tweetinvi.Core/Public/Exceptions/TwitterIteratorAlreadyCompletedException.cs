using System;

namespace Tweetinvi.Exceptions
{
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