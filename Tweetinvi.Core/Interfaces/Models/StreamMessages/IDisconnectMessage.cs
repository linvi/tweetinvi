namespace Tweetinvi.Core.Interfaces.Models.StreamMessages
{
    public interface IDisconnectMessage
    {
        int Code { get; set; }
        string StreamName { get; set; }
        string Reason { get; set; }
    }
}