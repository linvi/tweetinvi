using System;

namespace Tweetinvi.Exceptions
{
    /// <summary>
    /// This exception is raised when you provide an invalid argument because of its size or content.
    /// </summary>
    public class TwitterArgumentLimitException : ArgumentException
    {
        public TwitterArgumentLimitException(string argument, int limit, string limitType) : this(argument, limit, limitType, "items")
        {
        }

        public TwitterArgumentLimitException(string argument, int limit, string limitType, string limitValueType)
        {
            Message = $"Argument {argument} was over the limit of {limit} {limitValueType}";
            ParamName = argument;
            LimitType = limitType;
        }

        public TwitterArgumentLimitException(string argument, string message, string limitType) : base(message, argument)
        {
            LimitType = limitType;
        }

        public override string Message { get; }
        public override string ParamName { get; }
        public string LimitType { get; }
        public string Note => $"Limits can be changed in the client.Config.Limits.{LimitType}";
    }
}
