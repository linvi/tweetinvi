using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Interfaces.WebLogic;

namespace Tweetinvi.WebLogic
{
    public class MultipartElement : IMultipartElement
    {
        public MultipartElement()
        {
            AdditionalParameters = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string Boundary { get; set; }
        public string ContentDispositionType { get; set; }
        public string ContentId { get; set; }
        public string ContentType { get; set; }
        public string Data { get; set; }
        public Dictionary<string, string> AdditionalParameters { get; set; }

        public override string ToString()
        {
            string boundary = String.Format("--{0}\r\n", Boundary);
            string contentDisposition = String.Format("Content-Disposition: {0}; ", ContentDispositionType);
            string contenName = String.Format("name=\"{0}\"; ", ContentId);
            string additionalParameters = String.Join("\r\n", AdditionalParameters.Select(x => String.Format("{0}={1}", x.Key, x.Value)));
            string contentType = String.Format("\r\nContent-Type: {0}\r\n\r\n", ContentType);
            return String.Format("{0}{1}{2}{3}{4}{5}", boundary, contentDisposition, contenName, additionalParameters, contentType, Data);
        }
    }
}