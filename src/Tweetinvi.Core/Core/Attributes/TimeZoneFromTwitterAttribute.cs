using System;

namespace Tweetinvi.Core.Attributes
{
    public class TimeZoneFromTwitterAttribute : Attribute
    {
        // ReSharper disable once InconsistentNaming
        public string TZinfo { get; }
        public string DisplayValue { get; }

        public TimeZoneFromTwitterAttribute(string tzinfo, string displayValue)
        {
            TZinfo = tzinfo;
            DisplayValue = displayValue;
        }
    }
}