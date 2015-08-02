using System;
using System.Collections.Generic;

namespace Tweetinvi.Core.Events
{
    public interface IWeakEvent<TEventHandler>
    {
        void AddHandler(TEventHandler handler);
        void RemoveHandler(TEventHandler handler);
        void Raise(object sender, EventArgs e);
    }

    // Code taken from: http://tomlev2.wordpress.com/2010/05/17/c-a-simple-implementation-of-the-weakevent-pattern/
    public class WeakEvent<TEventHandler> : IWeakEvent<TEventHandler>
    {
        private readonly List<WeakDelegate<TEventHandler>> _handlers;
        private readonly object _syncRoot = new object();

        public WeakEvent()
        {
            _handlers = new List<WeakDelegate<TEventHandler>>();
        }

        public virtual void AddHandler(TEventHandler handler)
        {
            Delegate d = (Delegate)(object)handler;
            lock (_syncRoot)
            {
                _handlers.Add(new WeakDelegate<TEventHandler>(d));
            }
        }

        public virtual void RemoveHandler(TEventHandler handler)
        {
            // Also remove "dead" (garbage collected) handlers
            WeakDelegate<TEventHandler>[] handlers;
            lock (_syncRoot)
            {
                handlers = _handlers.ToArray();
            }

            foreach (var deadHandler in handlers)
            {
                if (!deadHandler.IsAlive || deadHandler.Equals(handler))
                {
                    lock (_syncRoot)
                    {
                        _handlers.Remove(deadHandler);
                    }
                }
            }
        }

        public virtual void Raise(object sender, EventArgs e)
        {
            WeakDelegate<TEventHandler>[] handlers;
            lock (_syncRoot)
            {
                handlers = _handlers.ToArray();
            }

            foreach (var weakDelegate in handlers)
            {
                if (weakDelegate.IsAlive)
                {
                    weakDelegate.Invoke(sender, e);
                }
                else
                {
                    lock (_syncRoot)
                    {
                        _handlers.Remove(weakDelegate);
                    }
                }
            }
        }

        protected List<WeakDelegate<TEventHandler>> Handlers
        {
            get { return _handlers; }
        }
    }
}