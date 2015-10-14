using System.Text;

namespace Tweetinvi.Security
{
    namespace System.Security.Cryptography
    {
        public class HMACSHA1Generator
        {
            public byte[] ComputeHash(byte[] source, byte[] key)
            {
                HMACSHA1 hma = new HMACSHA1(key);
                return hma.ComputeHash(source);
            }

            public byte[] ComputeHash(string source, string key, Encoding encoding)
            {
                HMACSHA1 hma = new HMACSHA1(encoding.GetBytes(key));
                return hma.ComputeHash(encoding.GetBytes(source));
            }
        }
    }
}