using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using System.Drawing;

namespace ShutdownTimer
{
    public partial class MainForm : Form
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

        private Timer _timer; // For using the tick event
        private Stopwatch _watch; // For checking ow much time has passed

        private bool _timerOn; // If the timer is on
        private TimeSpan _time; // The time the user entered before starting the timer
        private TimeSpan _timeStart; // The span of time from the beginning of the countdown (or any other timemode) to the end

        private ShutdownMode _shutdownMode; // Current shutdown mode
        private TimeMode _timeMode; // Current time mode

        public MainForm()
        {
            InitializeComponent();

            // Get language
            Text = Language.Title; // Get title
            _bStart.Text = Language.TimerButtonStart; // Get button text
            _lShutdownMode.Text = Language.ShutdownModeText; // Get shutdown mode text
            _lTimeMode.Text = Language.TimeModeText; // Get time mode text

            // Set up timers
            _timer = new Timer();
            _timer.Interval = 10;
            _timer.Tick += new EventHandler(_timer_Tick);

            _watch = new Stopwatch();

            // Align text
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
            _lShutdownMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            _lTimeMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // Set up shutdown mode list
            int length = Enum.GetNames(typeof(ShutdownMode)).Length;
            for (int i = 0; i < length; i++)
                _cbShutdownMode.Items.Add(FormatEnumName(((ShutdownMode)i).ToString()));
            _cbShutdownMode.SelectedIndex = 0;

            // Set up time mode list
            length = Enum.GetNames(typeof(TimeMode)).Length;
            for (int i = 0; i < length; i++)
                _cbTimeMode.Items.Add(FormatEnumName(((TimeMode)i).ToString()));
            _cbTimeMode.SelectedIndex = 0;
        }

        private string FormatEnumName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return "";
            StringBuilder newText = new StringBuilder(name.Length * 2);
            newText.Append(name[0]);
            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && name[i - 1] != ' ')
                    newText.Append(' ');
                newText.Append(name[i]);
            }
            return newText.ToString();
        }

        private void StartTimer()
        {
            TimeSpan time;
            Time.FormatTimeError error = Time.StringToTimeSpan(_rtbTime.Text, out time);
            switch (Time.StringToTimeSpan(_rtbTime.Text, out time))
            {
                case Time.FormatTimeError.None:
                    // Timer
                    _time = time; // Set time
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

                    // Lock componentes
                    _rtbTime.ReadOnly = true;
                    _cbTimeMode.Enabled = false;

                    // Change button text
                    _bStart.Text = Language.TimerButtonStop;

                    // Clear message box (from error messages)
                    ClearMessage();
                    break;

                case Time.FormatTimeError.NonNumericalSymbol:
                    SetErrorMessage(Language.ErrorMessageNonNumericalSymbol);
                    break;

                case Time.FormatTimeError.NoNumberAfterSeperator:
                    SetErrorMessage(Language.ErrorMessageNoNumberAfterSeperator);
                    break;

                case Time.FormatTimeError.TooManySeperators:
                    SetErrorMessage(Language.ErrorMessageTooManySeperators);
                    break;

                case Time.FormatTimeError.TooManyNumbersAfterSeperator:
                    SetErrorMessage(Language.ErrorMessageTooManyNumbersAfterSeperator);
                    break;

                case Time.FormatTimeError.TimeOverMax:
                    SetErrorMessage(Language.ErrorMessageTimeOverMax);
                    break;

                default:
#if DEBUG
                    throw new Exception("Forgot to add the \"FormatTimeError\" exception in the switch in \"_bStart_Click\".");
#endif
                    break;
            }
        }

        private void StopTimer()
        {
            // Timer
            _timer.Stop(); // Stop timer
            _watch.Stop(); // Stop watch
            _timerOn = false; // Flag timer as off

            if (_timeMode == TimeMode.WaitUntil) // Check if the time mode was wait until
            {
                // Reset timer (to the typed time)
                _rtbTime.Text = Time.TimeSpanToString(_time);
                _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
            }

            // Unlock componentes
            _rtbTime.ReadOnly = false;
            _cbTimeMode.Enabled = true;

            //
            _bStart.Text = Language.TimerButtonStart;
        }

        private void TimerEnd()
        {
            // Stop timer
            StopTimer();

            //
            _rtbTime.Text = Language.TimerFinishedMessage;
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;

            // Select "shutdown mode"
            switch (_shutdownMode)
            {
                case ShutdownMode.Hibernate:
#if !DEBUG
                    Shut.Hibernate();
#endif
                    break;

                case ShutdownMode.Lock:
#if !DEBUG
                    Shut.Lock();
#endif
                    break;

                case ShutdownMode.Logoff:
#if !DEBUG
                    Shut.Logoff();
#endif
                    break;

                case ShutdownMode.Restart:
#if !DEBUG
                    Shut.Restart();
#endif
                    break;

                case ShutdownMode.Shutdown:
#if !DEBUG
                    Shut.Shutdown();
#endif
                    break;

                case ShutdownMode.Sleep:
#if !DEBUG
                    Shut.Sleep();
#endif
                    break;
            }
        }

        private void RemoveSelection(Object obj)
        {
            RichTextBox textbox = obj as RichTextBox;
            if (textbox != null)
            {
                textbox.SelectionLength = 0;
            }
        }

        private void SetErrorMessage(string message)
        {
            _lInfoMessage.Text = message; // Set text
            _lInfoMessage.BackColor = Color.Red; // Set color

            _lInfoMessage.TextAlign = ContentAlignment.MiddleCenter; // Align text to center
        }

        private void ClearMessage()
        {
            _lInfoMessage.Text = ""; // Remove text
            _lInfoMessage.BackColor = SystemColors.Control; // Reset color
        }

        // Events
        private void _timer_Tick(object sender, EventArgs e)
        {
            // Counts down
            TimeSpan timeLeft = _timeStart - _watch.Elapsed;

            // Update (visual) timer
            _rtbTime.Text = Time.TimeSpanToString(timeLeft);
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;

            // Check if time is up
            if (timeLeft < TimeSpan.Zero)
                TimerEnd();
        }

        private void _bStart_Click(object sender, EventArgs e)
        {
            // Toggle the timer (start/stop)
            if (_timerOn) // If the timer is on
                StopTimer();
            else
                StartTimer();
        }

        private void _cbTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _timeMode = (TimeMode)_cbTimeMode.SelectedIndex;
        }

        private void _cbShutdownMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _shutdownMode = (ShutdownMode)_cbShutdownMode.SelectedIndex;
        }
    }
}
