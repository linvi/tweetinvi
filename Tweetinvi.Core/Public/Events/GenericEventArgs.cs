using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// EventArgs with value of Type T
    /// </summary>
    public class GenericEventArgs<T>  : EventArgs
    {
        public T Value { get; }

        public GenericEventArgs(T value)
        {
            Value = value;
        }
    }
}