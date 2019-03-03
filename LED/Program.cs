/*
 * Name: LightFX
 * Developed by: Dominik Kulawiak
 * Copyright: (C) 2019
 * Special thanks to:
 *      (GitHub) @iKadmium for thier sACN DMX Library
 *          Library rewritten with only one-way communication possible
 *      (GitHub) @filoe for thier visualzation samples for thier CSCore Library 
 *          Visualization sample used with modfifications to the data process
*/

#region References

using System;
using System.Text;

using LED.Core;
using LED.Core.Network;
using LED.Helpers.Console;

using static LED.Values.Constants;

#endregion

namespace LED
{
    class Program
    {
        public static Http Server     = null;
        public static Parser CMD      = null;
        public static SACN_Sender DMX = null;
        public static Factory Effects = null;

        public static byte[] data = new byte[CHANNELS];

        public static bool stop = false;

        static void Main()
        {
            #region Prerequisites

            Console.OutputEncoding = Encoding.UTF8;
            Console.Clear();
            Title.Refresh();
            Console.SetOut(new Modifier());

            if (!Permission.Administrator())
            {
                Console.WriteLine("Application must be run with administrator privileges.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            #endregion

            #region Initialization

            Console.WriteLine("Initializing Server... \n");

            CMD     = new Parser();
            Effects = new Factory();
            DMX     = new SACN_Sender();
            Server  = new Http();

            #endregion
        }
    }
}
