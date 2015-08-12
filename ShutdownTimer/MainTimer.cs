using System;
using System.Diagnostics;
using System.Windows.Forms;
using ShutdownTimer.TimerEvents;

namespace ShutdownTimer
{
    public enum ShutdownMode
    {
        Shutdown,
        Restart,
        Hibernate,
        Sleep,
        Logoff,
        Lock
    }

    public enum TimeMode
    {
        CountDown,
        WaitUntil
    }

    /// <summary>
    /// Handles 
    /// </summary>
    internal class MainTimer
    {
        //
        private Timer _timer; // For using the tick event
        private Stopwatch _watch; // For checking how much time has passed

        private bool _timerOn; // If the timer is on
        private TimeSpan _time; // The time the user entered before starting the timer
        private TimeSpan _timeStart; // The span of time from the beginning of the countdown (or any other timemode) to the end

        private ShutdownMode _shutdownMode; // Current shutdown mode
        private TimeMode _timeMode; // Current time mode

        //
        internal bool IsOn
        {
            get { return _timerOn; }
        }

        internal TimeSpan EnteredTime
        {
            get { return _time; }
        }
        internal TimeSpan StartTime
        {
            get { return _timeStart; }
        }

        internal ShutdownMode CurrentShutdownMode
        { get { return _shutdownMode; } }
        internal TimeMode CurrentTimeMode
        { get { return _timeMode; } }

        // Events
        internal event EventHandler<TimerTickEventArgs> OnTimerTick; // Whenever the timer ticks
        internal event EventHandler<TimerCountdownCompleteEventArgs> OnCountdownComplete; // Whenever the countdown is complete

        internal event EventHandler<TimerStartedEventArgs> OnTimerStarted; // Whenever the timer starts
        internal event EventHandler<TimerStoppedEventArgs> OnTimerStopped; // Whenever the timer stops

        internal event EventHandler<ShutdownModeEventArgs> OnShutdownModeChanged; // Whenever the shutdownmode is changed
        internal event EventHandler<TimeModeEventArgs> OnTimeModeChanged; // Whenever the timemode is changed

        // Constructor(s)
        internal MainTimer()
        {
            // Set up timers
            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Tick += new EventHandler(_timer_Tick);

            _watch = new Stopwatch();
        }

        //
        internal void ApplyCommandLineArguments()
        {
            // Apply command line arguments
            if (CLArgs.PShutdownMode.HasValue) // Shutdown Mode
                SetShutdownMode(CLArgs.PShutdownMode.Value);
            if (CLArgs.PTimeMode.HasValue) // Time Mode
                SetTimeMode(CLArgs.PTimeMode.Value);
        }

        internal void SetShutdownMode(ShutdownMode mode)
        {
            if (_shutdownMode == mode)
                return;

            _shutdownMode = mode;

            // Call event
            CallEvent<ShutdownModeEventArgs>(OnShutdownModeChanged,
                this, new ShutdownModeEventArgs(_shutdownMode));
        }
        internal void SetTimeMode(TimeMode mode)
        {
            if (_timeMode == mode)
                return;

            _timeMode = mode;

            // Call event
            CallEvent<TimeModeEventArgs>(OnTimeModeChanged,
                this, new TimeModeEventArgs(_timeMode));
        }

        internal void StartTimer(TimeSpan time)
        {
            _time = time;

            StartTimer();
        }
        internal void StartTimer()
        {
            // Start timer
            _timer.Start(); // Start timer
            _watch.Restart(); // Start watch
            _timerOn = true; // Flag timer as on

            switch (_timeMode)
            {
                case TimeMode.CountDown:
                    _timeStart = _time;
                    break;

                case TimeMode.WaitUntil:
                    TimeSpan timeLeft = _time - DateTime.Now.TimeOfDay;
                    if (timeLeft < TimeSpan.Zero)
                        timeLeft = new TimeSpan(24, 0, 0) + timeLeft;
                    _timeStart = timeLeft;
                    break;
            }

            // Call event
            CallEvent<TimerStartedEventArgs>(OnTimerStarted,
                this, new TimerStartedEventArgs());
        }

        internal void StopTimer()
        {
            // Timer
            _timer.Stop(); // Stop timer
            _watch.Stop(); // Stop watch
            _timerOn = false; // Flag timer as off

            // Call event
            CallEvent<TimerStoppedEventArgs>(OnTimerStopped,
                this, new TimerStoppedEventArgs());
        }

        private void TimerEnd()
        {
            // Stop timer
            StopTimer();

            // Call event
            CallEvent<TimerCountdownCompleteEventArgs>(OnCountdownComplete, 
                this, new TimerCountdownCompleteEventArgs());

            // Select "shutdown mode"
            switch (_shutdownMode)
            {
                case ShutdownMode.Hibernate:
                    Shut.Hibernate();
                    break;

                case ShutdownMode.Lock:
                    Shut.Lock();
                    break;

                case ShutdownMode.Logoff:
                    Shut.Logoff();
                    break;

                case ShutdownMode.Restart:
                    Shut.Restart();
                    break;

                case ShutdownMode.Shutdown:
                    Shut.Shutdown();
                    break;

                case ShutdownMode.Sleep:
                    Shut.Sleep();
                    break;
            }
        }

        private bool CallEvent<T>(EventHandler<T> handle, object sender, T e)
            where T : EventArgs
        {
            // If the handle is null
            if (handle == null)
                return false; // Return false - failed

            // Call handle
            handle(sender, e);
            return true; // Success
        }

        // Events
        private void _timer_Tick(object sender, EventArgs e)
        {
            // Counts down
            TimeSpan elapsed = _watch.Elapsed;
            TimeSpan timeLeft = _timeStart - elapsed;

            //
            CallEvent<TimerTickEventArgs>(OnTimerTick,
                this, new TimerTickEventArgs(_timeStart, elapsed, timeLeft));

            // Check if time is up
            if (timeLeft < TimeSpan.Zero)
            {

                TimerEnd();
            }
        }
    }
}
