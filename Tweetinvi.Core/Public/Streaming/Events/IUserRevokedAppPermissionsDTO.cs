using System;
using System.Collections.Generic;
using System.Text;

namespace Tweetinvi.Core.Public.Streaming.Events
{
    public interface IActivityStreamAppIdentifierDTO
    {
        string AppId { get; set; }
    }

    public interface IActivityStreamUserIdentifierDTO
    {
        string UserId { get; set; }
    }

    public interface IUserRevokedAppPermissionsDTO
    {
        IActivityStreamAppIdentifierDTO Target { get; set; }
        IActivityStreamUserIdentifierDTO Source { get; set; }

    }
}
