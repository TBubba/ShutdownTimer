using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShutdownTimer.TimerEvents
{
    public class TimeModeEventArgs : EventArgs
    {
        // Arguments
        public TimeMode Mode;

        // Constructor(s)
        internal TimeModeEventArgs()
        {
        }
        internal TimeModeEventArgs(TimeMode mode)
        {
            Mode = mode;
        }
    }
}
