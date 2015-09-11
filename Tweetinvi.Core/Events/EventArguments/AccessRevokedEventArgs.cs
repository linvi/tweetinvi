using System;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Core.Interfaces.DTO.StreamMessages;

namespace Tweetinvi.Core.Events.EventArguments
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