namespace Tweetinvi.Parameters
{
    /// <summary>
    /// For more information visit : https://developer.twitter.com/en/docs/direct-messages/sending-and-receiving/api-reference/list-events
    /// </summary>
    public interface IGetMessagesParameters : ICursorQueryParameters
    {
    }

    /// <inheritdoc/>
    public class GetMessagesParameters : CursorQueryParameters, IGetMessagesParameters
    {
        public GetMessagesParameters()
        {
            PageSize = TwitterLimits.DEFAULTS.MESSAGES_GET_MAX_PAGE_SIZE;
        }

        public GetMessagesParameters(IGetMessagesParameters parameters) : base(parameters)
        {
            if (parameters == null)
            {
                PageSize = TwitterLimits.DEFAULTS.MESSAGES_GET_MAX_PAGE_SIZE;
            }
        }

    }
}