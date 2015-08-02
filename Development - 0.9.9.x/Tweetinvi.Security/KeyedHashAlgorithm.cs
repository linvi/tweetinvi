using System;

namespace Tweetinvi.Security
{
    public abstract class KeyedHashAlgorithm : HashAlgorithm
    {
        /// <summary>
        /// The key to use in the hash algorithm.
        /// </summary>
        protected byte[] _keyValue;

        public virtual byte[] Key
        {
            get
            {
                return (byte[])_keyValue.Clone();
            }
            set
            {
                if (_state == 0)
                {
                    _keyValue = (byte[])value.Clone();                    
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_keyValue != null)
                {
                    Array.Clear(_keyValue, 0, _keyValue.Length);
                }

                _keyValue = null;
            }
            base.Dispose(disposing);
        }

        public new static KeyedHashAlgorithm Create()
        {
            return Create("System.Security.Cryptography.KeyedHashAlgorithm");
        }

        public new static KeyedHashAlgorithm Create(string algName)
        {
            return Activator.CreateInstance<KeyedHashAlgorithm>();
        }
    }
}
