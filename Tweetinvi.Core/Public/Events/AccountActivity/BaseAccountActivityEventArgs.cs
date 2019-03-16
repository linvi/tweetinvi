using System;

namespace Tweetinvi.Events
{
    public abstract class BaseAccountActivityEventArgs<T> : EventArgs
    {
        public long AccountUserId { get; }
        public DateTime EventDate { get; }
        public string Json { get; set; }

        public BaseAccountActivityEventArgs(AccountActivityEvent activityEvent)
        {
            AccountUserId = activityEvent.AccountUserId;
            EventDate = activityEvent.EventDate;
            Json = activityEvent.Json;
        }
    }
}
