using System.Collections.Generic;
using System.IO;
using Tweetinvi.Core.Web;

namespace Tweetinvi.WebLogic
{
    public class WebRequestResult : IWebRequestResult
    {
        public string URL { get; set; }
        public Stream ResultStream { get; set; }
        public int StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public Dictionary<string, IEnumerable<string>> Headers { get; set; }
        public byte[] Binary { get; set; }
        public string Text { get; set; }
    }
}