#region References 

using System.Linq;

using System.Threading;

#endregion

namespace LED.Effects
{
    public class Fade
    {
        public Fade()
        {
            new Thread(() =>
            {
                byte[] array = Program.data;
                while (array.Max() != 0)
                {
                    // Fades out the colors by level every 10ms until all the colors are 0
                    for (int i = 0; i < array.Length; i++)
                    {
                        if ((array[i] - 1) >= 0)
                        {
                            array[i] -= 1;
                        }
                    }
                    Program.data = array;
                    Thread.Sleep(10);
                }
            }).Start();
        }
    }
}
