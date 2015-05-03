using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;

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

        public enum FormatTimeError
        {
            None,
            NoNumberAfterSeperator,
            NonNumericalSymbol,
            TooManyNumbersAfterSeperator,
            TooManySeperators,
            TimeOverMax
        }

        private Timer _timer; // For using the tick event
        private Stopwatch _watch; // For checking ow much time has passed

        private bool _timerOn; // If the timer is on
        private TimeSpan _time; // The timespan related to the timer (count down, wait unil and so on)

        public MainForm()
        {
            InitializeComponent();

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

        private FormatTimeError FormatTime(string time, out TimeSpan timespan)
        {
            timespan = new TimeSpan();
            long mils = 0;

            string[] splits = time.Split(':');
            int length = splits.Length;
            if (length > 3) // Check if there are too many seperators
                return FormatTimeError.TooManySeperators;

            for (int i = length - 1; i >= 0; i--) // Loop through all splits
            {
                if (splits[i].Length == 0) // Check if there is anything after the seperator
                    return FormatTimeError.NoNumberAfterSeperator;

                if (!IsDigitsOnly(splits[i])) // Check if it is a valid number
                    return FormatTimeError.NonNumericalSymbol;

                if (splits[i].Length > 2) // Check if there are too many numbers after the seperator
                    return FormatTimeError.TooManyNumbersAfterSeperator;

                mils += (long)Math.Pow(60, (length - 1) - i) * long.Parse(splits[i]); // Add time
            }

            if (mils > (int)new TimeSpan(24, 0, 0).TotalSeconds) // Check if the timespan is too large
                return FormatTimeError.TimeOverMax;

            timespan = new TimeSpan(mils * 10000000L); // Create timespan
            return FormatTimeError.None; // Success (no error)
        }

        private void StartTimer()
        {
            TimeSpan time;
            switch (FormatTime(_rtbTime.Text, out time))
            {
                case FormatTimeError.None:
                    // Timer
                    _time = time; // Set time
                    _timer.Start(); // Start timer
                    _watch.Restart(); // Start watch
                    _timerOn = true; // Flag timer as on

                    // Lock componentes
                    _rtbTime.ReadOnly = true;
                    _cbTimeMode.Enabled = false;

                    //
                    _bStart.Text = "Stop";
                    break;

                case FormatTimeError.NonNumericalSymbol:
                    break;

                case FormatTimeError.NoNumberAfterSeperator:
                    break;

                case FormatTimeError.TooManySeperators:
                    break;

                case FormatTimeError.TooManyNumbersAfterSeperator:
                    break;

                case FormatTimeError.TimeOverMax:
                    break;

                default:
                    throw new Exception("Forgot to add the \"FormatTimeError\" exception in the switch in \"_bStart_Click\".");
            }
        }

        private void StopTimer()
        {
            // Timer
            _timer.Stop(); // Stop timer
            _watch.Stop(); // Stop watch
            _timerOn = false; // Flag timer as off

            // Unlock componentes
            _rtbTime.ReadOnly = false;
            _cbTimeMode.Enabled = true;

            //
            _bStart.Text = "Start";
        }

        private void TimerEnd()
        {
            // Stop timer
            StopTimer();

            // Select "shutdown mode"
            ShutdownMode sm = (ShutdownMode)_cbShutdownMode.SelectedIndex;
            switch (sm)
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

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        private void RemoveSelection(Object obj)
        {
            RichTextBox textbox = obj as RichTextBox;
            if (textbox != null)
            {
                textbox.SelectionLength = 0;
            }
        }

        private string FormatTimeSpan(TimeSpan time)
        {
            string text = "";

            // Hours
            if (time.Hours > 0)
            {
                if (time.Hours > 9)
                    text += time.Hours;
                else
                    text += "0" + time.Hours;
            }

            // Minutes
            if (time.Minutes > 0)
            {
                if (text != "")
                    text += ":";

                if (time.Minutes > 9)
                    text += time.Minutes;
                else
                    text += "0" + time.Minutes;
            }

            // Seconds
            if (text != "")
                text += ":";

            if (time.Seconds > 9 || text == "")
                text += time.Seconds;
            else
                text += "0" + time.Seconds;

            return text;
        }

        // Events
        private void _timer_Tick(object sender, EventArgs e)
        {
            TimeMode tm = (TimeMode)_cbTimeMode.SelectedIndex;
            switch (tm)
            {
                case TimeMode.CountDown:
                    TimeSpan timeLeft = _time - _watch.Elapsed; // Calculate time left

                    // Update (visual) timer
                    _rtbTime.Text = FormatTimeSpan(timeLeft);
                    _rtbTime.SelectionAlignment = HorizontalAlignment.Center;

                    // Check if time is up
                    if (timeLeft < TimeSpan.Zero)
                        TimerEnd();

                    break;

                case TimeMode.WaitUntil:



                    break;
            }
        }

        private void _bStart_Click(object sender, EventArgs e)
        {
            // Toggle the timer (start/stop)
            if (_timerOn) // If the timer is on
                StopTimer();
            else
                StartTimer();
        }

        private void _cbShutdownMode_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 
            if (_timerOn)
                e.Handled = true;
        }
    }
}
