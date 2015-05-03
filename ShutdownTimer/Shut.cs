using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ShutdownTimer
{
    internal static class Shut
    {
        internal static void Shutdown()
        {
            Process.Start("shutdown", "/s /t 0");
        }
        internal static void Restart()
        {
            Process.Start("shutdown", "/r /t 0");
        }
        internal static void Hibernate()
        {
            SetSuspendState(true, true, true);
        }
        internal static void Sleep()
        {
            SetSuspendState(false, true, true);
        }
        internal static void Logoff()
        {
            ExitWindowsEx(0, 0);
        }
        internal static void Lock()
        {
            LockWorkStation();
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
