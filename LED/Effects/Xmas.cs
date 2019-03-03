#region References 

using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class Xmas
    {
        public Xmas()
        {
            new Thread(() =>
            {
                byte[][] Pattern = { Colors.Red, Colors.Green, Colors.Red, Colors.Green, Colors.Red, Colors.Green, Colors.Red, Colors.Green, Colors.Red, Colors.Green };

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
