using System;

namespace Tweetinvi.Events
{
    /// <summary>
    /// Twitter account event
    /// </summary>
    public class AccountActivityEvent
    {
        public AccountActivityEvent()
        {
            EventDate = new DateTime();
        }

        public long AccountUserId { get; set; }
        public DateTimeOffset EventDate { get; set; }
        public string Json { get; set; }
    }


    public class AccountActivityEvent<T> : AccountActivityEvent
    {
        public T Args { get; set; }

        public AccountActivityEvent(T args)
        {
            Args = args;
        }
    }
}
