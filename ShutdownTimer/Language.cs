using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShutdownTimer
{
    public static class Language
    {
        // Title
        public static readonly string Title = "Shutdown Timer";

        //
        public static readonly string ShutdownModeText = "Shutdown mode:",
                                      TimeModeText = "Time mode:";

        // Timer
        public static readonly string TimerFinishedMessage = "bye :)";

        // Start/Stop button
        public static readonly string TimerButtonStart = "Start",
                                      TimerButtonStop = "Stop";

        // Error messages
        public static readonly string ErrorMessageNonNumericalSymbol = "Timer only accepts numbers and colons",
                                      ErrorMessageNoNumberAfterSeperator = "There must be a number after each colon",
                                      ErrorMessageTooManySeperators = "There must not be more than two colons",
                                      ErrorMessageTooManyNumbersAfterSeperator = "Too many numbers in a row",
                                      ErrorMessageTimeOverMax = "Time span is too large";
    }
}
