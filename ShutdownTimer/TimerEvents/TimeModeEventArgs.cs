using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShutdownTimer.TimerEvents
{
    public class ShutdownModeEventArgs : EventArgs
    {
        // Arguments
        public ShutdownMode Mode;

        // Constructor(s)
        internal ShutdownModeEventArgs()
        {
        }
        internal ShutdownModeEventArgs(ShutdownMode mode)
        {
            Mode = mode;
        }
    }
}
