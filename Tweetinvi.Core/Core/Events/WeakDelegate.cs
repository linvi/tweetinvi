using System;
using System.Reflection;

namespace Tweetinvi.Core.Events
{
    // Code taken from: http://tomlev2.wordpress.com/2010/05/17/c-a-simple-implementation-of-the-weakevent-pattern/
    public class WeakDelegate<TDelegate> : IEquatable<TDelegate>
    {
        private readonly WeakReference _targetReference;
        private readonly MethodInfo _method;

        public WeakDelegate(Delegate realDelegate)
        {
            if (realDelegate.Target != null)
                _targetReference = new WeakReference(realDelegate.Target);
            else
                _targetReference = null;
#if NET_CORE
            _method = realDelegate.GetMethodInfo();
#else
            _method = realDelegate.Method;
#endif

        }

        public TDelegate GetDelegate()
        {
            return (TDelegate)(object)GetDelegateInternal();
        }

        private Delegate GetDelegateInternal()
        {
#if NET_CORE
            if (_targetReference != null)
            {
                return _method.CreateDelegate(typeof(TDelegate), _targetReference.Target);
            }
            else
            {
                return _method.CreateDelegate(typeof(TDelegate));
            }
#else
            if (_targetReference != null)
            {
                return Delegate.CreateDelegate(typeof(TDelegate), _targetReference.Target, _method);
            }
            else
            {
                return Delegate.CreateDelegate(typeof(TDelegate), _method);
            }
#endif
        }

        public bool IsAlive
        {
            get { return _targetReference == null || _targetReference.IsAlive; }
        }

        #region IEquatable<TDelegate> Members

        public bool Equals(TDelegate other)
        {
            Delegate d = (Delegate)(object)other;
            return d != null
                && d.Target == _targetReference.Target
#if NET_CORE
                && d.GetMethodInfo().Equals(_method);
#else
                && d.Method.Equals(_method);
#endif
        }

        #endregion

        internal void Invoke(params object[] args)
        {
            Delegate handler = (Delegate)(object)GetDelegateInternal();
            handler.DynamicInvoke(args);
        }
    }
}