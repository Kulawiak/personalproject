#region References 

using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class Streetlight
    {
        public Streetlight()
        {
            new Thread(() =>
            {

                byte[][] Pattern = { Colors.Red, Colors.Yellow, Colors.Green };

                Program.data = _Extensions.Get_Array(Pattern);

                Program.stop = false;
                while (!Program.stop)
                {
                    _Extensions.Scroll();
                }
            }).Start();
        }
    }
}
