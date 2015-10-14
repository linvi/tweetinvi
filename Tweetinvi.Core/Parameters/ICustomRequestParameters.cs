using System;
using System.Collections.Generic;

namespace Tweetinvi.Core.Parameters
{
    public interface ICustomRequestParameters
    {
        List<Tuple<string, string>> CustomQueryParameters { get; }
        string FormattedCustomQueryParameters { get; }

        void AddCustomQueryParameter(string name, string value);
        void ClearCustomQueryParameters();
    }
}