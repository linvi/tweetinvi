using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.Parameters
{
    public interface ICustomRequestParameters
    {
        List<Tuple<string, string>> CustomQueryParameters { get; }
        string FormattedCustomQueryParameters { get; }

        void AddCustomQueryParameter(string name, string value);
        void ClearCustomQueryParameters();
    }

    public class CustomRequestParameters : ICustomRequestParameters
    {
        private readonly List<Tuple<string, string>> _customQueryParameters;

        public CustomRequestParameters()
        {
            _customQueryParameters = new List<Tuple<string, string>>();
        }

        public void AddCustomQueryParameter(string name, string value)
        {
            _customQueryParameters.Add(new Tuple<string, string>(name, value));
        }

        public void ClearCustomQueryParameters()
        {
            _customQueryParameters.Clear();
        }

        public List<Tuple<string, string>> CustomQueryParameters
        {
            get { return _customQueryParameters; }
        }

        public string FormattedCustomQueryParameters
        {
            get
            {
                if (_customQueryParameters.Count == 0)
                {
                    return string.Empty;
                }

                var queryParameters = new StringBuilder(string.Format("{0}={1}", _customQueryParameters[0].Item1, _customQueryParameters[0].Item2));

                for (int i = 1; i < _customQueryParameters.Count; ++i)
                {
                    queryParameters.Append(string.Format("&{0}={1}", _customQueryParameters[i].Item1, _customQueryParameters[i].Item2));
                }

                return queryParameters.ToString();
            }
        }
    }
}