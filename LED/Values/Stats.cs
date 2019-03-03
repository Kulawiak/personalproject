#region References

using System.Collections.Generic;

#endregion

namespace LED.Values
{
    class Stats
    {
        #region Stat Values

        //Ints
        public static int Exceptions, Requests = 0;
        
        //Strings
        public static string Private_IP = null;
        public static string Public_IP = null;

        //Dictonary
        public static Dictionary<int, string> Visitors = null;

        //Bool
        public static bool Error = false;

        #endregion
    }
}
