using System;
using Tweetinvi.Core.Interfaces;

namespace Tweetinvi.Core.Events.EventArguments
{
    public class ListEventArgs : EventArgs
    {
        public ListEventArgs(ITwitterList list)
        {
            List = list;
        }

        public ITwitterList List { get; private set; }
    }

    public class ListUserUpdatedEventArgs : ListEventArgs
    {
        public ListUserUpdatedEventArgs(ITwitterList list, IUser user)
            : base(list)
        {
            User = user;
        }

        public IUser User { get; private set; }
    }
}