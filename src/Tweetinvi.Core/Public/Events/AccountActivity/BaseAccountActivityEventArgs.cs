using System;

namespace Tweetinvi.Events
{
    public abstract class BaseAccountActivityEventArgs
    {
        protected BaseAccountActivityEventArgs(AccountActivityEvent activityEvent)
        {
            AccountUserId = activityEvent.AccountUserId;
            EventDate = activityEvent.EventDate;
            Json = activityEvent.Json;
        }

        /// <summary>
        /// The account user id for who the event has been raised
        /// </summary>
        public long AccountUserId { get; }

        /// <summary>
        /// The date when the event has occurred
        /// </summary>
        public DateTimeOffset EventDate { get; }

        /// <summary>
        /// The full json message from which this event has been extracted out.
        /// Note that a message can contain multiple events. This message will
        /// contain them all as the information associated with the event can be
        /// stored at different level of the json.
        /// </summary>
        public string Json { get; }
    }

    public abstract class BaseAccountActivityEventArgs<T> : BaseAccountActivityEventArgs
    {
        protected BaseAccountActivityEventArgs(AccountActivityEvent activityEvent) : base(activityEvent)
        {
        }

        /// <summary>
        /// The action that resulted in this event to be raised
        /// </summary>
        public T InResultOf { get; protected set; }
    }
}
