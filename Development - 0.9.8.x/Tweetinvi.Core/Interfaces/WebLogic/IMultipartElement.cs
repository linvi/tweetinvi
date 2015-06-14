using System.Collections.Generic;

namespace Tweetinvi.Core.Interfaces.WebLogic
{
    public interface IMultipartElement
    {
        string Name { get; set; }
        string Boundary { get; set; }
        string ContentId { get; set; }
        string ContentDispositionType { get; set; }
        string ContentType { get; set; }
        string Data { get; set; }
        Dictionary<string, string> AdditionalParameters { get; set; }
    }
}