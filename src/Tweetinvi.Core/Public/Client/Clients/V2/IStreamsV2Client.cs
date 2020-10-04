using System.Threading.Tasks;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters.V2;
using Tweetinvi.Streaming.V2;

namespace Tweetinvi.Client.V2
{
    public interface IStreamsV2Client
    {
        /// <summary>
        /// Create a sample stream
        /// <para> Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/sampled-stream/api-reference/get-tweets-sample-stream </para>
        /// </summary>
        ISampleStreamV2 CreateSampleStream();

        /// <summary>
        /// Create a filtered stream
        /// <para> Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/get-tweets-search-stream </para>
        /// </summary>
        IFilteredStreamV2 CreateFilteredStream();

        /// <inheritdoc cref="GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async();
        /// <inheritdoc cref="GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(params string[] ruleIds);

        /// <summary>
        /// Get the filtered stream rules configured for the app. Not specifying an id, will return them all
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/get-tweets-search-stream-rules </para>
        /// </summary>
        /// <returns>Filtered stream rules</returns>
        Task<FilteredStreamRulesV2Response> GetRulesForFilteredStreamV2Async(IGetRulesForFilteredStreamV2Parameters parameters);

        /// <inheritdoc cref="AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(params FilteredStreamRuleConfig[] rulesToAdd);

        /// <summary>
        /// Add filtered stream rules to the app.
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/post-tweets-search-stream-rules </para>
        /// </summary>
        /// <returns>Filtered stream rules created</returns>
        Task<FilteredStreamRulesV2Response> AddRulesToFilteredStreamAsync(IAddRulesToFilteredStreamV2Parameters parameters);

        /// <inheritdoc cref="DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(params string[] ruleIdsToDelete);
        /// <inheritdoc cref="DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(params FilteredStreamRuleV2[] rulesToDelete);

        /// <summary>
        /// Remove filtered stream rules to the app.
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/post-tweets-search-stream-rules </para>
        /// </summary>
        /// <returns>Deleted rules</returns>
        Task<FilteredStreamRulesV2Response> DeleteRulesFromFilteredStreamAsync(IDeleteRulesFromFilteredStreamV2Parameters parameters);

        /// <inheritdoc cref="TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters)"/>
        Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(params FilteredStreamRuleConfig[] rulesToAdd);
        /// <summary>
        /// Test the validity of creating a specific set of rules. Uses the `dry_run` parameter.
        /// <para>Read more : https://developer.twitter.com/en/docs/twitter-api/tweets/filtered-stream/api-reference/post-tweets-search-stream-rules </para>
        /// </summary>
        /// <returns>Filtered stream rules that would have been created successfully</returns>
        Task<FilteredStreamRulesV2Response> TestFilteredStreamRulesV2Async(IAddRulesToFilteredStreamV2Parameters parameters);

    }
}