using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Newtonsoft.Json.Linq;
using Tweetinvi.Core.Web;
using Tweetinvi.Models.DTO.QueryDTO;

namespace Testinvi.SetupHelpers
{
    [ExcludeFromCodeCoverage]
    public static class TwitterAccessorHelper
    {
        public static void ArrangeExecuteGETQuery(
            this ITwitterAccessor twitterAccessor,
            string query,
            JObject result)
        {
            A.CallTo(() => twitterAccessor.ExecuteGETQuery(query)).Returns(result);
        }

        public static void ArrangeExecuteGETQuery<T>(
            this ITwitterAccessor twitterAccessor, 
            string query, 
            T result) where T : class
        {
            A.CallTo(() => twitterAccessor.ExecuteGETQuery<T>(query, null)).Returns(result);
        }

        public static void ArrangeExecutePOSTQuery<T>(
            this ITwitterAccessor twitterAccessor,
            string query,
            T result) where T : class
        {
            A.CallTo(() => twitterAccessor.ExecutePOSTQuery<T>(query, null)).Returns(result);
        }

        public static void ArrangeExecutePOSTQuery(
            this ITwitterAccessor twitterAccessor,
            string query,
            JObject result)
        {
            A.CallTo(() => twitterAccessor.ExecutePOSTQuery(query)).Returns(result);
        }

        public static void ArrangeExecutePOSTMultipartQuery<T>(
            this ITwitterAccessor twitterAccessor,
            string query,
            T result) where T : class
        {
            A.CallTo(() =>
                twitterAccessor.ExecuteMultipartQuery<T>(
                    A<IMultipartHttpRequestParameters>.That.Matches(y => y.Query == query), null)).Returns(result);
        }

        public static void ArrangeTryExecutePOSTQuery(
           this ITwitterAccessor twitterAccessor,
           string query,
           bool result)
        {
            A.CallTo(() => twitterAccessor.TryExecutePOSTQuery(query, null))
                .ReturnsLazily(() => result);
        }

        public static void ArrangeTryExecuteDELETEQuery(
            this ITwitterAccessor twitterAccessor,
            string query,
            bool result)
        {
            A.CallTo(() => twitterAccessor.TryExecuteDELETEQuery(query, null))
                .ReturnsLazily(() => result);
        }

        public static void ArrangeExecuteCursorGETQuery<T, T1>(
            this ITwitterAccessor twitterAccessor,
            string query,
            IEnumerable<T> result) where T1 : class, IBaseCursorQueryDTO<T>
        {
            A.CallTo(() => twitterAccessor.ExecuteCursorGETQuery<T, T1>(query, A<int>.Ignored, -1))
                .Returns(result);
        }

        // Json
        public static void ArrangeExecuteJsonGETQuery(
            this ITwitterAccessor twitterAccessor,
            string query,
            string jsonResult)
        {
            A.CallTo(() => twitterAccessor.ExecuteGETQueryReturningJson(query))
                .Returns(jsonResult);
        }

        public static void ArrangeExecuteJsonPOSTQuery(
            this ITwitterAccessor twitterAccessor,
            string query,
            string jsonResult)
        {
            A.CallTo(() => twitterAccessor.ExecutePOSTQueryReturningJson(query))
                .Returns(jsonResult);
        }

        public static void ArrangeExecuteJsonCursorGETQuery<T>(
            this ITwitterAccessor twitterAccessor,
            string query,
            IEnumerable<string> jsonResult) where T : class, IBaseCursorQueryDTO
        {
            A.CallTo(() => twitterAccessor.ExecuteJsonCursorGETQuery<T>(query, A<int>.Ignored, A<long>.Ignored))
                .Returns(jsonResult);
        }

        // POST JSON body & get JSON response
        public static void ArrangeExecutePostQueryJsonBody<T>(this ITwitterAccessor twitterAccessor,
            string query, object reqBody, T result) where T : class
        {
            A.CallTo(() => twitterAccessor.ExecutePOSTQueryJsonBody<T>(query, reqBody, null)).Returns(result);
        }
    }
}