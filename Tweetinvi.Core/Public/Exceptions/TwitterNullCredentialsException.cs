namespace Tweetinvi.Exceptions
{
    public class TwitterNullCredentialsException : TwitterInvalidCredentialsException
    {
        public TwitterNullCredentialsException() : base("You must set the credentials to use the Twitter API. (Read the exception description field for more information)")
        {
        }

        public TwitterNullCredentialsException(string message) : base(message)
        {
        }

        public string Description
        {
            get { return "Before performing any query please set the credentials : Auth.SetUserCredentials(\"CONSUMER_KEY\", \"CONSUMER_SECRET\", \"ACCESS_TOKEN\", \"ACCESS_TOKEN_SECRET\");";  }
        }
    }
}