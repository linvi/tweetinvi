using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FakeItEasy;
using Newtonsoft.Json.Linq;
using Testinvi.Helpers;
using Tweetinvi.Core.Interfaces.Credentials;
using Tweetinvi.Core.Interfaces.DTO.QueryDTO;
using Tweetinvi.Core.Web;

namespace Testinvi.SetupHelpers
{
    [ExcludeFromCodeCoverage]
    public static class TwitterAccessorHelper
    {
        public static void ArrangeExecuteGETQuery(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            JObject result)
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteGETQuery(query))
                .Returns(result);
        }

        public static void ArrangeExecuteGETQuery<T>(
            this Fake<ITwitterAccessor> fakeTwitterAccessor, 
            string query, 
            T result) where T : class
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteGETQuery<T>(query, null))
                .Returns(result);
        }

        public static void ArrangeExecutePOSTQuery<T>(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            T result) where T : class
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecutePOSTQuery<T>(query, null))
                .Returns(result);
        }

        public static void ArrangeExecutePOSTQuery(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            JObject result)
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecutePOSTQuery(query))
                .Returns(result);
        }

        public static void ArrangeExecutePOSTMultipartQuery<T>(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            T result) where T : class
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteMultipartQuery<T>(A<IMultipartHttpRequestParameters>.That.Matches(y => y.Query == query), null))
                .Returns(result);
        }

        public static void ArrangeTryExecutePOSTQuery(
           this Fake<ITwitterAccessor> fakeTwitterAccessor,
           string query,
           bool result)
        {
            fakeTwitterAccessor
                .CallsTo(x => x.TryExecutePOSTQuery(query, null))
                .ReturnsLazily(() =>
                {
                    return result;
                });
        }

        public static void ArrangeExecuteCursorGETQuery<T, T1>(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            IEnumerable<T> result) where T1 : class, IBaseCursorQueryDTO<T>
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteCursorGETQuery<T, T1>(query, A<int>.Ignored, -1))
                .Returns(result);
        }

        // Json
        public static void ArrangeExecuteJsonGETQuery(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            string jsonResult)
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteJsonGETQuery(query))
                .Returns(jsonResult);
        }

        public static void ArrangeExecuteJsonPOSTQuery(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            string jsonResult)
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteJsonPOSTQuery(query))
                .Returns(jsonResult);
        }

        public static void ArrangeExecuteJsonCursorGETQuery<T>(
            this Fake<ITwitterAccessor> fakeTwitterAccessor,
            string query,
            IEnumerable<string> jsonResult) where T : class, IBaseCursorQueryDTO
        {
            fakeTwitterAccessor
                .CallsTo(x => x.ExecuteJsonCursorGETQuery<T>(query, A<int>.Ignored, A<long>.Ignored))
                .Returns(jsonResult);
        }

        
    }
}