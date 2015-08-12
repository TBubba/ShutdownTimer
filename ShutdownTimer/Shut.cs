using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShutdownTimer
{
    /// <summary>
    /// Contains all methods regarding shutting down, restarting, locking etc. the machine/system
    /// </summary>
    internal static class Shut
    {
        /// <summary>
        /// Starts a shutdown immediately (same as pressing "Shut down" in the start menu)
        /// </summary>
        internal static void Shutdown()
        {
#if !DEBUG
            Process.Start("shutdown", "/s /t 0"); // "/s" = shut down
#endif
        }
        /// <summary>
        /// Starts a restart immediately (same as pressing "Restart" in the start menu)
        /// </summary>
        internal static void Restart()
        {
#if !DEBUG
            Process.Start("shutdown", "/r /t 0"); // "/r" = restart
#endif
        }
        /// <summary>
        /// Goes into hibernating mode immediately (same as pressing "Hibernate" in the start menu)
        /// </summary>
        internal static void Hibernate()
        {
#if !DEBUG
            SetSuspendState(true, true, true);
#endif
        }
        /// <summary>
        /// Goes into sleeping mode immediately (same as pressing "Sleep" in the start menu)
        /// </summary>
        internal static void Sleep()
        {
#if !DEBUG
            SetSuspendState(false, true, true);
#endif
        }
        /// <summary>
        /// Starts a log off immediately (same as pressing "Log off" in the start menu)
        /// </summary>
        internal static void Logoff()
        {
#if !DEBUG
            ExitWindowsEx(0, 0);
#endif
        }
        /// <summary>
        /// Starts a lock immediately (same as pressing "Lock" in the start menu)
        /// (I know it might sound weird, but it is at least more consistent)
        /// </summary>
        internal static void Lock()
        {
#if !DEBUG
            LockWorkStation();
#endif
        }

        // Imports
        [DllImport("user32")]
        private static extern bool ExitWindowsEx(uint uFlags, uint dwReason);

        [DllImport("user32")]
        private static extern void LockWorkStation();

        [DllImport("PowrProf.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern bool SetSuspendState(bool hiberate, bool forceCritical, bool disableWakeEvent);

    }
}
