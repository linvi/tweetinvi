using System;
using Tweetinvi.Models;

namespace Tweetinvi.Events
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