using Newtonsoft.Json;

namespace Tweetinvi.Core.Upload
{
    public interface IUploadInitModel
    {
        long MediaId { get; set; }
        long ExpiresAfterInSeconds { get; set; }
    }
    
    // ReSharper disable once ClassNeverInstantiated.Local
    public class UploadInitModel : IUploadInitModel
    {
        [JsonProperty("media_id")] public long MediaId { get; set; }

        [JsonProperty("expires_after_secs")] public long ExpiresAfterInSeconds { get; set; }
    }
}