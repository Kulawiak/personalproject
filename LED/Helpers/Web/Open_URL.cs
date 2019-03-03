#region References

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using static System.Console;

#endregion

namespace LED.Helpers.Web
{
    internal class URL
    {
        public static void Open(string _URL)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {_URL.Replace("&", "^&")}") { CreateNoWindow = true });
                }
                else
                {
                    WriteLine("[INFO] As of now, Windows is the only supported OS.");
                }
            }
            catch (Exception X)
            {
                WriteLine($"[ERROR] {X.Message}");
            }
        }
    }
}
