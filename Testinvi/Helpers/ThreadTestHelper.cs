using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Timer = System.Timers.Timer;

namespace Testinvi.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class ThreadTestHelper
    {
        /// <summary>
        /// Start a Thread that will close after a delay
        /// </summary>
        /// <param name="threadAction">Action to be performed by the Thread</param>
        /// <param name="actionOnElapsed">Action to be performed when the Timer is over</param>
        /// <param name="closureDelay">Delay before stopping the thread</param>
        /// <param name="initializationDelay">
        /// Delay required for the threadAction to be ready to be Tested
        /// </param>
        /// <param name="initialDelay">Delay before a Thread is started</param>
        public static Thread StartLifetimedThread(
            Action threadAction,
            Action actionOnElapsed,
            long closureDelay,
            int initializationDelay = 0,
            int initialDelay = 0)
        {
            Thread t = new Thread(() =>
            {
                if (closureDelay > 0)
                {
                    Timer timer = new Timer(closureDelay);
                    timer.Elapsed += (sender, args) =>
                    {
                        timer.Stop();

                        if (actionOnElapsed != null)
                        {
                            actionOnElapsed();
                        }
                    };

                    timer.Start();
                    threadAction();
                    timer.Stop();
                }
            });

            if (initialDelay != 0)
            {
                Timer initialTimer = new Timer(initialDelay);
                initialTimer.Elapsed += (sender, args) =>
                {
                    initialTimer.Stop();
                    t.Start();
                    Thread.Sleep(initializationDelay);
                };

                initialTimer.Start();
            }
            else
            {
                t.Start();
                Thread.Sleep(initializationDelay);
            }

            return t;
        }

        public static Thread StartDelayedAction(
            Action actionOnElapsed,
            int delay)
        {
            Thread t = new Thread(() =>
            {
                Timer timer = new Timer(delay);
                timer.Elapsed += (sender, args) =>
                {
                    timer.Stop();

                    if (actionOnElapsed != null)
                    {
                        actionOnElapsed();
                    }
                };

                timer.Start();
            });

            t.Start();

            return t;
        }
    }
}