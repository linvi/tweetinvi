using System;

namespace Tweetinvi.Events
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