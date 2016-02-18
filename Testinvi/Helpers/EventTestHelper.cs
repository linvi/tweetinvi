using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Testinvi.Helpers
{
    public interface IEventTestHelper<T> where T : class
    {
        int TotalNumberOfCalls { get; }
        
        IList<T> Arguments { get; }
        IDictionary<T, int> ArgumentsNumberOfCalls { get; }

        void Verify(T eventArg, int expectedTimes);
        void VerifyNumberOfCalls(int expectedTimes);
        void VerifyWhere(Func<T, bool> where, int expectedTimes);
        void VerifyAtWhere(int callNumber, Func<T, bool> where);
        void VerifyArgumentAt<R>(Func<T, R> propertyToCheck, R expectedValue, int callNumber);

        void EventAction(object sender, T args);
        void ResetMetadata();
    }

    [ExcludeFromCodeCoverage]
    public class EventTestHelper<T> : IEventTestHelper<T> where T : class
    {
        public IList<T> Arguments
        {
            get;
            private set;
        }
        public IDictionary<T, int> ArgumentsNumberOfCalls
        {
            get;
            private set;
        }
        public int TotalNumberOfCalls
        {
            get { return Arguments.Count; }
        }

        public EventTestHelper()
        {
            Arguments = new List<T>();
            ArgumentsNumberOfCalls = new Dictionary<T, int>();
        }

        public void VerifyNumberOfCalls(int expectedTimes)
        {
            if (TotalNumberOfCalls != expectedTimes)
            {
                throw new Exception(string.Format("Fail : Expected<{0}> - Value<{1}>", expectedTimes, TotalNumberOfCalls));
            }
        }

        public void Verify(T eventArg, int expectedTimes)
        {
            int nbCallsWithTheArgument;
            if (ArgumentsNumberOfCalls.TryGetValue(eventArg, out nbCallsWithTheArgument))
            {
                if (nbCallsWithTheArgument == expectedTimes)
                {
                    return;
                }
            }

            throw new Exception(string.Format("Fail : Expected<{0}> - Value<{1}>", expectedTimes, nbCallsWithTheArgument));
        }

        public void VerifyArgumentAt<R>(
            Func<T, R> property,
            R expectedValue,
            int callNumber)
        {
            if (property == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("Property cannot be null");
            }

            if (Arguments.Count <= callNumber)
            {
                throw new IndexOutOfRangeException(string.Format("There has been less than {0} calls to the event", callNumber));
            }

            T callArgument = Arguments[callNumber];
            R value = property(callArgument);

            if (!Equals(value, expectedValue))
            {
                throw new Exception(string.Format("Fail : Expected<{0}> - Value<{1}>", value, expectedValue));
            }
        }

        public void VerifyAtWhere(int callNumber, Func<T, bool> where)
        {
            if (where == null)
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("Where condition cannot be null");
            }

            if (Arguments.Count <= callNumber)
            {
                throw new IndexOutOfRangeException(string.Format("There has been less than {0} calls to the event", callNumber));
            }

            if (!where(Arguments[callNumber]))
            {
                throw new Exception(string.Format("Fail : Call [{0}] does not match the condition", callNumber));
            }
        }

        public void VerifyWhere(Func<T, bool> where, int expectedTimes)
        {
            int nbCallsMatchingTheCondition = 0;
            for (int i = 0; i < Arguments.Count; ++i)
            {
                if (where(Arguments[i]))
                {
                    ++nbCallsMatchingTheCondition;
                }
            }

            if (nbCallsMatchingTheCondition != expectedTimes)
            {
                throw new Exception(string.Format("Fail : Expected<{0}> - Value<{1}>", expectedTimes, nbCallsMatchingTheCondition));
            }
        }

        public virtual void EventAction(object sender, T args)
        {
            int numberOfCallsWithTheArgument;
            if (ArgumentsNumberOfCalls.TryGetValue(args, out numberOfCallsWithTheArgument))
            {
                ArgumentsNumberOfCalls[args] = ++numberOfCallsWithTheArgument;
            }
            else
            {
                ArgumentsNumberOfCalls.Add(args, ++numberOfCallsWithTheArgument);
            }

            Arguments.Add(args);
        }

        public void ResetMetadata()
        {
            Arguments = new List<T>();
            ArgumentsNumberOfCalls = new Dictionary<T, int>();
        }
    }
}