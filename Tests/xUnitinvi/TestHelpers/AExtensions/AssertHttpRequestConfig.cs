using System;

namespace xUnitinvi.TestHelpers
{
    public class AssertHttpRequestConfig
    {
        public AssertHttpRequestConfig(Action<string> logAction)
        {
            Log = logAction;
        }

        public Action<string> Log { get; set; }
    }
}