using System;

namespace Tweetinvi.Events
{
    public abstract class BaseAccountActivityEventArgs : EventArgs
    {
        public BaseAccountActivityEventArgs(AccountActivityEvent activityEvent)
        {
            AccountUserId = activityEvent.AccountUserId;
            EventDate = activityEvent.EventDate;
            Json = activityEvent.Json;
        }

        public long AccountUserId { get; }
        public DateTime EventDate { get; }
        public string Json { get; set; }
    }

    public abstract class BaseAccountActivityEventArgs<T> : BaseAccountActivityEventArgs
    {
        protected BaseAccountActivityEventArgs(AccountActivityEvent activityEvent) : base(activityEvent)
        {
        }
    }
}
