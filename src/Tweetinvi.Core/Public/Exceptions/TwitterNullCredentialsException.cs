namespace Tweetinvi.Exceptions
{
    /// <summary>
    /// Exception raised when providing null credentials
    /// </summary>
    public class TwitterNullCredentialsException : TwitterInvalidCredentialsException
    {
        public TwitterNullCredentialsException() : base("You must set the credentials to use the Twitter API. (Read the exception description field for more information)")
        {
        }

        public TwitterNullCredentialsException(string message) : base(message)
        {
        }

        public string Description => "Before performing any query please set the credentials : Auth.SetUserCredentials(\"CONSUMER_KEY\", \"CONSUMER_SECRET\", \"ACCESS_TOKEN\", \"ACCESS_TOKEN_SECRET\");";
    }
}