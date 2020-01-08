using System;
using Tweetinvi.Events;

namespace Tweetinvi.Core.Events
{
    /// <summary>
    /// Extension methods to ease the use of events
    /// </summary>
    public static class EventHelpers
    {
        public static void Raise(this object o, EventHandler handler)
        {
            handler?.Invoke(o, EventArgs.Empty);
        }

        public static void Raise<T>(this object o, EventHandler<T> handler, T arg)
        {
            handler?.Invoke(o, arg);
        }

        public static void Raise<T>(this object o, object sender, EventHandler<T> handler, T arg)
        {
            handler?.Invoke(sender, arg);
        }

        public static void Raise<T>(this object o, EventHandler<GenericEventArgs<T>> handler, T arg)
        {
            handler?.Invoke(o, new GenericEventArgs<T>(arg));
        }
    }
}