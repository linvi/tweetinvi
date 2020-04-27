using System.Collections;
using System.Collections.Generic;

namespace Tweetinvi.Models
{
    public enum CustomHeaderWill
    {
        OverrideGeneratedHeaders,
        AddToGeneratedHeaders,
        RemoveGeneratedHeaders
    }

    public class CustomHeader
    {
        public CustomHeader(string key)
        {
            Key = key;
            Values = new List<string>();
            Behaviour = CustomHeaderWill.OverrideGeneratedHeaders;
        }

        public string Key { get; }
        public List<string> Values { get; }
        public CustomHeaderWill Behaviour { get; set; }
    }

    public class CustomRequestHeaders : IEnumerable<CustomHeader>
    {
        private Dictionary<string, CustomHeader> _customHeaders;

        public CustomRequestHeaders()
        {
            _customHeaders = new Dictionary<string, CustomHeader>();
        }

        public void Add(string key, string value)
        {
            Add(key, value, CustomHeaderWill.OverrideGeneratedHeaders);
        }

        public void Add(string key, List<string> values)
        {
            Add(key, values, CustomHeaderWill.OverrideGeneratedHeaders);
        }

        public void Add(string key, string value, CustomHeaderWill behaviour)
        {
            Add(key, new [] { value }, behaviour);
        }

        public void Add(string key, IEnumerable<string> values, CustomHeaderWill behaviour)
        {
            if (!_customHeaders.TryGetValue(key, out var currentValue) || currentValue == null)
            {
                currentValue = new CustomHeader(key);
            }

            currentValue.Behaviour = behaviour;
            currentValue.Values.AddRange(values);
            _customHeaders[key] = currentValue;
        }

        public void Add(CustomHeader customHeader)
        {
            if (customHeader.Values == null)
            {
                _customHeaders.Remove(customHeader.Key);
            }
            else
            {
                _customHeaders[customHeader.Key] = customHeader;
            }
        }

        public CustomHeader Get(string key)
        {
            return _customHeaders[key];
        }

        public void Remove(string key)
        {
            _customHeaders.Remove(key);
        }

        public CustomHeader this[string key] => Get(key);

        public IEnumerator<CustomHeader> GetEnumerator()
        {
            return _customHeaders.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}