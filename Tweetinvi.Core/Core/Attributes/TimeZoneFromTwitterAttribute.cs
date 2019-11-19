using System;

namespace Tweetinvi.Core.Attributes
{
    // ReSharper disable UnusedMember.Global
    public class TimeZoneFromTwitterAttribute : Attribute
    {
        public string TZinfo { get; }
        public string DisplayValue { get; }

        public TimeZoneFromTwitterAttribute(string tzinfo, string displayValue)
        {
            TZinfo = tzinfo;
            DisplayValue = displayValue;
        }
    }
}