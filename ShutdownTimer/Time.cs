using System;

namespace ShutdownTimer
{
    internal static class Time
    {
        /// <summary>
        /// Type of error that can occur while formatting time
        /// (.None if none occured)
        /// </summary>
        public enum FormatTimeError
        {
            None,
            NoNumberAfterSeperator,
            NonNumericalSymbol,
            TooManyNumbersAfterSeperator,
            TooManySeperators,
            TimeOverMax
        }

        /// <summary>
        /// Converts a TimeSpan into a string
        /// </summary>
        /// <param name="time">The TimeSpan to convert</param>
        /// <returns>A string with the value of the converted TimeSpan</returns>
        internal static string TimeSpanToString(TimeSpan time)
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

        /// <summary>
        /// Converts a string to a TimeSpan
        /// </summary>
        /// <param name="time">The string to convert</param>
        /// <param name="timespan">The TimeSpan to get the value of the converted string</param>
        /// <returns>Any eventual error that have occured 
        /// (FormatTimeError.None if no errors occured - otherwise the first error to occur will be returned)</returns>
        internal static FormatTimeError StringToTimeSpan(string time, out TimeSpan timespan)
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

        /// <summary>
        /// Checks if a string contains digits only (0-9)
        /// </summary>
        /// <param name="str">The string to check</param>
        /// <returns>Whether or not the string contains digits only (0-9)</returns>
        internal static bool IsDigitsOnly(string str)
        {
            int length = str.Length;
            for (int i = 0; i < length; i++)
            {
                char c = str[i];
                if (c < '0' || c > '9') // If char is non-numeric (if not 0-9)
                    return false;
            }

            return true;
        }
    }
}
