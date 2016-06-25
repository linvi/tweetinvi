using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// EventArgs with value of Type T
    /// </summary>
    public class GenericEventArgs<T>  : EventArgs
    {
        public T Value { get; private set; }

        public GenericEventArgs(T value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// EventArgs with value of Type T and U
    /// </summary>
    public class GenericEventArgs<T, U> : EventArgs
    {
        public T Value { get; private set; }
        public U Value2 { get; private set; }

        public GenericEventArgs(T value, U value2)
        {
            Value = value;
            Value2 = value2;
        }
    }

    /// <summary>
    /// EventArgs with value of Type T, U and V
    /// </summary>
    public class GenericEventArgs<T, U, V> : EventArgs
    {
        public T Value { get; private set; }
        public U Value2 { get; private set; }
        public V Value3 { get; private set; }

        public GenericEventArgs(T value, U value2, V value3)
        {
            Value = value;
            Value2 = value2;
            Value3 = value3;
        }
    }

    /// <summary>
    /// EventArgs with value of Type T, U and V
    /// </summary>
    public class GenericEventArgs<T, U, V, W> : EventArgs
    {
        public T Value { get; private set; }
        public U Value2 { get; private set; }
        public V Value3 { get; private set; }
        public W Value4 { get; private set; }

        public GenericEventArgs(T value, U value2, V value3, W value4)
        {
            Value = value;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }
    }
}