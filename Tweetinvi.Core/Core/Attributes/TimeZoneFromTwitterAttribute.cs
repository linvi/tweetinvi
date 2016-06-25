using System;

namespace Tweetinvi.Core.Attributes
{
    // ReSharper disable UnusedMember.Global
    public class TimeZoneFromTwitterAttribute : Attribute
    {
        public string TZinfo { get; private set; }
        public string DisplayValue { get; private set; }

        public TimeZoneFromTwitterAttribute(string tzinfo, string displayValue)
        {
            TZinfo = tzinfo;
            DisplayValue = displayValue;
        }
    }
}