using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using ShutdownTimer.TimerEvents;

namespace ShutdownTimer
{
    public partial class MainForm : Form
    {
        private MainTimer _timer; // For using the tick event

        private Language _language; // Current language

        // Constructor(s)
        public MainForm()
        {
            InitializeComponent();

            // Set up timer
            _timer = new MainTimer(); // Create the timer
            _timer.OnTimerTick += OnTimerTick; // Set up events
            _timer.OnCountdownComplete += OnCountdownComplete;
            _timer.OnTimerStarted += OnTimerStarted;
            _timer.OnTimerStopped += OnTimerStopped;
            _timer.OnShutdownModeChanged += OnShutdownModeChanged;
            _timer.OnTimeModeChanged += OnTimeModeChanged;

            // Set up shutdown mode list
            int length = Enum.GetNames(typeof(ShutdownMode)).Length;
            for (int i = 0; i < length; i++)
                _cbShutdownMode.Items.Add("");
            _cbShutdownMode.SelectedIndex = 0;

            // Set up time mode list
            length = Enum.GetNames(typeof(TimeMode)).Length;
            for (int i = 0; i < length; i++)
                _cbTimeMode.Items.Add("");
            _cbTimeMode.SelectedIndex = 0;

            // Load language
            _language = new Language();
            LoadLanguage(new Language());

            // Apply command line arguments
            if (CLArgs.PTime.HasValue) // Time
                _rtbTime.Text = Time.TimeSpanToString(CLArgs.PTime.Value);

            // Apply command line arguments to timer
            _timer.ApplyCommandLineArguments();

            // Align text
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
            _lShutdownMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            _lTimeMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        }

        //
        private void LoadLanguage(Language lang)
        {
            // Copy inputted language
            _language.CopyFrom(lang);

            // Refresh everything that uses language
            RefreshLanguage();
        }
        private void RefreshLanguage()
        {
            Text = _language.Title; // Get title
            _bStart.Text = _language.TimerButtonStart; // Get button text
            _lShutdownMode.Text = _language.ShutdownModeText; // Get shutdown mode text
            _lTimeMode.Text = _language.TimeModeText; // Get time mode text

            // Set all shutdown modes' names
            _cbShutdownMode.Items[0] = _language.ShutdownModeShutdown;
            _cbShutdownMode.Items[1] = _language.ShutdownModeRestart;
            _cbShutdownMode.Items[2] = _language.ShutdownModeHibernate;
            _cbShutdownMode.Items[3] = _language.ShutdownModeSleep;
            _cbShutdownMode.Items[4] = _language.ShutdownModeLogoff;
            _cbShutdownMode.Items[5] = _language.ShutdownModeLock;

            // Set all time modes' names
            _cbTimeMode.Items[0] = _language.TimeModeCountdown;
            _cbTimeMode.Items[1] = _language.TimeModeWaitUntil;
        }

        private void RemoveSelection(object obj)
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

        //
        private void StartTimer()
        {
            // Format inputted time
            TimeSpan time;
            Time.FormatTimeError error = Time.StringToTimeSpan(_rtbTime.Text, out time);

            // 
            switch (Time.StringToTimeSpan(_rtbTime.Text, out time))
            {
                case Time.FormatTimeError.None:
                    // Start timer
                    _timer.StartTimer(time);
                    break;

                case Time.FormatTimeError.NonNumericalSymbol:
                    SetErrorMessage(_language.ErrorMessageNonNumericalSymbol);
                    break;

                case Time.FormatTimeError.NoNumberAfterSeperator:
                    SetErrorMessage(_language.ErrorMessageNoNumberAfterSeperator);
                    break;

                case Time.FormatTimeError.TooManySeperators:
                    SetErrorMessage(_language.ErrorMessageTooManySeperators);
                    break;

                case Time.FormatTimeError.TooManyNumbersAfterSeperator:
                    SetErrorMessage(_language.ErrorMessageTooManyNumbersAfterSeperator);
                    break;

                case Time.FormatTimeError.TimeOverMax:
                    SetErrorMessage(_language.ErrorMessageTimeOverMax);
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
            //
            _timer.StopTimer();
        }

        // Timer Events
        private void OnTimerTick(object sender, TimerTickEventArgs e)
        {
            // Update (visual) timer
            _rtbTime.Text = Time.TimeSpanToString(e.TimeLeft);
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void OnCountdownComplete(object sender, TimerCountdownCompleteEventArgs e)
        {
            //
            _rtbTime.Text = _language.TimerFinishedMessage;
            _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
        }
        private void OnTimerStarted(object sender, TimerStartedEventArgs e)
        {
            // Lock componentes
            _rtbTime.ReadOnly = true;
            _cbTimeMode.Enabled = false;

            // Change button text
            _bStart.Text = _language.TimerButtonStop;

            // Clear message box (from error messages)
            ClearMessage();
        }
        private void OnTimerStopped(object sender, TimerStoppedEventArgs e)
        {
            // Check if the time mode was wait until
            if (_timer.CurrentTimeMode == TimeMode.WaitUntil)
            {
                // Reset timer (to the typed time)
                _rtbTime.Text = Time.TimeSpanToString(_timer.EnteredTime);
                _rtbTime.SelectionAlignment = HorizontalAlignment.Center;
            }

            // Unlock componentes
            _rtbTime.ReadOnly = false;
            _cbTimeMode.Enabled = true;

            //
            _bStart.Text = _language.TimerButtonStart;
        }
        private void OnShutdownModeChanged(object sender, ShutdownModeEventArgs e)
        {
            _cbShutdownMode.SelectedIndex = (int)e.Mode;
        }
        private void OnTimeModeChanged(object sender, TimeModeEventArgs e)
        {
            _cbTimeMode.SelectedIndex = (int)e.Mode;
        }

        // Winforms Events
        private void _bStart_Click(object sender, EventArgs e)
        {
            // Toggle the timer (start/stop)
            if (_timer.IsOn) // If the timer is on
                StopTimer();
            else
                StartTimer();
        }

        private void _cbTimeMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _timer.SetTimeMode((TimeMode)_cbTimeMode.SelectedIndex);
        }

        private void _cbShutdownMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _timer.SetShutdownMode((ShutdownMode)_cbShutdownMode.SelectedIndex);
        }
    }
}
