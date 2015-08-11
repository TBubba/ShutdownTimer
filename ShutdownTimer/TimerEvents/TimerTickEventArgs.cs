using System;

namespace ShutdownTimer.TimerEvents
{
    public class TimerTickEventArgs : EventArgs
    {
        // Arguments
        public TimeSpan TimerStart;
        public TimeSpan TimerElapsed;
        public TimeSpan TimeLeft;

        // Constructor(s)
        internal TimerTickEventArgs()
        {
        }
        internal TimerTickEventArgs(TimeSpan timerStart, TimeSpan timerElapsed, TimeSpan timeLeft)
        {
            TimerStart = timerStart;
            TimerElapsed = timerElapsed;
            TimeLeft = timeLeft;
        }
    }
}
