﻿using System;
using System.Collections.Generic;
using ShutdownTimer.Options;

namespace ShutdownTimer
{
    /// <summary>
    /// Handles everything related to command line arguments
    /// </summary>
    internal static class CLArgs
    {
        // Parameters
        internal static TimeSpan? PTime
        { get; private set; }
        internal static ShutdownMode? PShutdownMode
        { get; private set; }
        internal static TimeMode? PTimeMode
        { get; private set; }
        
        /// <summary>
        /// Gets, parses and keeps data from the Command Line Arguments
        /// </summary>
        internal static void ParseArgs()
        {
            // Keeps the parameters (temporarily)
            TimeSpan? time = null;
            ShutdownMode? sdm = null;
            TimeMode? tm = null;

            // Set up argument parser
            var p = new OptionSet() {
                { "sm|shutdownmode=",
                    "name of shutdown mode.\n"+
                    "s=shutdown, r=restart, sl=sleep,\n" +
                    "l=lock, lo=logoff, h=hibernate",
                    (string v) => {
                        if (v == "s" || v == "shutdown")
                            sdm = ShutdownMode.Shutdown;
                        else if (v == "r" || v == "restart")
                            sdm = ShutdownMode.Restart;
                        else if (v == "sl" || v == "sleep")
                            sdm = ShutdownMode.Sleep;
                        else if (v == "l" || v == "lock")
                            sdm = ShutdownMode.Lock;
                        else if (v == "lo" || v == "logoff")
                            sdm = ShutdownMode.Logoff;
                        else if (v == "h" || v == "hibernate")
                            sdm = ShutdownMode.Hibernate;
                        else
                            {/* INVALID SHUTDOWN MODE */}
                    }},
                { "t|time=", 
                    "name of time mode.\n"+
                    "format: hh:mm:ss | mm:ss | ss",
                    (string v) => {
                        TimeSpan t;
                        if (Time.StringToTimeSpan(v, out t) == Time.FormatTimeError.None)
                            time = t;
                    }},
                { "tm|timemode=",
                    "name of time mode.\n"+
                    "c=countdown, w=waituntil",
                    (string v) => {
                        if (v == "c" || v == "countdown")
                            tm = TimeMode.CountDown;
                        else if (v == "w" || v == "waituntil")
                            tm = TimeMode.WaitUntil;
                        else
                        { /* INVALID TIME MODE */}
                    }}
            };

            List<string> extra;
            try
            {
                // Parse command line arguments
                extra = p.Parse(Environment.GetCommandLineArgs());

                // Move temporary parameters to the permanent
                PTime = time;
                PShutdownMode = sdm;
                PTimeMode = tm;
            }
            catch (OptionException e)
            {
                throw new Exception();
            }
        }
    }
}
