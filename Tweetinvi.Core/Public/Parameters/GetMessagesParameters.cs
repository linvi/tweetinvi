using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events
    /// </summary>
    public interface IGetMessagesParameters : ICustomRequestParameters
    {
        int Count { get; set; }
        string Cursor { get; set; }
    }

    /// <summary>
    /// https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events
    /// </summary>
    public class GetMessagesParameters : CustomRequestParameters, IGetMessagesParameters
    {
        public int Count { get; set; } = TweetinviConsts.MESSAGE_GET_COUNT;
        public string Cursor { get; set; }
    }
}
