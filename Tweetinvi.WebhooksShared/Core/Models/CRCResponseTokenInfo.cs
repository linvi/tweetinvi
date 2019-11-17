namespace Tweetinvi.Core.Models
{
    public class CrcResponseTokenInfo
    {
        public CrcResponseTokenInfo()
        {
            ContentType = "application/json; charset=utf-8";
        }

        public string ContentType { get; set; }
        public string CrcResponseToken { get; set; }
        public string Json { get; set; }
    }
}
