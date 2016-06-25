using System;
using Tweetinvi.Models;
using Tweetinvi.Streaming.Events;

namespace Tweetinvi.Events
{
    public interface IAccessRevokedEventArgs
    {
        IAccessRevokedInfo Info { get; }
        IUser Source { get; }
        IUser Target { get; }
    }

    public class AccessRevokedEventArgs : EventArgs, IAccessRevokedEventArgs
    {
        public AccessRevokedEventArgs(
            IAccessRevokedInfo accessRevokedInfo,
            IUser source,
            IUser target)
        {
            Info = accessRevokedInfo;
            Source = source;
            Target = target;
        }

        public IAccessRevokedInfo Info { get; private set; }
        public IUser Source { get; private set; }
        public IUser Target { get; private set; }
    }
}