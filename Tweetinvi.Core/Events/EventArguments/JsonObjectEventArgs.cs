using System;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class JsonObjectEventArgs : EventArgs
    {
        public JsonObjectEventArgs(string json)
        {
            Json = json;
        }

        public string Json { get; private set; }
    }
}