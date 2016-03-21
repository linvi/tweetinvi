using System;
using Tweetinvi.Core.Events.EventArguments;

namespace Tweetinvi.Core.Events
{
    /// <summary>
    /// Extension methods to ease the use of events
    /// </summary>
    public static class EventHelpers
    {
        public static void Raise(this object o, EventHandler handler)
        {
            if (handler != null)
            {
                handler(o, EventArgs.Empty);
            }
        }

        public static void Raise<T>(this object o, EventHandler<T> handler, T arg)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(o, arg);
            }
        }

        public static void Raise<T>(this object o, object sender, EventHandler<T> handler, T arg)
            where T : EventArgs
        {
            if (handler != null)
            {
                handler(sender, arg);
            }
        }

        public static void Raise<T>(this object o, EventHandler<GenericEventArgs<T>> handler, T arg)
        {
            if (handler != null)
            {
                handler(o, new GenericEventArgs<T>(arg));
            }
        }

        public static void Raise<T, U>(this object o, EventHandler<GenericEventArgs<T, U>> handler, 
            T arg1, U arg2)
        {
            if (handler != null)
            {
                handler(o, new GenericEventArgs<T, U>(arg1, arg2));
            }
        }

        public static void Raise<T, U, V>(this object o, EventHandler<GenericEventArgs<T, U, V>> handler,
           T arg1, U arg2, V arg3)
        {
            if (handler != null)
            {
                handler(o, new GenericEventArgs<T, U, V>(arg1, arg2, arg3));
            }
        }

        public static void Raise<T, U, V, W>(this object o, EventHandler<GenericEventArgs<T, U, V, W>> handler,
           T arg1, U arg2, V arg3, W arg4)
        {
            if (handler != null)
            {
                handler(o, new GenericEventArgs<T, U, V, W>(arg1, arg2, arg3, arg4));
            }
        }
    }
}