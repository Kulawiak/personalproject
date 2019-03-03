#region References 

using System.Threading;

using LED.Values;

#endregion

namespace LED.Effects
{
    public class July_4
    {
        public July_4()
        {
            new Thread(() =>
            {
                byte[][] Pattern = { Colors.Red, Colors.White, Colors.Blue };

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
