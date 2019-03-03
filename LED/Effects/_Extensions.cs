#region References 

using System;
using System.Linq;
using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class _Extensions
    {
        public static void Scroll()
        {
            // Scrolls the whole whole byte array by shifting bytes
            for (int i = 0; i < Constants.LED_COUNT; i++)
            {
                if (Program.stop) break;
                Thread.Sleep(33);
                Program.data = Program.data.Skip(3).Concat(Program.data.Take(3)).ToArray();
            }
        }

        public static byte[] Get_Array(byte[] color)
        {
            // Creates an array from an RGB byte
            byte[] array = new byte[Constants.CHANNELS];
            for (int b = 0; b < 3; b++) for (int i = b; i < Constants.CHANNELS; i += 3) array[i] = color[b];
            return array;
        }

        public static byte[] Get_Array(string color)
        {
            // Creates an array from a color hex string
            byte[] array = new byte[Constants.CHANNELS];
            byte[] _color = GetColor(color);
            for (int b = 0; b < 3; b++) for (int i = b; i < Constants.CHANNELS; i += 3) array[i] = _color[b];
            return array;
        }

        public static byte[] Get_Array(byte[][] Pattern)
        {
            // Creates an array based on a given pattern (jagged array)
            byte[] array = new byte[Constants.CHANNELS];

            // Divides the colors evenly across the array
            for (int Index = 0; Index < Pattern.Length; Index++)
            {
                byte[] Color = Pattern[Index];

                for (int i = (Constants.LED_COUNT / Pattern.Length) * Index; i < (Constants.LED_COUNT / Pattern.Length) * (Index + 1); i++)
                {
                    for (int c = 0; c < 1; c++) array[(i * 3) + c] = Color[0]; //r
                    for (int c = 0; c < 1; c++) array[(i * 3) + c + 1] = Color[1]; //g
                    for (int c = 0; c < 1; c++) array[(i * 3) + c + 2] = Color[2]; //b
                }
            }

            return array;
        }

        static byte[] GetColor(string color)
        {
            // Converts a color hex string to a RGB byte
            return new byte[] {
                Convert.ToByte(Convert.ToInt32(color.Substring(0, 2), 16)),
                Convert.ToByte(Convert.ToInt32(color.Substring(2, 2), 16)),
                Convert.ToByte(Convert.ToInt32(color.Substring(4, 2), 16))
            };
        }

        static void DisplayInfo(byte[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]}, ");
            }
            Console.WriteLine();
            Console.WriteLine($"Array Max: {array.Max()}");
        }

        public static void Stop_All()
        {
            // Global terminator for all the effects
            Program.stop = true;
            if (Program.Effects.Audio != null) Program.Effects.Audio.Stop();
        }
    }
}
