
namespace ShutdownTimer
{
    /// <summary>
    /// 
    /// </summary>
    internal class Language
    {
        // Title
        public string Title = "Shutdown Timer";

        // Mode labels
        public string ShutdownModeText = "Shutdown mode:",
                      TimeModeText = "Time mode:";

        // Timer
        public string TimerFinishedMessage = "bye :)";

        // Start/Stop button
        public string TimerButtonStart = "Start",
                      TimerButtonStop = "Stop";

        // Error messages
        public string ErrorMessageNonNumericalSymbol = "Timer only accepts numbers and colons",
                      ErrorMessageNoNumberAfterSeperator = "There must be a number after each colon",
                      ErrorMessageTooManySeperators = "There must not be more than two colons",
                      ErrorMessageTooManyNumbersAfterSeperator = "Too many numbers in a row",
                      ErrorMessageTimeOverMax = "Time span is too large";

        // Shutdown Modes
        public string ShutdownModeShutdown = "Shutdown",
                      ShutdownModeRestart = "Restart",
                      ShutdownModeHibernate = "Hibernate",
                      ShutdownModeSleep = "Sleep",
                      ShutdownModeLogoff = "Log off",
                      ShutdownModeLock = "Lock";

        // Time Modes
        public string TimeModeCountdown = "Countdown",
                      TimeModeWaitUntil = "Wait until";

        // Constructor(s)
        internal Language()
        {
        }
        internal Language(Language lang)
        {
            // Creates a copy of parameter
            CopyFrom(lang);
        }

        //
        internal void CopyFrom(Language lang)
        {
            Title = lang.Title;

            ShutdownModeText = lang.ShutdownModeText;
            TimeModeText = lang.TimeModeText;

            TimerFinishedMessage = lang.TimerFinishedMessage;

            TimerButtonStart = lang.TimerButtonStart;
            TimerButtonStop = lang.TimerButtonStop;

            ErrorMessageNonNumericalSymbol = lang.ErrorMessageNonNumericalSymbol;
            ErrorMessageNoNumberAfterSeperator = lang.ErrorMessageNoNumberAfterSeperator;
            ErrorMessageTooManySeperators = lang.ErrorMessageTooManySeperators;
            ErrorMessageTooManyNumbersAfterSeperator = lang.ErrorMessageTooManyNumbersAfterSeperator;
            ErrorMessageTimeOverMax = lang.ErrorMessageTimeOverMax;

            ShutdownModeShutdown = lang.ShutdownModeShutdown;
            ShutdownModeRestart = lang.ShutdownModeRestart;
            ShutdownModeHibernate = lang.ShutdownModeHibernate;
            ShutdownModeSleep = lang.ShutdownModeSleep;
            ShutdownModeLogoff = lang.ShutdownModeLogoff;
            ShutdownModeLock = lang.ShutdownModeLock;

            TimeModeCountdown = lang.TimeModeCountdown;
            TimeModeWaitUntil = lang.TimeModeWaitUntil;
        }
    }
}
