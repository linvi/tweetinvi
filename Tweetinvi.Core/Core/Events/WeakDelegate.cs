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
            _method = realDelegate.GetMethodInfo();
        }

        public TDelegate GetDelegate()
        {
            return (TDelegate)(object)GetDelegateInternal();
        }

        private Delegate GetDelegateInternal()
        {
            if (_targetReference != null)
            {
                return _method.CreateDelegate(typeof(TDelegate), _targetReference.Target);
            }
            else
            {
                return _method.CreateDelegate(typeof(TDelegate));
            }
        }

        public bool IsAlive => _targetReference == null || _targetReference.IsAlive;

        #region IEquatable<TDelegate> Members

        public bool Equals(TDelegate other)
        {
            Delegate d = (Delegate)(object)other;
            return d != null
                && d.Target == _targetReference.Target
                && d.GetMethodInfo().Equals(_method);
        }

        #endregion

        internal void Invoke(params object[] args)
        {
            var handler = GetDelegateInternal();
            handler.DynamicInvoke(args);
        }
    }
}