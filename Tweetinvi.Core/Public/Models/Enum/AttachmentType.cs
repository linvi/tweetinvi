using System;
using System.Collections.Generic;
using System.Text;
using Tweetinvi.Core.Attributes;

namespace Tweetinvi.Models
{
    public enum AttachmentType
    {
        /// <summary>
        /// Default value used when the string from Twitter is not a value in the Enum
        /// </summary>
        UnrecognisedValue = 0,

        [JsonEnumString("media")]
        Media
    }
}
