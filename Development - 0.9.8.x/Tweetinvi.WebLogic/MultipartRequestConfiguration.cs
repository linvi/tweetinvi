using System;
using System.Text;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class MultipartRequestConfiguration : IMultipartRequestConfiguration
    {
        public MultipartRequestConfiguration()
        {
            Boundary = DateTime.Now.Ticks.ToString("x");
            StartBoundary = string.Format("--{0}\r\n", Boundary);
            EndBoundary = string.Format("\r\n--{0}--\r\n", Boundary);
            EncodingAlgorithm = Encoding.GetEncoding("iso-8859-1");
        }

        public string Boundary { get; set; }
        public string StartBoundary { get; set; }
        public string EndBoundary { get; set; }
        public Encoding EncodingAlgorithm { get; set; }
    }
}