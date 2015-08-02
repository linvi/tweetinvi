using System.Collections.Generic;
using System.Net;

namespace Tweetinvi.Core.Events
{
    /// <summary>
    /// Delegate used to handle WebExceptions
    /// </summary>
    /// <param name="ex">WebException to handle</param>
    public delegate void WebExceptionHandlingDelegate(WebException ex);
 
    /// <summary>
    /// Delegate used to handle information retrieved and passed through a Dictionary[string, object]
    /// </summary>
    /// <param name="responseObject">Information retrieved</param>
    public delegate void ObjectResponseDelegate(Dictionary<string, object> responseObject); 

    /// <summary>
    /// Delegate used to handle queries that require the use of cursors
    /// </summary>
    /// <param name="responseObject">Response given by the Twitter API</param>
    /// <param name="previousCursor">Cursor used during the previous query</param>
    /// <param name="nextCursor">Cursor to be used in the next query</param>
    /// /// <returns>Nb of objects processed</returns>
    public delegate int DynamicResponseDelegate(Dictionary<string, object> responseObject, long previousCursor, long nextCursor);
}