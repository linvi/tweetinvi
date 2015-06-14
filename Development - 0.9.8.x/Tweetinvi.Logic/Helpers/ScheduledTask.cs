using System;
using System.Collections.Generic;
using System.Timers;
using TweetinviLogic.Helpers.Visitors;

namespace TweetinviLogic.Helpers
{
    /// <summary>
    /// Task that can be executed in the future (only once).
    /// When executed, the task processes an object using a visitor.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScheduledTask<T> : IDisposable
    {
        #region Private attributes
        // timer that will cause the execution of the task
        private readonly Timer _timer;

        // Creation date of this task
        // private DateTime _time;

        // Object to be processed
        private readonly T _object;

        // Object processor
        private readonly IVisitor<T> _visitor;

        // List of scheduled tasks that will be executed in the future
        private static List<IDisposable> _scheduledTasks = new List<IDisposable>();

        #endregion

        /// <summary>
        /// Task constructor that creates a timer that will run out after the time specified in parameter.
        /// When this timer runs out, it will cause the execution of the task.
        /// </summary>
        /// <param name="timeframe">Time (in milliseconds) after which the task is going to be executed</param>
        /// <param name="t">Object to process when the task is going to be executed</param>
        /// <param name="visitor">Visitor that will process the object when the task is going to be executed</param>
        public ScheduledTask(double timeframe, T t, IVisitor<T> visitor)
        {
            _visitor = visitor;
            _object = t;
            // _time = new DateTime();
            _timer = new Timer(timeframe);
            _timer.Elapsed += executeTask;
            _timer.Start();
            // Add the task in the list of tasks to be executed in the future
            _scheduledTasks.Add(this);
        }

        /// <summary>
        /// Method to be executed when the timer runs out.
        /// Dispose of the attributes and process the object with the visitor.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void executeTask(object sender, ElapsedEventArgs args)
        {
            Dispose();
            if (_visitor != null)
            {
                _visitor.Visit(_object);
            }
        }

        /// <summary>
        /// Stop the timer and dispose of it. 
        /// Remove this task from the list of tasks to be executed in the future.
        /// </summary>
        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
            _scheduledTasks.Remove(this);
        }
    }
}