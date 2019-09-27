using Tweetinvi.Models;

namespace Tweetinvi.Parameters
{
    /// <summary>
    /// https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam
    /// </summary>
    /// <inheritdoc />
    public interface IReportUserForSpamParameters : ICustomRequestParameters
    {
        /// <summary>
        /// The user you want to block
        /// </summary>
        IUserIdentifier UserIdentifier { get; set; }
        
        /// <summary>
        /// Whether you want to block the user in addition to report him
        /// </summary>
        bool? PerformBlock { get; set; }
    }

    /// <inheritdoc />
    public class ReportUserForSpamParameters : CustomRequestParameters, IReportUserForSpamParameters
    {
        public ReportUserForSpamParameters(IUserIdentifier userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }

        public ReportUserForSpamParameters(string username) : this(new UserIdentifier(username))
        {
        }

        public ReportUserForSpamParameters(long? userId) : this(new UserIdentifier(userId))
        {
        }

        public ReportUserForSpamParameters(IReportUserForSpamParameters source) : base(source)
        {
            UserIdentifier = source?.UserIdentifier;
            PerformBlock = source?.PerformBlock;
        }

        /// <inheritdoc />
        public IUserIdentifier UserIdentifier { get; set; }
        /// <inheritdoc />
        public bool? PerformBlock { get; set; }
    }
}
