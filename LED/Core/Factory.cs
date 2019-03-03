#region References 

using System;

using LED.Effects;

#endregion

namespace LED.Core
{
    public class Factory
    {
        public Chase Chase             = null;
        public Fade Fade               = null;
        public July_4 July_4           = null;
        public Police Police           = null;
        public Rainbow Rainbow         = null;
        public Streetlight Streetlight = null;
        public Visualization Audio     = null;
        public Xmas Xmas               = null;

        public Factory()
        {
            Console.WriteLine("Effect Factory Class → OK");
        }
    }
}
