namespace Tweetinvi.Core.Models
{
    public class CRCResponseTokenInfo
    {
        public CRCResponseTokenInfo()
        {
            ContenType = "application/json; charset=utf-8";
        }

        public string ContenType { get; set; }
        public string CrcResponseToken { get; set; }
        public string Json { get; set; }
    }
}
