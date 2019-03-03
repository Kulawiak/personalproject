#region References 

using System.Collections.Generic;

#endregion

namespace LED.Values
{
    class Constants
    {
        #region Constant Values

        // App Info Settings
        public const string Product = "[LFX]";
        public const string Name = "LightFX";
        public const string Build = "Beta 16";

        // Visualization Settings
        public static int FPS = 60;
        public static int LED_COUNT = 100; //max is 448
        public static int CHANNELS = LED_COUNT * 3;
        public const bool Multicast = false;
        public static string IP = "192.168.0.101";
        public static int Scroll = 0;

        // Static Debug Settings
        public static bool Debug = false;

        // Espressif OUIs
        public const string Company = "Espressif Inc.";
        public static List<string> Espressif_OUIs = new List<string>()
        {
            "18-FE-34", "24-0A-C4", "24-6F-28", "24-B2-DE", "2C-3A-E8", "2C-F4-32", "30-AE-A4",
            "3C-71-BF", "4C-11-AE", "54-5A-A6", "5C-CF-7F", "60-01-94", "68-C6-3A", "80-7D-3A",
            "84-0D-8E", "84-F3-EB", "90-97-D5", "A0-20-A6", "A4-7B-9D", "A4-CF-12", "AC-D0-74",
            "B4-E6-2D", "BC-DD-C2", "C4-4F-33", "CC-50-E3", "D8-A0-1D", "DC-4F-22", "EC-FA-BC"
        };

        #endregion
    }
}
