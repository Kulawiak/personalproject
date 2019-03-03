#region References 

using LED.Values;

using System.Threading;

#endregion

namespace LED.Effects
{
    public class Chase
    {
        public Chase()
        {
            new Thread(() =>
            {
                byte[] array = new byte[Constants.CHANNELS];

                //Set only the first byte to red
                array[0] = 255;

                Program.data = array;

                Program.stop = false;
                while (!Program.stop)
                {
                    _Extensions.Scroll();
                }
            }).Start();
        }
    }
}
