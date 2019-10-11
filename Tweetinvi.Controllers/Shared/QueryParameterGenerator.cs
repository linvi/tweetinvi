using System.Text;
using Tweetinvi.Controllers.Properties;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Controllers.Shared
{
    public interface IQueryParameterGenerator
    {
        string GenerateCountParameter(int count);
        string GenerateTrimUserParameter(bool? trimUser);
        string GenerateSinceIdParameter(long? sinceId);
        string GenerateMaxIdParameter(long? maxId);
        string GenerateIncludeEntitiesParameter(bool? includeEntities);
        string GenerateSkipStatusParameter(bool skipStatus);
        string GeneratePageNumberParameter(int? pageNumber);
        string GenerateIncludeRetweetsParameter(bool includeRetweets);
        string GenerateLanguageParameter(Language? language);
        string GenerateTweetModeParameter(TweetMode? tweetMode);
        string GenerateCursorParameter(string cursor);
        string GenerateTweetIdentifier(ITweetIdentifier tweetId);

        string GenerateAdditionalRequestParameters(string additionalParameters, bool existingParameters = true);
        void AddMinMaxQueryParameters(StringBuilder query, IMinMaxQueryParameters parameters);
    }

    public class QueryParameterGenerator : IQueryParameterGenerator
    {
        public string GenerateCountParameter(int count)
        {
            if (count == -1)
            {
                return string.Empty;
            }

            return string.Format(Resources.QueryParameter_Count, count);
        }

        public string GenerateTrimUserParameter(bool? trimUser)
        {
            if (trimUser == null)
            {
                return string.Empty;
            }
            
            return string.Format(Resources.QueryParameter_TrimUser, trimUser);
        }

        public string GenerateSinceIdParameter(long? sinceId)
        {
            if (sinceId == null || sinceId == TweetinviSettings.DEFAULT_ID)
            {
                return string.Empty;
            }

            return string.Format(Resources.QueryParameter_SinceId, sinceId);
        }

        public string GenerateMaxIdParameter(long? maxId)
        {
            if (maxId == null || maxId == TweetinviSettings.DEFAULT_ID)
            {
                return string.Empty;
            }

            return string.Format(Resources.QueryParameter_MaxId, maxId);
        }

        public string GenerateIncludeEntitiesParameter(bool? includeEntities)
        {
            if (includeEntities == null)
            {
                return string.Empty;
            }
            
            return string.Format(Resources.QueryParameter_IncludeEntities, includeEntities);
        }

        public string GenerateSkipStatusParameter(bool skipStatus)
        {
            return string.Format(Resources.QueryParameter_SkipStatus, skipStatus);
        }

        public string GeneratePageNumberParameter(int? pageNumber)
        {
            if (pageNumber == null)
            {
                return string.Empty;
            }

            return string.Format(Resources.QueryParameter_PageNumber, pageNumber);
        }

        public string GenerateIncludeRetweetsParameter(bool includeRetweets)
        {
            return string.Format(Resources.QueryParameter_IncludeRetweets, includeRetweets);
        }

        public string GenerateLanguageParameter(Language? language)
        {
            var languageParameter = string.Empty;
            if (language != null && language.Value != Language.Undefined)
            {
                var languageCode = language.Value.GetLanguageCode();
                if (!string.IsNullOrEmpty(languageCode))
                {
                    languageParameter = string.Format("lang={0}", languageCode);
                }
            }

            return languageParameter;
        }

        public string GenerateTweetModeParameter(TweetMode? tweetMode)
        {
            var tweetModeParameter = string.Empty;

            if (tweetMode != null)
            {
                tweetModeParameter = $"tweet_mode={tweetMode.ToString().ToLowerInvariant()}";
            }

            return tweetModeParameter;
        }

        public string GenerateCursorParameter(string cursor)
        {
            return string.IsNullOrEmpty(cursor) ? "" : string.Format(Resources.QueryParameter_Cursor, cursor);
        }

        public string GenerateTweetIdentifier(ITweetIdentifier tweetId)
        {
            return tweetId.IdStr ?? tweetId.Id.ToString();
        }

        public string GenerateAdditionalRequestParameters(string additionalParameters, bool existingParameters = true)
        {
            if (string.IsNullOrEmpty(additionalParameters))
            {
                return string.Empty;
            }

            return $"{(existingParameters ? "&" : "?")}{additionalParameters}";
        }

        public void AddMinMaxQueryParameters(StringBuilder query, IMinMaxQueryParameters parameters)
        {
            query.AddParameterToQuery("count", parameters.PageSize);
            query.AddParameterToQuery("since_id", parameters.SinceId);
            query.AddParameterToQuery("max_id", parameters.MaxId);
        }
    }
}